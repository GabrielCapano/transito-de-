using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Base;

namespace Web.Extensions
{
    public static class SelectObjectExtension
    {
        public static SelectListItem ConvertToSelectList(this SelectObject obj)
        {
            return new SelectListItem
                {
                    Text = obj.Text,
                    Value = obj.Value
                };
        } 
    }
}