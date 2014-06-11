using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bus.Core
{
   public class UserLoginAttribute:AuthorizeAttribute
    {
       protected override bool AuthorizeCore(HttpContextBase httpContext)
       {
           
           if (TheCMS.Common.Cookie.GetCookie("UserID") == null || TheCMS.Common.Cookie.GetCookie("UserID").ToString() == "")
           {
               
               httpContext.Response.Write("<script>window.location.href='/home/Login';</script>");
               httpContext.Response.End();
           }
           return true;
       }
    }
}
