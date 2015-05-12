using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Syngenta.Core;
using Syngenta.Core.Contracts;
using Syngenta.Core.Contracts.Data;

namespace Syngenta.Web.FoodStandards.Controllers
{

    public class ApiBaseController : ApiController
    {
        protected ISyngentaUow Uow { get; set; }
    }

}
