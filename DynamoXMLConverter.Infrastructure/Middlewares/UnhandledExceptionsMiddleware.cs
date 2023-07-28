using DynamoXMLConverter.Domain.DependencyResolver;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DynamoXMLConverter.Domain.Models.Http;

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
                // Use _resolver to get service and store the error in Database
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string message = _enviroment.IsProduction() ? "Something went wrong." : ex.Message;

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new HttpErrorResponse("server_error", message), new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            }
        }
    }
}
