using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DynamoXMLConverter.Infrastructure.Database.Extensions
{
    public static class DatabaseMigratorExtension
    {
        public static async Task<IHost> MigrateDatabase(this IHost webApp)
        {
            var scope = webApp.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<XmlConverterDbContext>();

            try
            {
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(nameof(dbContext), ex);
            }
            finally
            {
                await scope.DisposeAsync();
            }

            return webApp;
        }
    }
}
