using System.Web.Mvc;

namespace Web.Areas.CEP
{
    public class CEPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CEP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CEP_default",
                "CEP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
