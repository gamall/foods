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
using System.Configuration;
using System.IO;

namespace Syngenta.Web.FoodStandards.Spanish.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [RequireHttps]
    public class BookController : SyngentaBaseController
    {
        public BookController(ISyngentaUow uow)
        {
            Uow = uow;
        }

        public ActionResult Select()
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Assembly()
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                ViewBag.IntroPageNumber = GetNumberOfImages(true);
                ViewBag.AdvancedPageNumber = GetNumberOfImages(false);

                var incompleteBook = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

                if (incompleteBook != null)
                {
                    ViewBag.SelectedPagesIntro = GetSelectedPages(incompleteBook.Id, 1);
                    ViewBag.SelectedPagesAdvanced = GetSelectedPages(incompleteBook.Id, 2);

                    return View(incompleteBook);
                }

                else
                {
                    return View(new UserBook());
                }
            }
        }

        public ActionResult Preview()
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                var entityBook = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

                if (entityBook != null)
                {
                    IEnumerable<UserBookPage> ubp;
                    ubp = Uow.UserBookPages.GetUserBookPageByUserBookId(entityBook.Id);

                    ViewBag.SelectedPages = GetSelectedPages(entityBook.Id, 0);

                    return View(entityBook);
                }
                else
                {
                    return View(new UserBook());
                }
            }
        }

        [HttpPost]
        public ActionResult Assembly(UserBook ub, FormCollection form)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                ViewBag.IntroPageNumber = GetNumberOfImages(true);
                ViewBag.AdvancedPageNumber = GetNumberOfImages(false);

                bool isFromSelectPage = form["IsFromSelectPage"] == "1" ? true : false;

                if (isFromSelectPage)
                {
                    var incompleteBook = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

                    if (incompleteBook != null)
                    {
                        ViewBag.SelectedPagesIntro = GetSelectedPages(incompleteBook.Id, 1);
                        ViewBag.SelectedPagesAdvanced = GetSelectedPages(incompleteBook.Id, 2);
                    }

                    ub.HighQuality = form["IsHighQuality"] == "true" ? true : false;
                    ub.LowQuality = form["IsHighQuality"] == "true" ? false : true;

                    ViewBag.SelectedLevel = form["IsIntro"] == "true" ? "intro" : "advanced";
                }
                else
                {
                    SaveBook(ub, true);
                }

                return View(ub);
            }
        }

        public ActionResult Download(UserBook ub)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Download(UserBook ub, FormCollection form)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                SaveBook(ub, false);

                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveCompleteBook(FormCollection form)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                bool isIntro, isHighQuality;
                isIntro = form["IsIntro"] == "true" ? true : false;
                isHighQuality = form["IsHighQuality"] == "true" ? true : false;

                string pageLevel = isIntro ? "intro" : "advanced";

                List<string> files = new List<string>();

                foreach (string file in Directory.GetFiles(Server.MapPath("~/resources/images/") + pageLevel))
                {
                    files.Add(Path.GetFileNameWithoutExtension(file));
                }

                string bookPages = String.Join(",", files.ToArray());

                var book = Uow.UserBooks.GetLatestCompletedUserBook(WebSecurity.CurrentUserId);

                book.HighQuality = isHighQuality;
                book.LowQuality = isHighQuality ? false : true;
                book.IsCompleted = true;

                if (book != null) Uow.UserBooks.Update(book);
                else Uow.UserBooks.Add(book);

                Uow.Commit();

                return View("Download");
            }
        }

        [HttpPost]
        public ActionResult DownloadPdf(UserBook ub)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                var completedBook = Uow.UserBooks.GetLatestCompletedUserBook(WebSecurity.CurrentUserId);

                if (completedBook != null)
                {
                    string strPage = GetSelectedPages(completedBook.Id, 0);

                    string[] arrPage = strPage.Split(',').ToArray();

                    Utils.PdfHelper pdf = new Utils.PdfHelper();

                    if (completedBook.HighQuality)
                    {
                        pdf.CreatePDF(arrPage, completedBook.User.Email, true);
                    }
                    if (completedBook.LowQuality)
                    {
                        pdf.CreatePDF(arrPage, completedBook.User.Email, false);
                    }

                    pdf.ZipTempFiles(completedBook.User.Email);

                    GetZipFile(completedBook.User.Email);

                    pdf.DeleteTempFiles(completedBook.User.Email);
                }

                return View("DownloadPdf");
            }
        }

        public ActionResult DownloadPdf()
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ViewPage(string url)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                var result = ViewEngines.Engines.FindView(ControllerContext, url, null);

                if (result.View != null)
                {
                    return View(url);
                }
                else
                {
                    Response.Status = "404 Not found";
                    Response.StatusCode = 404;

                    return View("Error404");
                }
            }
        }

        #region helpers

        private bool IsExpired()
        {
            var user = Uow.Users.GetById(WebSecurity.CurrentUserId);

            return (user.LastLoginDate.Value.AddHours(3) < DateTime.Now) ? true : false;
        }

        private void SaveBook(UserBook ub, bool isPreview)
        {
            if (Uow.UserBooks.IsUserExist(WebSecurity.CurrentUserId) != null)
            {
                //var entityBook = isPreview ? Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId) : Uow.UserBooks.GetLatestCompletedUserBook(WebSecurity.CurrentUserId);
                var entityBook = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

                if (entityBook != null)
                {
                    entityBook.LowQuality = ub.LowQuality;
                    entityBook.HighQuality = ub.HighQuality;
                    entityBook.IsCompleted = isPreview ? false : true;

                    Uow.UserBooks.Update(entityBook);
                    Uow.Commit();
                }
                else
                {
                    ub.UserId = WebSecurity.CurrentUserId;
                    ub.IsCompleted = isPreview ? false : true;
                    Uow.UserBooks.Add(ub);
                    Uow.Commit();
                }
            }
            else
            {
                ub.UserId = WebSecurity.CurrentUserId;
                ub.IsCompleted = isPreview ? false : true;
                Uow.UserBooks.Add(ub);
                Uow.Commit();
            }
        }

        private string GetNumberOfImages(bool isIntro)
        {
            if (isIntro)
            {
                return Directory.GetFiles(Server.MapPath("~/resources/images/intro")).Count().ToString();
            }
            else
            {
                return Directory.GetFiles(Server.MapPath("~/resources/images/advanced")).Count().ToString();
            }
        }

        private string GetSelectedPages(int userBookId, int levelOption)
        {
            IEnumerable<UserBookPage> ubp = Uow.UserBookPages.GetUserBookPageByUserBookId(userBookId);

            List<string> listPages = new List<string>();

            if (levelOption == 0) // all levels
            {
                foreach (var page in ubp.OrderByDescending(o => o.IsIntroductory.ToString()).OrderBy(o => o.PageNumber))
                {
                    if (page.IsIntroductory)
                    {
                        listPages.Add(page.PageNumber.ToString() + "i");
                    }
                    else
                    {
                        listPages.Add(page.PageNumber.ToString() + "a");
                    }
                }
            }
            else // 1: intro, 2: advanced
            {
                bool isIntro = levelOption == 1 ? true : false;

                foreach (var page in ubp.Where(o => o.IsIntroductory == isIntro).OrderBy(o => o.PageNumber))
                {
                    listPages.Add(page.PageNumber.ToString());
                }
            }

            return string.Join(",", listPages.ToArray());
        }

        private void GetZipFile(string userEmail)
        {
            Stream iStream;
            byte[] buffer = new byte[10000];
            int length;
            long dataToRead;
            string filePath = Server.MapPath("~/resources/temp/" + userEmail + "_syngenta.zip");
            string fileName = Path.GetFileName(filePath);

            if (System.IO.File.Exists(filePath))
            {
                iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                dataToRead = iStream.Length;

                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Length", dataToRead.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=Syngenta-ebook-" + DateTime.Now.ToString("yyyy-MM-dd-HHmm") + ".zip");

                while (dataToRead > 0)
                {
                    length = iStream.Read(buffer, 0, 10000);

                    Response.OutputStream.Write(buffer, 0, length);
                    Response.Flush();

                    dataToRead -= length;
                }

                if (iStream != null) iStream.Close();

                Response.End();

            }
        }

        #endregion
    }
}
