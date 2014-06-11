using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bus.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //string host = System.Web.HttpContext.Current.Request.Url.Host;
            //throw new Exception(host);
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Global_Error_" + DateTime.Now.Ticks + ".txt")))
            {
                tw.WriteLine("===========================================================================================");
                tw.WriteLine("ExecptionMessage:" + ex.Message);
                tw.WriteLine(ex.Source);
                tw.WriteLine(ex.StackTrace);
                tw.WriteLine("===========================================================================================");
                tw.Flush();
                tw.Close();
            }
        }
    }
}