using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Models.File;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using DynamoXMLConverter.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DynamoXMLConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileService _fileService;

        public HomeController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        public IActionResult Index()
        {         
            return View(InitializeModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] IEnumerable<IFormFile> files)
        {
            var fileProcessResponse = await _fileService.ProcessFiles(files);

            if (!fileProcessResponse.IsSucceed)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = fileProcessResponse.ErrorMessage;
                return RedirectToAction("Index");
            }

            var jsonData = JsonConvert.SerializeObject(fileProcessResponse.Files);
            TempData[Constants.File.RouteParams.ProcessedFiles] = jsonData;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { });
        }

        private HomePageDisplayModel InitializeModel() 
        {
            var model = new HomePageDisplayModel();
            model.ErrorMessage = TempData.TryGetValue<string>(Constants.File.RouteParams.ErrorMessage);
            model.UploadedFiles = TempData.TryGetValue<IEnumerable<FileDisplayModel>>(Constants.File.RouteParams.ProcessedFiles) ?? Array.Empty<FileDisplayModel>();

            // Always clear the temp data. By default when it is observed is automaticaly cleared but in the same session.
            // So if the session is not expired, the data will stay and the page may display same data
            TempData.Clear();
            return model;
        }
    }
}