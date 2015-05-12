using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Syngenta.Core
{
    public class UserBook
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public string SelectedPagesIntro { get; set; }
        //public string SelectedPagesAdvanced { get; set; }
        public bool LowQuality { get; set; }
        public bool HighQuality { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public virtual ICollection<UserBookPage> UserBookPages { get; set; }

    }
}
