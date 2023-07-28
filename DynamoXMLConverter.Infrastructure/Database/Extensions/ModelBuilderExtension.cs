using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DynamoXMLConverter.Infrastructure.Database.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void ConfigureEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Remove cascade delete on all relationships in the database
            // This logic should be the last in the Seed method to ensure every relation will not have cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
