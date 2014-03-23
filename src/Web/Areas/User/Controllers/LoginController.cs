using System;
using System.Linq;
using System.Web.Mvc;
using Repository.Base;
using Web.Areas.User.Models;

namespace Web.Areas.User.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Ocorrencia", new { Area = "Ocorrencias" });

            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel paramsLogin, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!string.IsNullOrEmpty(paramsLogin.Email) && !string.IsNullOrEmpty(paramsLogin.Password))
            {
                #region Check if the user exists

                var response = Business.User.Instance.CheckLogin(paramsLogin.Email, paramsLogin.Password);

                if (response.Status)
                {
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Dashboard", new { Area = "GTP" });

                    return Redirect(returnUrl);
                }

                ModelState.AddModelError("Erro", response.Messages.First());

                if (Request.IsAjaxRequest())
                {
                    return Json(response);
                }

                return RedirectToAction("Index", "Login", paramsLogin);

                #endregion
            }

            return View("Index");
        }

        #region Forgot Password

        public ActionResult Recovery(string token)
        {
            var user = Business.User.Instance.GetUserByToken(token);

            if (user != null)
                return View("ForgotPassword", new ForgotPasswordModel { UserId = user.Id });

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Business.User.Instance.GetById(model.UserId);

                if (user != null)
                    Business.User.Instance.CreateUser(user, model.Password);
            }

            return View("Index", new LoginViewModel());
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var response = new ResponseObject();

            var user = Business.User.Instance.GetByEmail(email);

            if (user != null)
                response = Business.User.Instance.ForgotPassword(user);
            else
            {
                response.Status = false;
                response.Messages.Add("Não foi possível localizar o email informado");
            }

            return Json(response);
        }

        #endregion

        public ActionResult Logout()
        {
            Business.User.Instance.Logout();

            return RedirectToAction("Index", "Login");
        }

    }
}
