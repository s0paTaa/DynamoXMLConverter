using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.DependencyResolver;
using DynamoXMLConverter.Domain.Logging;
using DynamoXMLConverter.Domain.Models.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Web;

namespace DynamoXMLConverter.Infrastructure.Middlewares
{
    public class UnhandledExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDependencyInjectionResolver _resolver;
        private readonly IWebHostEnvironment _enviroment;

        public UnhandledExceptionsMiddleware(RequestDelegate next, IDependencyInjectionResolver resolver, IWebHostEnvironment environment)
        {
            _next = next;
            _resolver = resolver;
            _enviroment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                IDynamoLogger logger = _resolver.GetService<IDynamoLogger>();

                // Log exception in the database
                await logger.LogException(ex);

                string message = JsonConvert.SerializeObject(new HttpErrorResponse("500", ex.Message), Formatting.Indented, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.Cookies.Append(Constants.Cookies.ErrorFromMiddleware, HttpUtility.UrlEncode(message), new CookieOptions 
                {
                    Secure = true,
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                });
                httpContext.Response.Redirect("Home/Error", true);
            }
        }
    }
}
