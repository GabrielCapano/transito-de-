using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Model;
using Model.Migrations;
using Repository.Base;
using Web.App_Start;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(); 
            //Database.SetInitializer(
            //new MigrateDatabaseToLatestVersion<DatabaseContext, DbMigrationsConfiguration<DatabaseContext>>());
            //var dbMigrator = new DbMigrator(new Configuration
            //    {
            //        AutomaticMigrationDataLossAllowed = true
            //    });
            //dbMigrator.Update();


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterMaps();


        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
                return;

            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            // retrieve roles from UserData
            if (authTicket != null)
            {
                string[] roles = authTicket.UserData.Split(';');

                if (Context.User != null)
                    Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }
        }

    }
}