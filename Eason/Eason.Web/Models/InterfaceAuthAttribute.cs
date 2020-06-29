using Eason.EntityFramework.Entities.Authorization;
using Eason.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Eason.Web.Models
{
    public class InterfaceAuthAttribute : AuthorizeAttribute
    {


        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            //if (context.HttpContext.Request.IsAjaxRequest())
            //{
            UrlHelper urlHelper = new UrlHelper(context.RequestContext);
            context.Result = new JsonpResult()
            {
                Data = new { login = false },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //}
            //else
            //    base.HandleUnauthorizedRequest(context);

        }
    }
}