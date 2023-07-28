using DynamoXMLConverter.Domain.Models.Shared;
using Microsoft.AspNetCore.Http;

namespace DynamoXMLConverter.Domain.Services
{
    public interface IFileHelperService
    {
        Task<BaseResponseModel> ValidateUploadedFiles(IEnumerable<IFormFile> formFiles);
    }
}
