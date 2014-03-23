using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Repository.Base;
using Web.Areas.Email.Models;
using Web.Base;
using Web.Filters;
using EmailConfiguration = Business.Email;

namespace Web.Areas.Email.Controllers
{
    [Authorize]
    public class EmailConfigurationController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View("Index", new EmailConfigurationViewModel());
        }

        public ActionResult EditCurrent()
        {
            ViewBag.FormType = "Editar";
            var configuration = EmailConfiguration.Instance.GetCurrentConfiguration();

            var model = Mapper.Map<EmailConfigurationViewModel>(configuration);

            return View("Index", model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.FormType = "Editar";
            var configuration = EmailConfiguration.Instance.GetById(id);

            var model = Mapper.Map<EmailConfigurationViewModel>(configuration);

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Get(PaginationObject pagination, string name)
        {
            return Json(EmailConfiguration.Instance.GetAllEmailConfiguration(ref pagination, name));
        }


        [HttpPost, ValidateAntiForgeryToken, ValidateAjax]
        public ActionResult Save(EmailConfigurationViewModel model)
        {
            var response = new ResponseObject();
            response.Messages.Add("");

            if (ModelState.IsValid)
            {
                var md = Mapper.Map<Model.Models._Email.EmailConfiguration>(model);
                md.ConfigurationType = (Model.Models._Email.ConfigurationType)Enum.Parse(typeof(ConfigurationType), model.ConfigurationType);
                
                response = EmailConfiguration.Instance.SaveEmailConfiguration(md);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Remove(List<int> id)
        {
            return Json(EmailConfiguration.RemoveEmailConfiguration(id.ToArray()));
        }

    }
}
