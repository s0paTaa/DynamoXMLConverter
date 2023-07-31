namespace DynamoXMLConverter.Domain
{
    public static class Constants
    {
        public static class File
        {
            public const long MultipartBodyLengthInBytes = 268435456; // 256 MB
            public const long MultipartBodyLengthForFileInBytes = 4194304; // 4 MB
            public const int FileLifetimeInDays = 10;

            public static class RouteParams
            {
                public const string ErrorMessage = "error-message";
                public const string ProcessedFiles = "processed-files";
                public const string JsonText = "json-text";
                public const string DeleteSuccessMessage = "delete-success-message";
            }

            public static class ErrorMessages
            {
                public const string InvalidIdentifier = "Invalid identifier";
                public const string FileNotFound = "File not found";
                public const string FileAlreadyExist = "File already exist";
            }

            public static class SuccessMessages
            {
                public const string DeleteSuccessMessage = "File is deleted successfully";
            }
        }

        public static class ClamAV
        {
            public static class ErrorMessages
            {
                public const string ClamServerNotFound = "ClamAv server is not responding";
            }
        }

        public static class Hangfire
        {
            public const byte DisableConcurrentExecutionTimeoutInSeconds = 60;

            public static class Jobs
            {
                public const string RemoveExpiredFilesJobName = "Remove Expired Files Job";
            }
        }
    }
}
