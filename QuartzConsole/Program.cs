﻿using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            var jobDetails = JobBuilder.Create<SomeJob>()
                .WithIdentity(JobKey.Create("some job", "some group"))
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("some trigger", "some group"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(2)
                        .RepeatForever();
                })
                .Build();


            var scheduler = await new StdSchedulerFactory().GetScheduler();
            await scheduler.ScheduleJob(jobDetails, trigger);

            await scheduler.Start();
        }
    }

    public class SomeJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Some job is doing it's job!");
        }
    }
}
