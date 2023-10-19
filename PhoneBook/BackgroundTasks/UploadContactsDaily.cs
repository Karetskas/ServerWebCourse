using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Academits.Karetskas.PhoneBook.Dto;
using Microsoft.Extensions.DependencyInjection;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.BusinessLogic.DataConversion;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BackgroundTasks
{
    public sealed class UploadContactsDaily : IHostedService
    {
        private Timer? _timer;
        private readonly IExcel _excel;
        private readonly object _path;
        private readonly IServiceProvider _serviceProvider;

        public UploadContactsDaily(string path, IExcel excel, IServiceProvider serviceProvider)
        {
            CheckArgument(path);
            CheckArgument(excel);

            _path = path;
            _excel = excel;
            _serviceProvider = serviceProvider;
        }

        private void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument {nameof(obj)} is null.");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            const int day = 24;

            _timer = new Timer(CreateExcelDocument, _path, TimeSpan.Zero, TimeSpan.FromHours(day));

            return Task.CompletedTask;
        }

        private void CreateExcelDocument(object? path)
        {
            var contacts = GetContacts();
            string fileName = $"{_path}PhoneBook_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";

            _excel.SaveDocument(fileName, contacts);
        }

        private List<ContactDto> GetContacts()
        {
            var serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
            var unitOfWork = new UnitOfWorkPhoneBook(serviceProvider.GetRequiredService<PhoneBookDbContext>(), serviceProvider);

            var filterText = "";
            var contactsCount = unitOfWork.GetRepository<IContactRepository>()!.GetContactsCount(filterText);

            return unitOfWork.GetRepository<IContactRepository>()!.GetContacts(filterText, 1, contactsCount);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();

            return Task.CompletedTask;
        }
    }
}
