using DynamoXMLConverter.Domain.DependencyResolver;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DynamoXMLConverter.Domain.Models.Http;
using DynamoXMLConverter.Domain.Logging;

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
                var logger = _resolver.GetService<IDynamoLogger>();

                // Log exception in the database
                await logger.LogException(ex);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string message = string.Empty;
                
                if (_enviroment.IsProduction())
                {
                    message = JsonConvert.SerializeObject(new HttpErrorResponse("server_error", "Something went wrong"), Formatting.Indented, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                }
                else
                {
                    message = JsonConvert.SerializeObject(
                        new HttpErrorResponse("server_error", new HttpMessageModel(ex.Message, ex.StackTrace)), 
                        Formatting.Indented, 
                        new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                }

                await httpContext.Response.WriteAsync(message, System.Text.Encoding.ASCII);
            }
        }
    }
}
