using DynamoXMLConverter.Domain.Models.Shared;

namespace DynamoXMLConverter.Models.Preview
{
    public class JsonPreviewModel : BaseResponseModel
    {
        public string JsonText { get; set; } = string.Empty;
    }
}
