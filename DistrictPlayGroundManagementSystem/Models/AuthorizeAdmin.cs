using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistrictPlayGroundManagementSystem.Models
{
    public class AuthorizeAdmin: AuthorizeAttribute
    {
        protected override  bool AuthorizeCore(HttpContextBase httpContext)
        {
            var UserId = httpContext.Session["AdminId"];
            if(UserId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext authorizationContext)
        {
            authorizationContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary(
                            new
                            {
                                area = "",
                                controller = "Home",
                                action = "Login"
                            })
                        );
        }
    }
}