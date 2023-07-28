namespace DynamoXMLConverter.Domain.DependencyResolver
{
    public interface IDependencyInjectionResolver
    {
        T GetService<T>() where T : class;
    }
}
