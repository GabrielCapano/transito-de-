using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Base;
using Web.Base;
using AutoMapper;
using Web.Areas.Ocorrencias.Models;
using BO = Business;
using Web.Filters;

namespace Web.Areas.Ocorrencias.Controllers
{
    [Authorize(Roles="Operador")]
    public class OcorrenciaController : BaseController
    {
        //
        // GET: /Ocorrencias/Ocorrencia/


        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult Get(PaginationObject pagination, string name)
        {
            var response = Business.Ocorrencia.Instance.GetAllOcorrencias(ref pagination, name);

            var rlResponse = new ResponseObject
            {
                Messages = response.Messages,
                Status = response.Status,
                Pagination = response.Pagination,
                Objects = Mapper.Map<List<OcorrenciaViewModel>>(response.Objects).Select(a=>(object)a).ToList()
            };

            return Json(rlResponse);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateAjax]
        public ActionResult Save(OcorrenciaViewModel model)
        {
            var response = new Repository.Base.ResponseObject();

            if (ModelState.IsValid)
            {
                var realModel = Mapper.Map<Model.Models._CET.Ocorrencia>(model);
                response = BO.Ocorrencia.Instance.SaveOcorrencia(realModel);
            }

            return Json(response);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.isEdit = true;
            var model = Mapper.Map<OcorrenciaViewModel>(BO.Ocorrencia.Instance.GetById(id));

            return View("Form", model);
        }
    }
}
