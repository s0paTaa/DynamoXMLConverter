using DynamoXMLConverter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamoXMLConverter.Infrastructure.Database.Configurations
{
    public class DynamoFileConfiguration : IEntityTypeConfiguration<DynamoFile>
    {
        public void Configure(EntityTypeBuilder<DynamoFile> builder)
        {
            builder.HasKey(p => p.ID).HasName("PK_Identifier");
            builder.HasIndex(p => p.ID).IsUnique();
            builder.Property(p => p.ID).HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.ContentType).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Extension).IsRequired().HasMaxLength(10);
        }
    }
}
