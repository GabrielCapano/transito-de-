using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BO = Business;
using System.Web.Mvc;

namespace Web.Base
{
    [Authorize]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            var user = BO.User.Instance.GetLoggedUser();
            ViewBag.UserName = user != null ? user.Name : "ND";
            ViewBag.UserId = user != null ? user.Id : 0;
            ViewBag.UserAuth = user != null ? String.Join(";", user.AuthLevels.Select(a => a.Label)) : "";
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new ServiceStackJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        public class ServiceStackJsonResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                if (Data != null)
                {
                    response.Write(JsonConvert.SerializeObject(Data, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy hh:mm:ss" }));
                }
            }
        }    
    }
}