namespace DynamoXMLConverter.Domain.Logging
{
    public interface IDynamoLogger
    {
        Task LogException(Exception exception);
    }
}
