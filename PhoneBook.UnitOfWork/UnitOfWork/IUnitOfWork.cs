using System;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork
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
