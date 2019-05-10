using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace SomeService
{
    public class InvoiceService
    {
        private readonly IJobFactory _jobFactory;
        //private const string FilePath = @"D:\Cheranga\Temp\TestData.txt";
        //private readonly ITodoApiClient _client;
        //private readonly Timer _timer;

        //public InvoiceService(ITodoApiClient client)
        //{
        //    _client = client;
        //    _timer = new Timer(2000){AutoReset = true};
        //    _timer.Elapsed -= OnElapsed;
        //    _timer.Elapsed += OnElapsed;
        //}

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

        //private async void OnElapsed(object sender, ElapsedEventArgs e)
        //{
        //    var todos = await _client.GetTodosAsync();
        //    var content = JsonConvert.SerializeObject(todos);

        //    using (var fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        //    {
        //        using (var writer = new StreamWriter(fileStream) { AutoFlush = true })
        //        {
        //            await writer.WriteLineAsync($"{DateTime.UtcNow:HH:mm:ss} :: {"data received!"}");
        //        }
        //    }
        //}

        public void OnStop()
        {
            //_timer.Stop();
            //File.Delete(FilePath);
        }
    }
}