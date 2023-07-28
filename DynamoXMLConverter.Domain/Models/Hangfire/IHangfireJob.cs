namespace DynamoXMLConverter.Domain.Models.Hangfire
{
    public interface IHangfireJob
    {
        void Execute();
    }
}
