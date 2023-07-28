using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoXMLConverter.Domain.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task Reload(T entity);

        IQueryable<T> GetAll();

        IQueryable<T> GetAllAsNoTracking();

        Task Update(T entity, bool saveChanges = true);

        Task UpdateRange(IEnumerable<T> collection, bool saveChanges = true);

        Task Delete(T entity, bool saveChanges = true);

        Task DeleteRange(IEnumerable<T> collection, bool saveChanges = true);

        Task Add(T entity, bool saveChanges = true);

        Task AddRange(IEnumerable<T> collection, bool saveChanges = true);

        Task SaveChanges();
    }
}
