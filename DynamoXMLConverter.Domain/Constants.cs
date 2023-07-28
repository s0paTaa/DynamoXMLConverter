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
