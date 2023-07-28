using DynamoXMLConverter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamoXMLConverter.Infrastructure.Database.Configurations
{
    public class JsonFileConfiguration : IEntityTypeConfiguration<JsonFile>
    {
        public void Configure(EntityTypeBuilder<JsonFile> builder)
        {
            builder.HasKey(p => p.Identifier).HasName("PK_Identifier");
            builder.HasIndex(p => p.Identifier).IsUnique();
            builder.Property(p => p.Identifier).HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }
    }
}
