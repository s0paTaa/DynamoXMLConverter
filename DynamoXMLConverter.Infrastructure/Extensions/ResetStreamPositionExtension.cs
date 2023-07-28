namespace DynamoXMLConverter.Infrastructure.Extensions
{
    public static class ResetStreamPositionExtension
    {
        public static void ResetPosition(this Stream stream)
        {
            // Reseting the position is good practice before validating some stream or using it to create a file.
            stream.Position = 0;
        }
    }
}
