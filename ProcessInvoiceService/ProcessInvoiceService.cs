using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace ProcessInvoiceService
{
    public class ProcessInvoiceService
    {
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public ProcessInvoiceService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public void OnStart()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = _jobFactory;
            _scheduler.Start().Wait();

            //var voteJob = JobBuilder.Create<VoteJob>()
            //    .Build();
            var jobDetails = JobBuilder.Create<InMemoryInvoiceProcessor>()
                .WithIdentity(JobKey.Create("Invoice processing", "Document processing"))
                .Build();

            //var voteJobTrigger = TriggerBuilder.Create()
            //    .StartNow()
            //    .WithSimpleSchedule(s => s
            //        .WithIntervalInSeconds(60)
            //        .RepeatForever())
            //    .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("Create or update invoices", "Document processing"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(2)
                        .RepeatForever();
                })
                .Build();

            _scheduler.ScheduleJob(jobDetails, trigger).Wait();


            //var jobDetails = JobBuilder.Create<InMemoryInvoiceProcessor>()
            //    .WithIdentity(JobKey.Create("Invoice processing", "Document processing"))
            //    .Build();

            //var trigger = TriggerBuilder.Create()
            //    .WithIdentity(new TriggerKey("Create or update invoices", "Document processing"))
            //    .StartNow()
            //    .WithSimpleSchedule(builder =>
            //    {
            //        builder.WithIntervalInSeconds(2)
            //            .RepeatForever();
            //    })
            //    .Build();

            //var scheduler = new StdSchedulerFactory().GetScheduler().Result;
            //var offset = scheduler.ScheduleJob(jobDetails, trigger).Result;

            //scheduler.Start().Wait();
        }

        public void OnStop()
        {
        }
    }
}