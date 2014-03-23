using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models._Alert;
using Web.Base;
using Web.Proxy;

namespace Web.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/

        public bool NewAlert()
        {
            var alert = new AlertProxy();
            alert.NewAlert(new Alert("Novo alerta", "Testando Alerta", AlertType.Info));
            return true;
        }

    }
}
