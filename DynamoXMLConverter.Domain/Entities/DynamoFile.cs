using DynamoXMLConverter.Domain.Models.File;

namespace DynamoXMLConverter.Domain.Entities
{
    public class DynamoFile
    {
        public DynamoFile()
        {          
        }

        public DynamoFile(DynamoFileModel model, DateTime dateExpire)
        {
            Name = model.FileName; 
            Value = model.Text;
            Extension = model.Extension;
            ContentType = model.ContentType;
            DateExpire = dateExpire;
            DateCreated = DateTime.UtcNow;
        }

        public Guid ID { get; }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public string Extension { get; private set; }

        public string ContentType { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime DateExpire { get; private set; }
    }
}
