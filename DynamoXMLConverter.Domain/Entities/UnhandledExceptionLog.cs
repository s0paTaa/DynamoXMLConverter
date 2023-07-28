namespace DynamoXMLConverter.Domain.Entities
{
    public class UnhandledExceptionLog
    {
        public UnhandledExceptionLog()
        {           
        }

        public UnhandledExceptionLog(string message, string? source = null, string? stackTrace = null)
        {
            Message = message;
            StackTrace = stackTrace;
            Source = source;
            DateCreated = DateTime.UtcNow;
        }

        public long Id { get; private set; }
        public string Message { get; private set; }
        public string? StackTrace { get; private set; }
        public string? Source { get; private set; }
        public DateTime DateCreated { get; private set; }
    }
}
