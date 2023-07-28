using Newtonsoft.Json;

namespace DynamoXMLConverter.Domain.Models.Http
{
    public class HttpErrorResponse
    {
        public HttpErrorResponse(string code)
        {
            Code = code;
        }

        public HttpErrorResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public HttpErrorResponse(string code, HttpMessageModel model)
        {
            Code = code;
            MessageHolder = model;
        }

        public HttpErrorResponse(string code, dynamic data)
        {
            Code = code;
            Data = data;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpMessageModel MessageHolder { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }
    }
}
