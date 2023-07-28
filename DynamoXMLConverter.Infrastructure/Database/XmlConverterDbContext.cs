using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DynamoXMLConverter.Infrastructure.Database
{
    public class XmlConverterDbContext : DbContext
    {
        public XmlConverterDbContext(DbContextOptions<XmlConverterDbContext> options)
            : base(options)
        {          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureEntities();
        }

        public virtual void SetEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry<TEntity>(entity).State = entityState;
        }

        public DbSet<JsonFile> JsonFiles { get; set; }
    }
}
