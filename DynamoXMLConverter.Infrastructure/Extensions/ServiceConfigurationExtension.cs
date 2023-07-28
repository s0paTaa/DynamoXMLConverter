using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.DependencyResolver;
using DynamoXMLConverter.Infrastructure.Database;
using DynamoXMLConverter.Infrastructure.DependencyResolver;
using Hangfire;
using Hangfire.SimpleInjector;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace DynamoXMLConverter.Infrastructure.Extensions
{
    public static class ServiceConfigurationExtension
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services, Container container, ConfigurationManager configuration)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = Constants.File.MultipartBodyLengthInBytes;
            });

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation()
                    .AddViewComponentActivation();
            });

            SimpleInjectorConfig.RegisterCommon(container, configuration);

            services.AddSingleton<IDependencyInjectionResolver>(new DependencyInjectionResolver(container));

            services.AddDbContext<XmlConverterDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("XmlConverterDatabase"));
            });

            services.AddHttpContextAccessor();

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseActivator(new SimpleInjectorJobActivator(container))
                .UseSqlServerStorage(configuration.GetConnectionString("XmlConverterDatabase"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                    TryAutoDetectSchemaDependentOptions = false
                }));

            services.AddHangfireServer();

            return services;
        }
    }
}
