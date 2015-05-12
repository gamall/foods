using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Syngenta.Web.FoodStandards.Spanish.Filters;
using Syngenta.Web.FoodStandards.Spanish.Models;
using Syngenta;
using Syngenta.Core;
using Syngenta.Core.Contracts.Data;
using Syngenta.Core.Libraries;
using System.Configuration;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;

namespace Syngenta.Web.FoodStandards.Spanish.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [RequireHttps]
    public class AccountController : SyngentaBaseController
    {

        public AccountController(ISyngentaUow uow)
        {
            Uow = uow;
        }

        //
        // GET: /Account/Index

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (WebSecurity.CurrentUserId > 0 && !IsExpired())
            {
                if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
                {
                    return Redirect("~/admin/dashboard");
                }
                else
                {
                    return Redirect("foreword");
                }
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string returnUrl, FormCollection form)
        {
            Thread.Sleep(100);

            var ipAddress = HttpContext.Request.UserHostAddress;
            bool validEmail = form["Email"].Contains("@" + ConfigurationManager.AppSettings["login_register_email_domain"]);

            // validate IP address
            var halfAnHourAgo = DateTime.Now.AddMinutes(-30);
            var failedIP = Uow.FailedLoginAttempts.GetAll().Where(x => x.IpAddress == ipAddress).Where(x => x.Datestamp > halfAnHourAgo);

            if (failedIP.Count() < 5)
            {
                // validate account
                if (ModelState.IsValid && form["Email"].IsEmailAddress() && validEmail
                    && WebSecurity.Login(form["Email"], form["Password"], persistCookie: false)
                    && WebSecurity.IsConfirmed(form["Email"])
                    && !Uow.Users.GetByEmail(form["Email"]).IsDisabled)
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        // populate PreviousLoginDate and LastLoginDate 
                        var entityUser = Uow.Users.GetByEmail(form["Email"]);

                        entityUser.PreviousLoginDate = entityUser.LastLoginDate;
                        entityUser.LastLoginDate = DateTime.Now;

                        Uow.Users.Update(entityUser);
                        Uow.Commit();

                        if (Roles.IsUserInRole(form["Email"], "Admin"))
                        {
                            return Redirect("~/admin/dashboard");
                        }
                        else
                        {
                            return Redirect("foreword");
                        }
                    }
                }
                else
                {
                    // validate email
                    if (validEmail)
                    {
                        var failedEmail = Uow.FailedLoginAttempts.GetByMail(form["Email"]).Where(x => x.Datestamp > halfAnHourAgo);

                        if (failedEmail.Count() < 4)
                        {
                            SaveFailedLoginAttempt(form["Email"], ipAddress);
                        }
                        else
                        {
                            // disable account
                            var entityUser = Uow.Users.GetByEmail(form["Email"]);
                            entityUser.IsDisabled = true;
                            Uow.Users.Update(entityUser);
                            Uow.Commit();
                        }
                    }
                    else
                    {
                        SaveFailedLoginAttempt(form["Email"], ipAddress);
                        //// check ip address
                        //if (failedIP.Count() < 4)
                        //{
                        //    SaveFailedLoginAttempt(form["Email"], ipAddress);
                        //}
                        //else
                        //{
                        //    SaveFailedLoginAttempt(form["Email"], ipAddress);
                        //}
                    }

                }
            }

            WebSecurity.Logout();

            ViewBag.Message = "Error";
            return View();

        }

        //
        // GET: /Account/Foreword

        public ActionResult Foreword()
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                return View("foreword");
            }
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOut()
        {
            WebSecurity.Logout();

            return View("loggedout");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user, FormCollection form)
        {
            Thread.Sleep(100);

            if (ModelState.IsValid && form["Email"].IsEmailAddress()
                && form["Email"].Contains("@" + ConfigurationManager.AppSettings["login_register_email_domain"]))
            {
                // Attempt to register the user
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                var randomCharIndex = new Random().Next(0, 25);

                var securePassword = new SecureString();

                securePassword = ConvertToSecureString(Membership.GeneratePassword(6, 0) + new Random().Next(0, 9).ToString() + chars[randomCharIndex]);

                try
                {
                    var confirmationToken = WebSecurity.CreateUserAndAccount(user.Email, ConvertToString(securePassword), new
                    {
                        Surname = user.Surname,
                        Forename = user.Forename
                    }, true);

                    //something for later, we will change the confirmation token in database to all lower case.

                    var url = ConfigurationManager.AppSettings["WebsiteBaseUrl"] + "registrationconfirmation/" + confirmationToken.ToString();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Please click the link below to confirm your registration: ");
                    sb.AppendLine(url);
                    sb.AppendLine("Your Syngenta temporary password is: " + ConvertToString(securePassword));
                    sb.AppendLine("Thank you.");

                    string emailBody = sb.ToString();
                    string emailSubject = "Syngenta Registration";

                    Utils.EmailHelper mailer = new Utils.EmailHelper();
                    mailer.sendConfirmationEmail(user.Email, emailBody, emailSubject);

                    if (!Roles.RoleExists("Member"))
                    {
                        Roles.CreateRole("Member");
                    }

                    Roles.AddUserToRole(user.Email, "Member");

                    return View("RegistrationPending", user);

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            else
            {
                ViewBag.Message = "Error";
            }

            return View();
        }

        //
        // GET: /Account/RegistrationConfirmation

        [AllowAnonymous]
        public ActionResult RegistrationConfirmation(string token)
        {
            if (WebSecurity.ConfirmAccount(token))
            {
                return View("RegistrationConfirmation");
            }
            else
            {
                return View("RegistrationConfirmationError");
            }
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgottenPassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword(FormCollection form)
        {
            string emailAddress = form["Email"];
            string emailSubject = "Syngenta - Reset Password";

            Thread.Sleep(100);

            if (!String.IsNullOrEmpty(emailAddress))
            {
                if (WebSecurity.IsConfirmed(emailAddress))
                {
                    string confirmationToken = WebSecurity.GeneratePasswordResetToken(emailAddress, 120);

                    //something for later, we will change the reset token in database to all lower case.

                    var url = ConfigurationManager.AppSettings["WebsiteBaseUrl"] + "resetpassword/" + confirmationToken;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Please click the link below to create your new password:");
                    sb.AppendLine(url);
                    sb.AppendLine("Thank you.");

                    string emailBody = sb.ToString();

                    Utils.EmailHelper mailer = new Utils.EmailHelper();
                    mailer.sendConfirmationEmail(emailAddress, emailBody, emailSubject);
                }

                ViewBag.Message = "Email has been sent, please check your email.";

            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            ViewBag.Token = token;
            var userId = WebSecurity.GetUserIdFromPasswordResetToken(token);
            if (userId == -1) return View("ResetPasswordFailure");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token, FormCollection form)
        {
            Thread.Sleep(100);

            if (WebSecurity.ResetPassword(token, form["NewPassword"])
                && (form["NewPassword"] == form["ConfirmPassword"])
                && IsValidPassword(form["ConfirmPassword"]))
            {
                return View("ResetPasswordSuccess");
            }

            return View("ResetPasswordFailure");
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordFailure()
        {
            return View();
        }

        #region Helpers

        private SecureString ConvertToSecureString(string input)
        {
            var secureStr = new SecureString();

            if (input.Length > 0)
            {
                foreach (var c in input.ToCharArray()) secureStr.AppendChar(c);
            }

            return secureStr;
        }

        private string ConvertToString(SecureString secureStr)
        {
            var unmanagedStr = IntPtr.Zero;

            try
            {
                unmanagedStr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureStr);

                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedStr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedStr);
            }
        }

        private bool IsValidPassword(string input)
        {
            string strRegex = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30})";

            Regex re = default(Regex);
            re = new Regex(strRegex);

            return re.IsMatch(input) ? true : false;
        }

        private bool IsExpired()
        {
            var user = Uow.Users.GetById(WebSecurity.CurrentUserId);

            return (user.LastLoginDate.Value.AddHours(3) < DateTime.Now) ? true : false;
        }

        private void SaveFailedLoginAttempt(string emailAddress, string ipAddress)
        {
            // keep a log of failed login attempts, including the email address entered, IP address used, and datestamp;
            var failed = new FailedLoginAttempt()
            {
                EmailAddress = emailAddress,
                IpAddress = ipAddress,
                Datestamp = DateTime.Now,
                IsAdmin = Roles.IsUserInRole(emailAddress, "Admin")
            };

            Uow.FailedLoginAttempts.Add(failed);
            Uow.Commit();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }


        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Ya existe la dirección de correo electrónico. Ingrese una dirección de correo electrónico diferente.";

                //case MembershipCreateStatus.DuplicateEmail:
                //    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                //case MembershipCreateStatus.InvalidPassword:
                //    return "The password provided is invalid. Please enter a valid password value.";

                //case MembershipCreateStatus.InvalidEmail:
                //    return "The e-mail address provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidAnswer:
                //    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidQuestion:
                //    return "The password retrieval question provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.InvalidUserName:
                //    return "The user name provided is invalid. Please check the value and try again.";

                //case MembershipCreateStatus.ProviderError:
                //    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                //case MembershipCreateStatus.UserRejected:
                //    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
