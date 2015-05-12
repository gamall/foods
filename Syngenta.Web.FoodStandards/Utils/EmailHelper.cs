using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace Syngenta.Web.FoodStandards.Utils
{
    public class EmailHelper
    {

        //toaddress, emailbody will come from somewhere else as a parameter

        public void sendConfirmationEmail(string strMailToAddress, string strMailBody, string strMailSubject)
        {
            string strToEmail = ConfigurationManager.AppSettings["SendRegistrationEmailTo"].ToString();
            string strFromEmail = ConfigurationManager.AppSettings["senderEmail"].ToString();
            string strFromName = ConfigurationManager.AppSettings["senderName"].ToString();
            string strSMTPServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
            //string strSMTPUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
            //string strSMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();

            SmtpClient smtp = new SmtpClient(strSMTPServer);

            //if (strSMTPUserName != "" && strSMTPPassword != "")
            //{
            //    smtp.Credentials = new System.Net.NetworkCredential(strSMTPUserName, strSMTPPassword);
            //}

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(strFromEmail, strFromName);

            try
            {
                mail.ReplyToList.Add(new MailAddress(strFromEmail));
            }
            catch (Exception) { }

            mail.Subject = strMailSubject;
            mail.To.Add(strMailToAddress);
            mail.Body = strMailBody;
            mail.IsBodyHtml = false;

            if (strSMTPServer != "")
            {
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception)
                { }
            }

            mail = null;
        }

    }
}