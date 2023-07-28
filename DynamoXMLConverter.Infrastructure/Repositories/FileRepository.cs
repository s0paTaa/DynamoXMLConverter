using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Repositories;
using DynamoXMLConverter.Infrastructure.Database;
using DynamoXMLConverter.Infrastructure.Repositories.Base;

namespace DynamoXMLConverter.Infrastructure.Repositories
{
    public class FileRepository : BaseRepository<JsonFile>, IFileRepository
    {
        public FileRepository(XmlConverterDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
