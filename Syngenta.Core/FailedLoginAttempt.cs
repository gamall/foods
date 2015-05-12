using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core
{
    public class FailedLoginAttempt
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string IpAddress { get; set; }
        public DateTime Datestamp { get; set; }
        public bool IsAdmin { get; set; }
    }
}