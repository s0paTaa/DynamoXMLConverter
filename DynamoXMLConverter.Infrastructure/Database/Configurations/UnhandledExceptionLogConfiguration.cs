using DynamoXMLConverter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamoXMLConverter.Infrastructure.Database.Configurations
{
    public class UnhandledExceptionLogConfiguration : IEntityTypeConfiguration<UnhandledExceptionLog>
    {
        public void Configure(EntityTypeBuilder<UnhandledExceptionLog> builder)
        {
            builder.Property(p => p.Message).IsRequired();
            builder.Property(p => p.StackTrace).IsRequired(false);
            builder.Property(p => p.DateCreated).IsRequired();
        }
    }
}
