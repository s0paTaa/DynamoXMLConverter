using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Models.Shared;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using nClam;
using System.Net;

namespace DynamoXMLConverter.Infrastructure.Services
{
    public class FileHelperService : IFileHelperService
    {
        private readonly IClamClient _clamClient;
        private readonly string[] _possibleFileExtensions = { ".xml" };

        public FileHelperService(IClamClient clamClient)
        {
            _clamClient = clamClient ?? throw new ArgumentNullException(nameof(clamClient));
        }

        public async Task<BaseResponseModel> ValidateUploadedFiles(IEnumerable<IFormFile> formFiles)
        {
            var responseModel = new BaseResponseModel();
            var doesAllFilesExceedsTheMaximumBodyLength = formFiles.Select(f => f.Length).Sum() > Constants.File.MultipartBodyLengthInBytes;

            if (doesAllFilesExceedsTheMaximumBodyLength)
            {
                responseModel.ErrorMessage = "Files exceeds the maximum capacity of 256 MB";
                return responseModel;
            }

            foreach (var file in formFiles)
            {
                var fileProcessResponse = await ValidateFile(file);

                if (!fileProcessResponse.IsSucceed)
                {
                    responseModel.ErrorMessage = fileProcessResponse.ErrorMessage;
                    return responseModel;
                }
            }

            responseModel.IsSucceed = true;

            return responseModel;
        }

        private async Task<BaseResponseModel> ValidateFile(IFormFile file)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var responseModel = new BaseResponseModel();

                if (file == null)
                {
                    responseModel.ErrorMessage = "File not found";
                    return responseModel;
                }

                string trustedFileName = WebUtility.HtmlEncode(file.FileName);
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(extension) || _possibleFileExtensions.Contains(extension) == false)
                {
                    responseModel.ErrorMessage = string.Concat("You can upload only ", string.Join(',', _possibleFileExtensions), " files");
                    return responseModel;
                }

                if (file.Length == 0 || file.Length > Constants.File.MultipartBodyLengthForFileInBytes)
                {
                    responseModel.ErrorMessage = $"Selected file [{trustedFileName}] is broken or exceeds the limit of 4 MB";
                    return responseModel;
                }

                await file.CopyToAsync(stream);

                if (await _clamClient.ValidateFileStream(stream) == false)
                {
                    responseModel.ErrorMessage = $"Virus detected in {trustedFileName}";
                    return responseModel;
                }

                responseModel.IsSucceed = true;

                return responseModel;
            }
        }
    }
}
