using System;
using Quartz;
using Quartz.Impl;

namespace Academits.Karetskas.WindowsService
{
    public sealed class QuartzNetTask : IQuartzNetTask
    {
        private readonly StdSchedulerFactory _factory;
        private IScheduler? _scheduler;

        public QuartzNetTask(StdSchedulerFactory stdSchedulerFactory)
        {
            _factory = stdSchedulerFactory ?? throw new ArgumentNullException(nameof(stdSchedulerFactory), $"The argument \"{nameof(stdSchedulerFactory)}\" is null.");
        }

        public async void Start()
        {
            _scheduler = await _factory.GetScheduler();

            await _scheduler.Start();

            IJobDetail job = JobBuilder.Create<WorkWithTextFile>()
                .WithIdentity("MyJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MyTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public void Stop()
        {
            _scheduler?.Shutdown();
        }
    }
}
