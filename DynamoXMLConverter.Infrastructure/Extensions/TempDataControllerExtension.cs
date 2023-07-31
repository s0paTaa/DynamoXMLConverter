using DynamoXMLConverter.Domain;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace DynamoXMLConverter.Infrastructure.Extensions
{
    public static class TempDataControllerExtension
    {
        public static TResult TryGetValue<TResult>(this ITempDataDictionary tempData, string key) where TResult : class
        {
            TResult? result = null;

            switch (key)
            {
                case Constants.File.RouteParams.ErrorMessage:
                    result = tempData[key] as TResult;
                    break;
                case Constants.File.RouteParams.ProcessedFiles:
                    string json = tempData[key] as string ?? string.Empty;
                    result = JsonConvert.DeserializeObject<TResult>(json);
                    break;
                case Constants.File.RouteParams.JsonText:
                    result = tempData[key] as TResult;
                    break;
                case Constants.File.RouteParams.DeleteSuccessMessage:
                    result = tempData[key] as TResult;
                    break;
            }

            return result;
        }
    }
}
