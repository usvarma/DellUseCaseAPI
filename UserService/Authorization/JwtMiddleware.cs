using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Service;

namespace UserService.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService, IUserServices userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = await tokenService.VerifyTokenAsync(token);
            if (userId)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = tokenService.GetUserInfoFromTokenAsync(token).Result;
            }

            await _next(context);
        }
    }
}
