using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Repositories;
using DynamoXMLConverter.Infrastructure.Database;
using DynamoXMLConverter.Infrastructure.Repositories.Base;

namespace DynamoXMLConverter.Infrastructure.Repositories
{
    public class UnhandledExceptionLogRepository : BaseRepository<UnhandledExceptionLog>, IUnhandledExceptionLogRepository
    {
        public UnhandledExceptionLogRepository(XmlConverterDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
