using Quartz;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Academits.Karetskas.WindowsService
{
    public sealed class WorkWithTextFile : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var streamWriter = new StreamWriter(@"D:\\FileService.txt", true);

            for (var i = 0; i < 5; i++)
            {
                streamWriter.WriteLine($"Hello world - {i}. Time: {DateTime.Now}");
            }

            streamWriter.Dispose();

            return Task.CompletedTask;
        }
    }
}
