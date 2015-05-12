using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using Syngenta.Core;
using Syngenta.Core.Contracts.Data;
using Syngenta.Web.FoodStandards.Spanish.Filters;
using Syngenta.Web.FoodStandards.Spanish.Models;

namespace Syngenta.Web.FoodStandards.Spanish.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class EsBookApiController : ApiBaseController
    {

        public EsBookApiController(ISyngentaUow uow)
        {
            Uow = uow;
        }

        public int Post(UserBookPage ubp)
        {
            var ub = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

            if (ub != null)
            {
                ub.LowQuality = ubp.UserBook.LowQuality;
                ub.HighQuality = ubp.UserBook.HighQuality;
                ub.IsCompleted = false;
               
                Uow.UserBooks.Update(ub);
                Uow.Commit();

                ubp.UserBook = ub;
                ubp.UserBookId = ub.Id;
            }
            else
            {
                ubp.UserBook.UserId = WebSecurity.CurrentUserId;
                ubp.UserBook.IsCompleted = false;
               
                Uow.UserBooks.Add(ubp.UserBook);
                Uow.Commit();
                
                ubp.UserBookId = ubp.UserBook.Id;
            }

            ubp.Datestamp = DateTime.Now;

            Uow.UserBookPages.Add(ubp);
            Uow.Commit();

            return ubp.UserBookId;
        }

        public void Delete(UserBookPage ubp)
        {
            var ub = Uow.UserBooks.GetLatestIncompletedUserBook(WebSecurity.CurrentUserId);

            if (ub != null)
            {
                var entityUbp = Uow.UserBookPages.GetUserBookPageId(ub.Id, ubp.PageNumber, ubp.IsIntroductory);

                Uow.UserBookPages.Delete(entityUbp.FirstOrDefault(o => o.PageNumber == ubp.PageNumber));

                Uow.Commit();
            }
        }

    }

}
