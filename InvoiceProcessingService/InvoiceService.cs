using System;
using System.IO;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace InvoiceProcessingService
{
    public class AnotherInvoiceService
    {
        private readonly ITodoApiClient _client;
        private readonly ITodoRepository _repository;

        public AnotherInvoiceService(ITodoApiClient client, ITodoRepository repository)
        {
            _client = client;
            _repository = repository;
        }

        public void OnStart()
        {
            var tasks = _client.GetTodosAsync().Result;
            if (tasks == null || !tasks.Any())
            {
                return;
            }

            var status = _repository.UpsertTodoItemsAsync(tasks).Result;

            if (!status)
            {
                throw new Exception("Cannot insert/update tasks");
            }
        }

        public void OnStop()
        {
            
        }
    }

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

            var jobDetails = JobBuilder.Create<InvoiceProcessingJob>()
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
            File.Delete(InvoiceProcessingJob.FilePath);
        }
    }
}