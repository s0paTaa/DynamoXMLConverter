﻿using DynamoXMLConverter.Domain;
using DynamoXMLConverter.Domain.Models.Shared;
using DynamoXMLConverter.Domain.Services;
using DynamoXMLConverter.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
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
                TempData[Constants.File.RouteParams.ErrorMessage] = "Invalid identifier";
                return RedirectToAction("Download");
            }

            var file = await _fileService.GetJsonFileByIdentifier(value);

            if (file == null)
            {
                TempData[Constants.File.RouteParams.ErrorMessage] = "File not found";
                return RedirectToAction("Download");
            }

            byte[] fileBytes = Encoding.ASCII.GetBytes(file.JsonText);

            return File(fileBytes, "application/download", file.FileName);
        }

        private BaseResponseModel InitializeModel()
        {
            var model = new BaseResponseModel();
            model.ErrorMessage = TempData.TryGetValue<string>(Constants.File.RouteParams.ErrorMessage);

            // Always clear the temp data. By default when it is observed is automaticaly cleared but in the same session.
            // So if the session is not expired, the data will stay and the page may display same data
            TempData.Clear();

            return model;
        }
    }
}