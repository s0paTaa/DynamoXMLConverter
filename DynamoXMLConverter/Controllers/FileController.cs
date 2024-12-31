using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Models.Shared;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace DynamoXMLConverter.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        public IActionResult Download()
        {
            return View(InitializeModel());
        }

        [HttpPost]
        public async Task<IActionResult> Download([FromForm] string identifier)
        {
            if (Guid.TryParse(identifier, out Guid value) == false)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.InvalidIdentifier;
                return RedirectToAction("Download");
            }

            var file = await _fileService.GetJsonFileByIdentifier(value);

            if (file == null)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.FileNotFound;
                return RedirectToAction("Download");
            }

            byte[] fileBytes = Encoding.UTF8.GetBytes(file.Text);

            return File(fileBytes, file.ContentType, string.Concat(file.FileName, file.Extension));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] string identifier)
        {
            if (Guid.TryParse(identifier, out Guid value) == false)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.InvalidIdentifier;
                return RedirectToAction("Download");
            }

            bool isDeleteSuccessful = await _fileService.DeleteByIdentifier(value);

            if (!isDeleteSuccessful)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.FileNotFound;
                return RedirectToAction("Download");
            }

            TempData[Constants.File.RouteParams.DeleteSuccessMessage] = Constants.File.SuccessMessages.DeleteSuccessMessage;
            return RedirectToAction("Download");
        }

        private BaseResponseModel InitializeModel()
        {
            var model = new BaseResponseModel();
            model.ErrorMessage = TempData.TryGetValue<string>(Constants.File.RouteParams.ErrorMessage);
            model.SuccessMessage = TempData.TryGetValue<string>(Constants.File.RouteParams.DeleteSuccessMessage);

            // Always clear the temp data. By default when it is observed is automaticaly cleared but in the same session.
            // So if the session is not expired, the data will stay and the page may display same data
            TempData.Clear();

            return model;
        }
    }
}
