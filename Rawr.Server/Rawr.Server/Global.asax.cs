using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rawr.Server
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("crossdomain.xml");
			routes.IgnoreRoute("clientaccesspolicy.xml");
			routes.IgnoreRoute("favicon.ico");

			routes.MapRoute(
				"Default", // Route name
				"{characterRegionServer}", 
				//"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Character", action = "Load", characterRegionServer = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
		}
	}
}