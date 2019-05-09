using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace ProcessInvoiceService
{
    public static class DependencyExtensions
    {
        public static void InitializeJobs(this IServiceCollection services, Assembly assembly)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //
            // Register the job factory
            //
            services.AddSingleton<IJobFactory>(provider =>
            {
                var jobFactory = new JobFactory(provider);
                return jobFactory;
            });
            //
            // Register the jobs
            //
            var jobTypes = assembly.GetTypes().Where(x => typeof(IJob).IsAssignableFrom(x)).ToList();
            jobTypes.ForEach(x=>services.AddSingleton(x));
        }
    }
}