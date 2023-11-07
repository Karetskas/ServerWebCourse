using System;
using System.Threading;
using PhoneBook.Utilities;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Academits.Karetskas.PhoneBook.BusinessLogic.Handlers;

namespace Academits.Karetskas.PhoneBook.BackgroundTasks
{
    public sealed class UploadContactsDailyHostedService : IHostedService
    {
        private Timer? _timer;
        private readonly object _path;
        private readonly CreateExcelDocumentWithAllContactsHandler _createExcelDocument;

        public UploadContactsDailyHostedService(string path, CreateExcelDocumentWithAllContactsHandler createExcelDocument)
        {
            ExceptionHandling.CheckArgumentForNull(path);
            ExceptionHandling.CheckArgumentForNull(createExcelDocument);

            _path = path;
            _createExcelDocument = createExcelDocument;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            const int twentyFourHours = 24;

            _timer = new Timer(_createExcelDocument.Handle, _path, TimeSpan.Zero, TimeSpan.FromHours(twentyFourHours));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();

            return Task.CompletedTask;
        }
    }
}
