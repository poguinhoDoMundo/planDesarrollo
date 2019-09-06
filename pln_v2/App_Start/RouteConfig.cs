using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace pln_v2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            /*
            routes.MapRoute(
                name: "Factores",
                url: "{Home}/{Factores}",
                 defaults: new { controller = "Home", action = "Factores" } 
            );

            routes.MapRoute(
                name: "Proyectos",
                url: "{Home}/{Proyectos}/{id}",
                defaults: new { controller = "Home", action = "Proyectos" }
            );
            
            routes.MapRoute(
                name: "About",
                url: "{Home}/{About}",
                defaults: new { controller = "Home", action = "About" }
            );

           
            routes.MapRoute(
                name: "Login",
                url: "{Home}/{Login}",
                defaults: new { controller = "Home", action = "Login" }
            );
            

            routes.MapRoute(
                name: "MisProyectos",
                url: "{Home}/{MisProyectos}/{id}",
                defaults: new { controller = "Home", action = "MisProyectos" }
            );

            routes.MapRoute(
                name: "Propuesta",
                url: "{Home}/{Propuesta}",
                defaults: new { controller = "Home", action = "Propuesta" }
            );

            routes.MapRoute(
                name: "ProyectoDetalle",
                url: "{Home}/{ProyectoDetalle}/{id}/{viene}",
                defaults: new { controller = "Home", action = "ProyectoDetalle" }
            );

            routes.MapRoute(
                name: "resultOk",
                url: "{Home}/{resultOk}",
                defaults: new { controller = "Home", action = "resultOk" }
            );

            routes.MapRoute(
                name: "Revision",
                url: "{Home}/{Revision}",
                defaults: new { controller = "Home", action = "Revision" }
            );

            routes.MapRoute(
                name: "RevisionDetalle",
                url: "{Home}/{RevisionDetalle}/{id}",
                defaults: new { controller = "Home", action = "RevisionDetalle" }
            );

            routes.MapRoute(
                name: "SearchProyectos",
                url: "{Home}/{SearchProyectos}/{id}",
                defaults: new { controller = "Home", action = "SearchProyectos" }
            );*/

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Factores", id = UrlParameter.Optional }
            );

        }
    }
}