using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Repository.Base;
using Web.Areas.User.Models;
using Web.Base;
using Web.Filters;
using BO = Business;
using MdUser = Model.Models._User;
using Repo = Repository;

namespace Web.Areas.User.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult Create()
        {
            ViewBag.isEdit = false;
            return View("Form", new UserViewModel());
        }

        public ActionResult Edit(int id)
        {
            ViewBag.isEdit = true;
            var model = Mapper.Map<UserViewModel>(BO.User.Instance.GetById(id));
            
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult GetUsers(PaginationObject pagination, string name)
        {
            return Json(BO.User.Instance.GetAllActiveUsers(ref pagination, name));
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateAjax]
        public ActionResult Save(UserViewModel model)
        {
            var response = new Repo.Base.ResponseObject();            
            
            if (ModelState.IsValid)
            {
                model.AuthLevels = model.AuthLevels.Where(a => a.IsSelected).ToList();
                var realModel = Mapper.Map<MdUser.User>(model);
                response = BO.User.SaveUser(realModel);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Remove(List<int> id)
        {
            return Json(BO.User.Remove(id.ToArray()));
        }

    }
}
