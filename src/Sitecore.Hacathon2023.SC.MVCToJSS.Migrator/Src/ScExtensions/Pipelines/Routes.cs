using Sitecore.Mvc.Pipelines.Loader;
using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Pipelines
{
    public class Routes : InitializeRoutes
    {
        public override void Process(PipelineArgs args)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected virtual void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "MVCToJSS", // Route name
                "MVCToJSS/{controller}/{action}" // URL with parameters
                );
        }
    }
}