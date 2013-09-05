using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvc.Controllers;
using NavigationRoutes;
using PteMvc.Controllers;
using RunnerupWeb.Controllers;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<HomeController>("Admin", c => c.Index())
                .AddChildRoute<DemoController>("Map", c => c.Map());

            //routes.MapNavigationRoute<ExampleLayoutsController>("Example Layouts", c => c.Starter())
            //      .AddChildRoute<ExampleLayoutsController>("Marketing", c => c.Marketing())
            //      .AddChildRoute<ExampleLayoutsController>("Fluid", c => c.Fluid())
            //      .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
            //    ;
        }
    }
}
