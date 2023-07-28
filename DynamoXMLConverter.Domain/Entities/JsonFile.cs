namespace DynamoXMLConverter.Domain.Entities
{
    public class JsonFile
    {
        public JsonFile()
        {          
        }

        public JsonFile(string name, string value, DateTime dateExpire)
        {
            Name = name; 
            Value = value; 
            DateExpire = dateExpire;
            DateCreated = DateTime.UtcNow;
        }

        public Guid Identifier { get; }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime DateExpire { get; private set; }
    }
}
