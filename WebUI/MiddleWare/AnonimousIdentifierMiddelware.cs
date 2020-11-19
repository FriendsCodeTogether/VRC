using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebUI.MiddleWare
{
    public class AnonimousIdentifierMiddelware
    {
        private readonly RequestDelegate _next;

        public AnonimousIdentifierMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cookieExists = context.Request.Cookies.ContainsKey("AnonymousUserId");
            if (!cookieExists)
            {
                context.Response.Cookies.Append("AnonymousUserId", Guid.NewGuid().ToString(), new CookieOptions { HttpOnly = false, IsEssential = true  }); ;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
