namespace DynamoXMLConverter.Domain.Models.Http
{
    public class HttpMessageModel
    {
        public HttpMessageModel(string message, string? stackTrace)
        {
            ExceptionMessage = message;
            ExceptionStackTrace = stackTrace;
        }

        public string ExceptionMessage { get; set; }
        public string? ExceptionStackTrace { get; set; }
    }
}
