using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.CEP.Controllers
{
    public class CEPController : BaseController
    {
        //
        // GET: /CEP/CEP/

        public ActionResult GetLogradouroByCEP(int cep)
        {
            return Content("Oi");
        }

    }
}
