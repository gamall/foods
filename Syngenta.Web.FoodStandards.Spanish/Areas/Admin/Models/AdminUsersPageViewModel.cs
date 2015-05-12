using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syngenta.Data.Repositories;
using Syngenta.Core;
using System.Web.Mvc;
using System.Web.Security;

namespace Syngenta.Web.FoodStandards.Spanish.Areas.Admin.Models
{
    public class AdminUsersPageViewModel
    {
        public IList<User> Items { get; private set; }
        public PagedList<User> PagedItems { get; private set; }

        public PagedList<User> RegisteredUsers { get; private set; }

        public AdminUsersPageViewModel(PagedList<User> users)
        {
            this.Items = users.Items;
            this.PagedItems = users;
        }
    }
}