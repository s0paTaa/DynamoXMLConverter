using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Models.Hangfire;
using DynamoXMLConverter.Domain.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace DynamoXMLConverter.Infrastructure.Hangfire.Jobs
{
    [DisableConcurrentExecution(timeoutInSeconds: Constants.Hangfire.DisableConcurrentExecutionTimeoutInSeconds)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public class RemoveExpiredFilesJob : IHangfireJob
    {
        private readonly IFileRepository _fileRepository;

        public RemoveExpiredFilesJob(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        }

        public void Execute()
        {
            RemoveExpiredFiles().Wait();
        }

        private async Task RemoveExpiredFiles()
        {
            DateTime dateTimeInUtc = DateTime.UtcNow;
            IEnumerable<DynamoFile> expiredJsonFiles = await _fileRepository.GetAllAsNoTracking()
                .Where(f => f.DateExpire < dateTimeInUtc)
                .ToListAsync();

            if (expiredJsonFiles.Any())
            {
                await _fileRepository.DeleteRange(expiredJsonFiles);
            }
        }
    }
}
