using DynamoXMLConverter.Domain.DependencyResolver;
using DynamoXMLConverter.Domain.Hangfire;
using DynamoXMLConverter.Domain.Models.Configuration;
using DynamoXMLConverter.Domain.Models.Hangfire;
using DynamoXMLConverter.Infrastructure.DependencyResolver;
using DynamoXMLConverter.Infrastructure.Hangfire;
using DynamoXMLConverter.Infrastructure.Hangfire.Jobs;
using Microsoft.Extensions.Configuration;
using nClam;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Reflection;

namespace DynamoXMLConverter.Infrastructure
{
    public class SimpleInjectorConfig
    {
        public static void RegisterCommon(Container container, ConfigurationManager configuration)
        {
            Lifestyle lifestyle = new AsyncScopedLifestyle();
            Assembly assembly = Assembly.Load("DynamoXMLConverter.Infrastructure");
            ClamClientConfiguration clamClientConfiguration = configuration.GetSection("ClamClientConfig").Get<ClamClientConfiguration>() 
                ?? throw new ArgumentNullException("ClamClient configurations are not found");

            var reccuringJobRegistrations = assembly.GetExportedTypes()
                .Where(service => service.GetInterfaces().Any(i => i == typeof(IHangfireJob)))
                .ToList();

            var registrations = assembly.GetExportedTypes()
                .Where(service => service.GetInterfaces().Any(i => i.Name.Substring(1) == service.Name)
                    && service.GetConstructors().FirstOrDefault()?.GetParameters().Any(p => p.ParameterType == typeof(string) || p.ParameterType.IsValueType) == false)
                .Select(service => new
                {
                    Interface = service.GetInterfaces().First(i => i.Name.Substring(1) == service.Name),
                    Service = service
                });

            var hangfireJobManager = registrations.Where(r => r.Interface == typeof(IHangfireJobManager));
            var dependencyInjectionResolver = registrations.Where(r => r.Interface == typeof(IDependencyInjectionResolver));

            registrations = registrations.Except(hangfireJobManager);
            registrations = registrations.Except(dependencyInjectionResolver);

            foreach (var registration in registrations)
            {
                container.Register(registration.Interface, registration.Service, lifestyle);
            }
          
            foreach (var reccuringJob in reccuringJobRegistrations)
            {
                container.Register(reccuringJob, reccuringJob, lifestyle);
            }

            container.Register(typeof(IHangfireJobManager), typeof(HangfireJobManager), Lifestyle.Transient);
            container.Register<IClamClient>(() => new ClamClient(clamClientConfiguration.Host, clamClientConfiguration.Port), lifestyle);
        }
    }
}
