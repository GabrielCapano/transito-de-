using System.Web.Mvc;

namespace Web.Areas.Ocorrencias
{
    public class OcorrenciasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ocorrencias";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ocorrencias_default",
                "Ocorrencias/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
