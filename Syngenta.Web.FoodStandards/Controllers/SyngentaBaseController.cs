using Syngenta.Core.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Syngenta.Web.FoodStandards.Controllers
{
    public class SyngentaBaseController : Controller
    {
        protected ISyngentaUow Uow { get; set; }
    }
}
