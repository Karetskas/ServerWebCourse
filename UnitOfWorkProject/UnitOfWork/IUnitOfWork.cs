using System;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();

        T? GetRepository<T>() where T : class, IRepository;

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
