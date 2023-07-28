using DynamoXMLConverter.Domain.Hangfire;
using DynamoXMLConverter.Domain.Models.Hangfire;
using Hangfire;

namespace DynamoXMLConverter.Infrastructure.Hangfire
{
    public class HangfireJobManager : IHangfireJobManager
    {
        public void RegisterRecurringJob<THangfireJob>(string explicitJobId, string jobRunTime, bool startJobImmediately = false)
            where THangfireJob : IHangfireJob
        {
            RecurringJob.AddOrUpdate<THangfireJob>(explicitJobId, h => h.Execute(), jobRunTime);

            if (startJobImmediately)
            {
                RecurringJob.TriggerJob(explicitJobId);
            }
        }
    }
}
