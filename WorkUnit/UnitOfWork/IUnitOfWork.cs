using System;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.UnitOfWork
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
