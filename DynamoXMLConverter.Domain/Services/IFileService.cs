﻿using DynamoXMLConverter.Domain.Models.File;
using Microsoft.AspNetCore.Http;

namespace DynamoXMLConverter.Domain.Services
{
    public interface IFileService
    {
        Task<ProcessFilesModel> ProcessFiles(IEnumerable<IFormFile> formFiles);
        Task<DynamoFileModel?> GetJsonFileByIdentifier(Guid identifier);
        Task<bool> DeleteByIdentifier(Guid identifier);
    }
}
