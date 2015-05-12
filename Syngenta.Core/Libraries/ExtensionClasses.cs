using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;

namespace Syngenta.Core.Libraries
{
    public static class Extensions
    {
        public static bool IsEmailAddress(this string input)
        {
            string strRegex = "^([a-zA-Z0-9_\\-\\.\\']+)@((\\[[0-9]{1,3}" + "\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\" + ".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
            Regex re = default(Regex);
            re = new Regex(strRegex);
            return re.IsMatch(input);
        }
    }
}
