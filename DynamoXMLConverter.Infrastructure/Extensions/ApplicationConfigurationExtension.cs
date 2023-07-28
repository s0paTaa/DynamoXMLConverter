using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Hangfire;
using DynamoXMLConverter.Infrastructure.Hangfire.Jobs;
using DynamoXMLConverter.Infrastructure.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using SimpleInjector;

namespace DynamoXMLConverter.Infrastructure.Extensions
{
    public static class ApplicationConfigurationExtension
    {
        public static WebApplication ConfigureApp(this WebApplication app, Container container)
        {
            app.ConfigureSimpleInjector(container);

            app.UseMiddleware<UnhandledExceptionsMiddleware>();
            app.UseMiddleware<AntiXssMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/dashboard");
            app.MapHangfireDashboard();

            ConfigureHangfireJobs(container);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }

        private static void ConfigureHangfireJobs(Container container)
        {
            IHangfireJobManager hangfireJobManager = container.GetInstance<IHangfireJobManager>();
            hangfireJobManager.RegisterRecurringJob<RemoveExpiredFilesJob>(Constants.Hangfire.Jobs.RemoveExpiredFilesJobName, Cron.Hourly());
        }
    }
}
