using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCDemo {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //
            // -- add dynamic routes from Contensive add-ons and link-alias
            // -- these routes have no wildcards so add them first, before other routes
            //  -- 
            // -- the Contensive app can have any name, but the sitename included here as an option
            using (var cp = new  Contensive.Processor.CPClass(System.Web.Hosting.HostingEnvironment.SiteName)) {
                foreach (var kvp in cp.routeMap.routeDictionary) {
                    try {
                        routes.MapRoute(
                            name: kvp.Value.virtualRoute,
                            url: kvp.Value.virtualRoute,
                            defaults: new { controller = "Home", action = "Contensive" }
                        );
                    } catch (Exception ex) {
                        cp.Site.ErrorReport(ex, "Unexpected exception adding virtualRoute [" + kvp.Key + "]");
                    }
                }
            }
            //
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
