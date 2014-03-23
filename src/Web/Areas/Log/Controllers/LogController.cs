using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Repository.Base;
using Web.Base;

namespace Web.Areas.Log.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Log/Log/

        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult Get(PaginationObject pagination, string name)
        {
            var response = Business.Log.Instance.GetAllLogs(ref pagination, name);

            var rlResponse = new ResponseObject
            {
                Messages = response.Messages,
                Status = response.Status,
                Pagination = response.Pagination,
                Objects = response.Objects
            };

            return Json(rlResponse);
        }

    }
}
