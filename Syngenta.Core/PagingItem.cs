using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core
{
    public class PagingItem
    {
        public PagingItem(int page, bool current, string url)
        {
            this.Page = page;
            this.IsCurrent = current;
            this.PagingUrl = url;
        }

        public int Page { get; set; }
        public bool IsCurrent { get; set; }
        public string PagingUrl { get; set; }
    }
}