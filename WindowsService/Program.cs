using Topshelf;
using Topshelf.ServiceConfigurators;
using Topshelf.HostConfigurators;
using System;
using Quartz.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Academits.Karetskas.WindowsService
{
    internal class Program
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        static void Main()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<StdSchedulerFactory>()
                .AddTransient<QuartzNetTask>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            HostFactory.Run(ConfigureHost);

            Console.ReadKey();
        }

        private static void ConfigureHost(HostConfigurator hostConfigurator)
        {
            hostConfigurator.Service<QuartzNetTask>(ConfigureService);
            hostConfigurator.RunAsLocalSystem();
            hostConfigurator.SetDescription("Windows service for saving information to a file.");
            hostConfigurator.SetDisplayName("FileSavingService");
            hostConfigurator.SetServiceName("FileSavingService");
        }

        private static void ConfigureService<T>(ServiceConfigurator<T> serviceConfigurator) where T : class, IQuartzNetTask
        {
            var scheduler = ServiceProvider?.GetService<T>();

            if (scheduler is null)
            {
                return;
            }

            serviceConfigurator.ConstructUsing(_ => scheduler);
            serviceConfigurator.WhenStarted(quartzNetTask => quartzNetTask.Start());
            serviceConfigurator.WhenStopped(quartzNetTask => quartzNetTask.Stop());
        }
    }
}