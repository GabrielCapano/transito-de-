using System.Web.Mvc;

namespace Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Redefinição de Senha",
                "User/Login/Recovery/{token}",
                new { action = "Recovery", controller = "Login" }
            );

            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", Controller="Login", id = UrlParameter.Optional }
            );
        }
    }
}
