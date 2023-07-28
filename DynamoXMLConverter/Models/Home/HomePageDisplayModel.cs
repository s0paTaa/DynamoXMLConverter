using DynamoXMLConverter.Domain.Models.File;

namespace DynamoXMLConverter.Models.Home
{
    public class HomePageDisplayModel
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public IEnumerable<FileDisplayModel> UploadedFiles { get; set; } = Array.Empty<FileDisplayModel>();
    }
}
