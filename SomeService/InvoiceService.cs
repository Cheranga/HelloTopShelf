using System.IO;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace SomeService
{
    public class InvoiceService
    {
        private readonly IJobFactory _jobFactory;

        public InvoiceService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public void OnStart()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = _jobFactory;
            scheduler.Start().Wait();

            var jobDetails = JobBuilder.Create<SomeJob>()
                .WithIdentity(JobKey.Create("Invoice processing", "Document processing"))
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("Create or update invoices", "Document processing"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(2)
                        .RepeatForever();
                })
                .Build();

            scheduler.ScheduleJob(jobDetails, trigger).Wait();
        }

        public void OnStop()
        {
            File.Delete(SomeJob.FilePath);
        }
    }
}