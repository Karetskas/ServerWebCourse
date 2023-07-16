using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class BaseEfRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext DbContext;
        protected DbSet<T> DbSet;

        protected BaseEfRepository(DbContext dbContext)
        {
            DbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" is null.")
                : dbContext;

            DbSet = dbContext.Set<T>();
        }

        public virtual void Create(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public virtual T[] GetAll()
        {
            return DbSet.ToArray();
        }

        public virtual T GetById(int id)
        {
            var dbSetItemsCount = DbSet.ToArray().Length;

            if (id >= dbSetItemsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    $"The argument \"{nameof(id)}\" = {id} must be less than {dbSetItemsCount}.");
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    $"The argument \"{nameof(id)}\" = {id} must be greater than 0.");
            }

            return DbSet.Find(id)!;
        }

        public virtual void AddRange(params T[] entities)
        {
            DbSet.AddRange(entities);
        }
    }
}
