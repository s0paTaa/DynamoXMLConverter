using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Repositories;
using DynamoXMLConverter.Infrastructure.Database;
using DynamoXMLConverter.Infrastructure.Repositories.Base;

namespace DynamoXMLConverter.Infrastructure.Repositories
{
    public class FileRepository : BaseRepository<DynamoFile>, IFileRepository
    {
        public FileRepository(XmlConverterDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
