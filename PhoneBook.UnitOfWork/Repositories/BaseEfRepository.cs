using System.Linq;
using PhoneBook.Utilities;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories
{
    public class BaseEfRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext DbContext;
        protected DbSet<T> DbSet;

        protected BaseEfRepository(DbContext dbContext)
        {
            ExceptionHandling.CheckArgumentForNull(dbContext);

            DbContext = dbContext;

            DbSet = dbContext.Set<T>();
        }

        public virtual void AddRange(params T[] entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Create(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public void DeleteRange(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual bool ContainAnyElements()
        {
            return DbSet.Any();
        }

        public virtual T[] GetAll()
        {
            return DbSet.ToArray();
        }

        public virtual T? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
