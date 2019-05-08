using Quartz;
using Quartz.Impl;

namespace ProcessInvoiceService
{
    public class InvoiceService
    {
        private readonly IInvoiceProcessor _invoiceProcessor;

        public InvoiceService(IInvoiceProcessor invoiceProcessor)
        {
            _invoiceProcessor = invoiceProcessor;
        }

        public void OnStart()
        {
            var jobDetails = JobBuilder.Create<InMemoryInvoiceProcessor>()
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

            var scheduler = new StdSchedulerFactory().GetScheduler().Result;
            var offset = scheduler.ScheduleJob(jobDetails, trigger).Result;

            scheduler.Start().Wait();

        }

        public void OnStop()
        {

        }
    }


}