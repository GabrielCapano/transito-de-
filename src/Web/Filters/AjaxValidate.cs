using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Filters
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel = new
                    {
                        Status = false,
                        Messages = new List<string>()
                    };
                var errorList =
                    modelState.Keys.Where(x => modelState[x].Errors.Count > 0)
                              .Select(x => modelState[x].Errors.Select(y => y.ErrorMessage));

                foreach (var error in errorList)
                {
                    errorModel.Messages.AddRange(error);
                }


                filterContext.Result = new JsonResult()
                    {
                        Data = errorModel
                    };
            }
        }
    }
}