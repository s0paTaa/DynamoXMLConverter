using Microsoft.AspNetCore.Builder;
using SimpleInjector;

namespace DynamoXMLConverter.Infrastructure.Extensions
{
    public static class ConfigureSimpleInjectorExtension
    {
        public static IApplicationBuilder ConfigureSimpleInjector(this IApplicationBuilder app, Container container)
        {
            app.UseSimpleInjector(container);

            container.Verify();

            return app;
        }
    }
}
