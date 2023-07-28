using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using DynamoXMLConverter.Models.Preview;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXMLConverter.Controllers
{
    public class PreviewController : Controller
    {
        private readonly IFileService _fileService;

        public PreviewController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        public IActionResult ShowJsonFile()
        {
            return View(InitializeModel());
        }

        [HttpPost]
        public async Task<IActionResult> ShowJsonFile([FromForm] string identifier)
        {
            if (Guid.TryParse(identifier, out Guid value) == false)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.InvalidIdentifier;
                return RedirectToAction("ShowJsonFile");
            }

            var file = await _fileService.GetJsonFileByIdentifier(value);

            if (file == null)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = Constants.File.ErrorMessages.FileNotFound;
                return RedirectToAction("ShowJsonFile");
            }

            TempData[Constants.File.RouteParams.JsonText] = file.JsonText;
            return RedirectToAction("ShowJsonFile");
        }

        private JsonPreviewModel InitializeModel()
        {
            var model = new JsonPreviewModel();
            model.ErrorMessage = TempData.TryGetValue<string>(Constants.File.RouteParams.ErrorMessage);
            model.JsonText = TempData.TryGetValue<string>(Constants.File.RouteParams.JsonText);

            // Always clear the temp data. By default when it is observed is automaticaly cleared but in the same session.
            // So if the session is not expired, the data will stay and the page may display same data
            TempData.Clear();

            return model;
        }
    }
}
