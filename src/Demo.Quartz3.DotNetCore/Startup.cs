using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CrystalQuartz.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Demo.Quartz3.DotNetCore
{
    using CrystalQuartz.Application;
    using CrystalQuartz.Core.Domain.ObjectInput;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));

            var scheduler = CreateScheduler();

            app.UseCrystalQuartz(
                () => scheduler,
                new CrystalQuartzOptions
                {
                    JobDataMapInputTypes = CrystalQuartzOptions
                        .CreateDefaultJobDataMapInputTypes()
                        .Concat(new[]
                        {
                            new RegisteredInputType(
                                new InputType("user", "User"),
                                null,
                                new FixedInputVariantsProvider(
                                    new InputVariant("john_smith", "John Smith"),
                                    new InputVariant("bob_doe", "Bob Doe"))),
                        })
                        .ToArray(),
                    AllowedJobTypes = RegisterJobTypes()
                });


            app.UseStaticFiles();
            app.UseMvc();
        }



        private IScheduler CreateScheduler()
        {

            NameValueCollection properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "TestScheduler",
                ["quartz.scheduler.instanceId"] = "instance_one",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "5",
                ["quartz.jobStore.misfireThreshold"] = "60000",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "false",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.jobStore.clustered"] = "true",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                ["quartz.dataSource.default.connectionString"] = TestConstants.SqlServerConnectionString,
                ["quartz.dataSource.default.provider"] = TestConstants.DefaultSqlServerProvider,
                ["quartz.serializer.type"] = "json"
            };

            var schedulerFactory = new StdSchedulerFactory(properties);

            var scheduler = schedulerFactory.GetScheduler().Result;

            return scheduler;
        }

        private Type[] RegisterJobTypes()
        {
            IList<Type> jobTypes = new List<Type>();
            jobTypes.Add(typeof(Jobs.HelloJob));
            jobTypes.Add(typeof(Jobs.SimpleRecoveryJob));
            jobTypes.Add(typeof(Jobs.SimpleRecoveryStatefulJob));
            return jobTypes.ToArray();
        }

    }
    
}
