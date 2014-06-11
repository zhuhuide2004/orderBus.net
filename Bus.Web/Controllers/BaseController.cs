using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCMS.Common;

namespace Bus.Web.Controllers
{
    public class BaseController : Controller
    {
        public Data.Users umodel = new Data.Users();
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (TheCMS.Common.Cookie.GetCookie("UserID") != null && TheCMS.Common.Cookie.GetCookie("UserID").ToString() != "")
            {
                var userID = TypeConverter.StrToInt(TheCMS.Common.Cookie.GetCookie("UserID").ToString());
                umodel = Data.UsersDB.GETUsers(userID);
            }
        }
        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (TheCMS.Common.Cookie.GetCookie("UserID") != null && TheCMS.Common.Cookie.GetCookie("UserID").ToString() != "")
        //    {
        //        var userID = TypeConverter.StrToInt(TheCMS.Common.Cookie.GetCookie("UserID").ToString());
        //        umodel = Data.UsersDB.GETUsers(userID);
                
        //        //throw new Exception(umodel.ID+"=="+userID);
        //    }
        //    //Response.Redirect("/home/login");
        //    base.OnActionExecuted(filterContext);
        //}
        //protected override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    if (TheCMS.Common.Cookie.GetCookie("UserID") != null && TheCMS.Common.Cookie.GetCookie("UserID").ToString() != "")
        //    {
        //        var userID = TypeConverter.StrToInt(TheCMS.Common.Cookie.GetCookie("UserID").ToString());
        //        umodel = Data.UsersDB.GETUsers(userID);

        //        //throw new Exception(umodel.ID+"=="+userID);
        //    }
        //    base.OnResultExecuting(filterContext);
        //}

    }
}