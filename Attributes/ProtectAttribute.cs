using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProtectorTokenAuth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtectorTokenAuth.Attributes
{
    /// <summary>
    /// Protects the annotated method or class for the specified user roles. Only requests containing 
    /// a protector access token will be able to access these entities.
    /// </summary>
    public class ProtectAttribute : TypeFilterAttribute
    {
        public ProtectAttribute(params string[] roles) : base(typeof(ProtectFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class ProtectFilter : IAuthorizationFilter
    {
        readonly string[] roles;
        private IAuthService authService;

        public ProtectFilter(string[] roles, IAuthService authService)
        {
            this.roles = roles;
            this.authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers.Where(h => h.Key == "ProtectorToken").Select(h => h.Value).FirstOrDefault();

            if (!authService.ValidateAccessToken(token, roles))
            {
                var result = new UnauthorizedResult();
                context.Result = result;
            }
        }
    }
}
