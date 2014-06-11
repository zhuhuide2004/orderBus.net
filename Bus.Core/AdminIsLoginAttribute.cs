using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bus.Core
{
    public class AdminIsLoginAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (TheCMS.Common.Cookie.GetCookie("AdminHash") == null || TheCMS.Common.Cookie.GetCookie("AdminHash").ToString() == "")
            {
                httpContext.Response.Write("<script>window.top.location.href='/Admin/Login';</script>");
            }
            return true;
        }
    }
}
