using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Syngenta.Core;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using WebSupergoo.ABCpdf7;

namespace Syngenta.Web.FoodStandards.Spanish.Utils
{
    public class PdfHelper
    {
        public void CreatePDF(string[] arrPage, string userEmail, bool isHighQuality)
        {
            Doc docFinal = new Doc();
            Doc docSingle = new Doc();

            string res = isHighQuality ? "HR" : "LR";
            string fileName = ConfigurationManager.AppSettings["SyngentaBookTitle"] + "_" + res + "_" + userEmail + ".pdf";
            string strLevel, level, pdfFile = "";
            string language = ConfigurationManager.AppSettings["WebsiteLanguage"];

            foreach (string page in arrPage)
            {
                if (!String.IsNullOrEmpty(page))
                {
                    strLevel = (page.Substring(page.Length - 1) == "i") ? "intro" : "advanced";
                    level = (strLevel == "intro") ? "background" : strLevel;
                    pdfFile = page.Substring(0, page.Length - 1);
                    if (!String.IsNullOrEmpty(pdfFile) && int.Parse(pdfFile) < 10) pdfFile = "0" + pdfFile;

                    docSingle.Read(HttpContext.Current.Server.MapPath("~/resources/pdf/") + res + "/" + strLevel + "/" + language + "-" + level + "-" + res + "-" + pdfFile + ".pdf");
                }
                docFinal.Append(docSingle);
            }

            docFinal.Save(HttpContext.Current.Server.MapPath("~/resources/temp/" + fileName));

        }

        public void ZipTempFiles(string userEmail)
        {
            FastZip fz = new FastZip();
            fz.CreateZip(HttpContext.Current.Server.MapPath("~/resources/temp/" + userEmail + "_syngenta.zip"), HttpContext.Current.Server.MapPath("~/resources/temp/"), false, userEmail + ".pdf");
        }

        public void DeleteTempFiles(string userEmail)
        {
            foreach (string filePath in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/resources/temp/")))
            {
                if (filePath.Contains(userEmail))
                    File.Delete(filePath);
            }
        }
    }
}