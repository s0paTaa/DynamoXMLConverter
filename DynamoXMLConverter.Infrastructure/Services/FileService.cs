using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Models.File;
using DynamoXMLConverter.Domain.Models.Shared;
using DynamoXMLConverter.Domain.Repositories;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Xml;

namespace DynamoXMLConverter.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileHelperService _fileHelperService;

        public FileService(IFileRepository fileRepository, IFileHelperService fileHelperService)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _fileHelperService = fileHelperService ?? throw new ArgumentNullException(nameof(fileHelperService));
        }

        public async Task<bool> DeleteByIdentifier(Guid identifier)
        {
            DynamoFile? file = await _fileRepository.GetAllAsNoTracking()
                .Where(f => f.ID.Equals(identifier))
                .SingleOrDefaultAsync();

            if (file == null)
            {
                return false;
            }

            await _fileRepository.Delete(file);

            return true;
        }

        public Task<DynamoFileModel?> GetJsonFileByIdentifier(Guid Identifier)
        {
            return _fileRepository.GetAllAsNoTracking()
                .Where(f => f.ID.Equals(Identifier))
                .Select(f => new DynamoFileModel
                {
                    FileName = f.Name,
                    Text = f.Value,
                    ContentType = f.ContentType,
                    Extension = f.Extension
                }).FirstOrDefaultAsync();
        }

        public async Task<ProcessFilesModel> ProcessFiles(IEnumerable<IFormFile> formFiles)
        {
            ProcessFilesModel responseModel = new();
            BaseResponseModel filesValidationResult = await _fileHelperService.ValidateUploadedFiles(formFiles);

            if (!filesValidationResult.IsSucceed)
            {
                responseModel.ErrorMessage = filesValidationResult.ErrorMessage;
                return responseModel;
            }

            ICollection<DynamoFile> entities = [];
            List<DynamoFileModel> convertedFiles = new();

            foreach (IFormFile file in formFiles)
            {
                string fileNameWithoutExtension = file.FileName.Split('.')[0];
                string convertedText, targetContentType, targetExtension;

                if (Constants.File.AllowedMimeTypes.ContainsKey(file.ContentType))
                {
                    switch (file.ContentType)
                    {
                        case "application/json":
                            convertedText = await ConvertJsonFileToXml(file);
                            targetContentType = "text/xml";
                            targetExtension = ".xml";
                            break;
                        case "text/xml":
                            convertedText = await ConvertXmlFileToJson(file);
                            targetContentType = "application/json";
                            targetExtension = ".json";
                            break;
                        default:
                            throw new Exception("Invalid contentType");
                    }

                    convertedFiles.Add(new DynamoFileModel
                    {
                        FileName = fileNameWithoutExtension,
                        Text = convertedText,
                        ContentType = targetContentType,
                        Extension = targetExtension
                    });
                }
                else
                {
                    throw new Exception("Invalid contentType");
                }
            }

            foreach (var file in convertedFiles)
            {
                DynamoFile entity = new(file, DateTime.UtcNow.AddDays(Constants.File.FileLifetimeInDays));
                entities.Add(entity);
            }

            await _fileRepository.AddRange(entities);

            responseModel.IsSucceed = true;
            responseModel.Files = entities.Select(e => new FileDisplayModel
            {
                FileIdentifier = e.ID.ToString(),
                FileName = e.Name
            });

            return responseModel;
        }

        // Add try catch and logging
        #region PrivateMethods
        private async Task<string> ConvertXmlFileToJson(IFormFile file)
        {
            using MemoryStream stream = new();
            XmlDocument xmlDoc = new();
            await file.CopyToAsync(stream);
            stream.ResetPosition();
            xmlDoc.Load(stream);

            return JsonConvert.SerializeXmlNode(xmlDoc);
        }

        private async Task<string> ConvertJsonFileToXml(IFormFile file)
        {
            using StreamReader reader = new(file.OpenReadStream());
            string json = await reader.ReadToEndAsync();      
            XmlDocument? xmlDoc = JsonConvert.DeserializeXmlNode(json);

            return xmlDoc.InnerXml;
        }
        #endregion
    }
}
