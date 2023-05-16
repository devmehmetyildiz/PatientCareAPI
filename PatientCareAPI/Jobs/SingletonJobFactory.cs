using Microsoft.Extensions.DependencyInjection;
using PatientCareAPI.DataAccess;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Jobs
{
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _scopeFactory;

        public SingletonJobFactory(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory)
        {
            _serviceProvider = serviceProvider;
            _scopeFactory = scopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();
            try
            {
                var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                if (job == null)
                {
                    throw new InvalidOperationException($"Cannot instantiate job {bundle.JobDetail.JobType}");
                }
                return job;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
