using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Server;
using HangfireTest.Core.HangfireExtensions;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Mvc;
using Owin;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(HangfireTest.Web.Startup))]
namespace HangfireTest.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("hangfire_db")
                .UseDashboardMetric(DashboardMetrics.ServerCount)
                .UseFilter(new NoDuplicatesAttribute());

            app.UseHangfireDashboard();
        }
    }
    
}
