using DynamoXMLConverter.Domain.Models.Hangfire;

namespace DynamoXMLConverter.Domain.Hangfire
{
    public interface IHangfireJobManager
    {
        void RegisterRecurringJob<THangfireJob>(string explicitJobId, string jobRunTime, bool startJobImmediately = false) where THangfireJob : IHangfireJob;
    }
}
