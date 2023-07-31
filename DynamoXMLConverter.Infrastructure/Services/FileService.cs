using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Entities;
using DynamoXMLConverter.Domain.Models.File;
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
            JsonFile file = await _fileRepository.GetAllAsNoTracking()
                .Where(f => f.Identifier.Equals(identifier))
                .SingleOrDefaultAsync();

            if (file == null)
            {
                return false;
            }

            await _fileRepository.Delete(file);

            return true;
        }

        public Task<JsonFileModel?> GetJsonFileByIdentifier(Guid Identifier)
        {
            return _fileRepository.GetAllAsNoTracking()
                .Where(f => f.Identifier.Equals(Identifier))
                .Select(f => new JsonFileModel
                {
                    FileName = f.Name,
                    JsonText = f.Value
                }).FirstOrDefaultAsync();
        }

        public async Task<ProcessFilesModel> ProcessFiles(IEnumerable<IFormFile> formFiles)
        {
            var responseModel = new ProcessFilesModel();
            var filesValidationResult = await _fileHelperService.ValidateUploadedFiles(formFiles);

            if (!filesValidationResult.IsSucceed)
            {
                responseModel.ErrorMessage = filesValidationResult.ErrorMessage;
                return responseModel;
            }

            ICollection<JsonFile> entities = new List<JsonFile>();
            IDictionary<string, string> convertedFiles = new Dictionary<string, string>();

            foreach (var file in formFiles)
            {
                string fileNameWithoutExtension = file.FileName.Split('.')[0];
                string convertedXml = await ConvertXmlFileToJson(file);
                convertedFiles[fileNameWithoutExtension] = convertedXml;
            }

            var convertedFileValues = convertedFiles.Values.ToList();
            bool hasExistingFileInDatabse = await _fileRepository.GetAllAsNoTracking()
                .AnyAsync(f => convertedFileValues.Contains(f.Value));

            if (hasExistingFileInDatabse)
            {
                responseModel.ErrorMessage = Constants.File.ErrorMessages.FileAlreadyExist;
                return responseModel;
            }

            foreach (var file in convertedFiles)
            {             
                JsonFile entity = new JsonFile(file.Key, file.Value, DateTime.UtcNow.AddDays(Constants.File.FileLifetimeInDays));
                entities.Add(entity);
            }

            await _fileRepository.AddRange(entities);

            responseModel.IsSucceed = true;
            responseModel.Files = entities.Select(e => new FileDisplayModel
            {
                FileIdentifier = e.Identifier.ToString(),
                FileName = e.Name
            });

            return responseModel;
        }

        #region PrivateMethods
        private async Task<string> ConvertXmlFileToJson(IFormFile file)
        {
            var xmlDoc = new XmlDocument();

            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.ResetPosition();
                xmlDoc.Load(stream);
            }

            return JsonConvert.SerializeXmlNode(xmlDoc);
        }
        #endregion
    }
}
