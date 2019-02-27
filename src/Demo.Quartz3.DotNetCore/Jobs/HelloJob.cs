using Demo.Quartz3.DotNetCore.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Quartz3.Jobs
{
    public class HelloJob : IJob
    {
        private static readonly ILog log = LogProvider.GetLogger(typeof(SimpleRecoveryJob));
        private static readonly Random Random = new Random();

        public Task Execute(IJobExecutionContext context)
        {
            log.InfoFormat("HelloJob: Hello Quartznet at {0}", DateTime.Now.ToString("r"));
            var jobDetailJobDataMap = context.MergedJobDataMap;

            foreach (var key in jobDetailJobDataMap.Keys)
            {
                var jobDataMapItemValue = jobDetailJobDataMap[key];

                log.InfoFormat(key + ": " + jobDataMapItemValue + " (" + jobDataMapItemValue.GetType() + ")");
            }

            return Task.Delay(TimeSpan.FromSeconds(Random.Next(10, 20)));
        }
    }
}
