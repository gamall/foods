using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core
{
    public class UserBookPage
    {
        public int Id { get; set; }
        public int UserBookId { get; set; }
        public int PageNumber { get; set; }
        public bool IsIntroductory { get; set; }
        public DateTime Datestamp { get; set; }

        [ForeignKey("UserBookId")]
        public virtual UserBook UserBook { get; set; }
    }
}