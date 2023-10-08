using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces
{
    public interface IGenericRepository<T> : IRepository where T : class
    {
        void Create(T entity);

        void AddRange(params T[] entities);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(params T[] entities);

        bool ContainAnyElements();

        T[] GetAll();

        T? GetById(int id);
    }
}
