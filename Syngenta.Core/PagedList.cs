using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syngenta.Core
{
    public class PagedList<T>
    {

        public PagedList(IList<T> items, int totalItems, int currentPage, int pageSize)
        {
            this.Items = items;
            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;

            this.LastPageNumber = (int)Math.Ceiling((double)totalItems / (double)pageSize);

        }

        public IList<PagingItem> Pages
        {
            get
            {
                IList<PagingItem> items = new List<PagingItem>();

                for (int i = 0; i < this.LastPageNumber; i++)
                {
                    var page = i + 1;
                    if (page == this.CurrentPage)
                    {
                        items.Add(new PagingItem(page, true, (page == 1 ? "" : "page=" + page.ToString())));
                    }
                    else
                    {
                        items.Add(new PagingItem(page, false, (page == 1 ? "" : "page=" + page.ToString())));
                    }
                }

                return items;
            }
        }

        public IList<T> Items { get; private set; }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int LastPageNumber { get; private set; }
        public int PageSize { get; private set; }


        /// <summary>
        /// Stat - index of the starting item on this page (e.g. showing 10-20 of 100 where 10 is the start index)
        /// </summary>
        public int StartIndex
        {
            get
            {
                if (TotalItems < PageSize) return 1;
                return (PageSize * (CurrentPage - 1)) + 1;
            }
        }


        /// <summary>
        /// Stat - index of the ending item on this page (e.g. showing 10-20 of 100 where 20 is the end index)
        /// </summary>
        public int EndIndex
        {
            get
            {
                if (TotalItems < PageSize) return TotalItems;
                if ((PageSize * CurrentPage) > TotalItems) return TotalItems;
                return (PageSize * CurrentPage);
            }
        }

        public string NextPage
        {
            get
            {
                return "page=" + (this.CurrentPage + 1).ToString();
            }
        }

        public string LastPage
        {
            get
            {
                int page = int.Parse(Math.Ceiling((double)this.TotalItems / (double)this.PageSize).ToString());
                if (page == 1) return "";
                return "page=" + page.ToString();
            }
        }

        public string PreviousPage
        {
            get
            {
                return (this.CurrentPage == 2 ? "" : "page=" + (this.CurrentPage - 1).ToString());
            }
        }

        public bool PreviousPageVisible
        {
            get
            {
                return this.CurrentPage > 1;
            }
        }

        public bool NextPageVisible
        {
            get
            {
                return this.CurrentPage < this.LastPageNumber;
            }
        }

    }
}
