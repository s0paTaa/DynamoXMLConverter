using DynamoXMLConverter.Domain.DependencyResolver;
using SimpleInjector;

namespace DynamoXMLConverter.Infrastructure.DependencyResolver
{
    public class DependencyInjectionResolver : IDependencyInjectionResolver
    {
        private readonly Container container;

        public DependencyInjectionResolver(Container container)
        {
            this.container = container;
        }

        public T GetService<T>() where T : class
        {
            return container.GetInstance<T>();
        }
    }
}
