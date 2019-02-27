﻿using System.Threading.Tasks;
using CrystalQuartz.Application;
using CrystalQuartz.Core;
using CrystalQuartz.Core.SchedulerProviders;
using CrystalQuartz.WebFramework;
using CrystalQuartz.WebFramework.HttpAbstractions;
using Microsoft.AspNetCore.Http;

namespace CrystalQuartz.AspNetCore
{
    public class CrystalQuartzPanelMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RunningApplication _runningApplication;

        public CrystalQuartzPanelMiddleware(
            RequestDelegate next,
            ISchedulerProvider schedulerProvider,
            Options options)
        {
            _next = next;

            var application = new CrystalQuartzPanelApplication(schedulerProvider, options);

            _runningApplication = application.Run();
        }

        public Task Invoke(HttpContext httpContext)
        {
            IRequest request = new AspNetCoreRequest(httpContext.Request.Query, httpContext.Request.HasFormContentType ? httpContext.Request.Form : null);
            IResponseRenderer responseRenderer = new AspNetCoreResponseRenderer(httpContext);

            _runningApplication.Handle(request, responseRenderer);
            return Task.CompletedTask;
        }
    }
}