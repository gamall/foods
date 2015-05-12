using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core
{
    public class User
    {
        
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? PreviousLoginDate { get; set; }

        public virtual ICollection<UserBook> UserBooks { get; set; }

    }
}
