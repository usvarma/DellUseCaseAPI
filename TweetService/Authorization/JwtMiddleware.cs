using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UserService.Models;

namespace TweetService.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //var baseUrl = new Uri("https://localhost:44358");
            var verifyTokenUrl = new Uri("https://localhost:44358/api/v1.0/tweets/users/verifytoken");
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Token tokenObj = new() { TokenString = token};
            var response = await httpClient.PostAsJsonAsync(verifyTokenUrl, tokenObj);
            var userExists = await response.Content.ReadFromJsonAsync<bool>();
            //var userId = await tokenService.VerifyTokenAsync(token);
            if (userExists)
            {
                var getUserInfoUrl = new Uri("https://localhost:44358/api/v1.0/tweets/users/GetUserInfo");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var getUserInfoResponse = await httpClient.PostAsJsonAsync(getUserInfoUrl, tokenObj);
                var user = await getUserInfoResponse.Content.ReadFromJsonAsync<User>();
                // attach user to context on successful jwt validation
                context.Items["User"] = user;
            }

            await _next(context);
        }
    }
}
