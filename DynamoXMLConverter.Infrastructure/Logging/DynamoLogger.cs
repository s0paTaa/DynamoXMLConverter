using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Logging;
using DynamoXMLConverter.Domain.Repositories;

namespace DynamoXMLConverter.Infrastructure.Logging
{
    public class DynamoLogger : IDynamoLogger
    {
        private readonly IUnhandledExceptionLogRepository _exceptionLogRepository;

        public DynamoLogger(IUnhandledExceptionLogRepository unhandledExceptionLogRepository)
        {
            _exceptionLogRepository = unhandledExceptionLogRepository ?? throw new ArgumentNullException(nameof(unhandledExceptionLogRepository));
        }

        public async Task LogException(Exception exception)
        {
            var newLog = new UnhandledExceptionLog(exception.Message, exception.Source, exception.StackTrace);

            await _exceptionLogRepository.Add(newLog);
        }
    }
}
