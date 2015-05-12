using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syngenta.Core
{
    public class UserModificationLog
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }
        public string AdminIPAddress { get; set; }
        public DateTime Datestamp { get; set; }
    }
}
