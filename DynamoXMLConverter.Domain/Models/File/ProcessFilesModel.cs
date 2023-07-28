using DynamoXMLConverter.Domain.Models.Shared;

namespace DynamoXMLConverter.Domain.Models.File
{
    public class ProcessFilesModel : BaseResponseModel
    {
        public IEnumerable<FileDisplayModel> Files { get; set; } = Array.Empty<FileDisplayModel>();
    }
}
