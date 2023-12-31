﻿namespace DynamoXMLConverter.Domain.Models.Shared
{
    public class BaseResponseModel
    {
        public bool IsSucceed { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
    }
}
