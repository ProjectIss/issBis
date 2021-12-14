
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bis.Custom
{
    public static class Display
    {
        public static string Name
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return (HttpContext.Current.User as CustomPrincipal).Name;
                }
                else return "";
            }
        }

        public static string Role
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return (HttpContext.Current.User as CustomPrincipal).Role;
                }
                else return "";
            }
        }
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return (HttpContext.Current.User as CustomPrincipal).EId;
                }
                else return 0;
            }
        }
    }
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Auth",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}