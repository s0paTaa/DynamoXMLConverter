using DynamoXMLConverter.Domain.Repositories.Base;
using DynamoXMLConverter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DynamoXMLConverter.Infrastructure.Repositories.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        protected BaseRepository(XmlConverterDbContext dbContext) 
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = dbContext.Set<TEntity>();
        }

        protected XmlConverterDbContext DbContext { get; }

        protected IQueryable<TEntity> DbSet { get; set; }

        public async Task Reload(TEntity entity)
        {
            await DbContext.Entry(entity).ReloadAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public IQueryable<TEntity> GetAllAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public async Task Add(TEntity entity, bool saveChanges = true)
        {
            SetEntityStateWithoutChangeDetection(entity, EntityState.Added);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        public async Task AddRange(IEnumerable<TEntity> collection, bool saveChanges = true)
        {
            SetEntityStateInCollectionWithoutChangeDetection(collection, EntityState.Added);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        public async Task Delete(TEntity entity, bool saveChanges = true)
        {
            SetEntityStateWithoutChangeDetection(entity, EntityState.Deleted);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        public async Task DeleteRange(IEnumerable<TEntity> collection, bool saveChanges = true)
        {
            SetEntityStateInCollectionWithoutChangeDetection(collection, EntityState.Deleted);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        public async Task SaveChanges()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity, bool saveChanges = true)
        {
            SetEntityStateWithoutChangeDetection(entity, EntityState.Modified);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        public async Task UpdateRange(IEnumerable<TEntity> collection, bool saveChanges = true)
        {
            SetEntityStateInCollectionWithoutChangeDetection(collection, EntityState.Modified);

            if (saveChanges)
            {
                await SaveChanges();
            }
        }

        private void SetEntityStateInCollectionWithoutChangeDetection(IEnumerable<TEntity> collection, EntityState state)
        {
            try
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (var entity in collection.ToList())
                {
                    DbContext.SetEntityState(entity, state);
                }
            }
            finally
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        private void SetEntityStateWithoutChangeDetection(TEntity entity, EntityState state)
        {
            try
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                DbContext.SetEntityState(entity, state);
            }
            finally
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
