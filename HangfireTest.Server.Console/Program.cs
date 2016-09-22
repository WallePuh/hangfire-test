using System;
using System.Configuration;
using System.IO;
using Hangfire;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using HangfireTest.Core;
using Ninject;
using Ninject.Modules;
using RestSharp;
using Topshelf;

namespace HangfireTest.Server.Console
{
    class Program
    {
        private const string Endpoint = "http://localhost:9010";
        static void Main()
        {
            LogProvider.SetCurrentLogProvider(new ColouredConsoleLogProvider());

            HostFactory.Run(x =>
            {
                x.Service<Application>(s =>
                {
                    s.ConstructUsing(name => new Application());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Hangfire Windows Service Sample");
                x.SetDisplayName("Hangfire Windows Service Sample");
                x.SetServiceName("hangfire-sample");
            });
        }

        private class Application
        {
            private IDisposable _host;

            public void Start()
            {
                GlobalConfiguration.Configuration.UseSqlServerStorage("hangfire_db");
                GlobalConfiguration.Configuration.UseActivator(
                    new NinjectJobActivator(
                        new StandardKernel(
                            new HangfireModule())));

                _host = new BackgroundJobServer(new BackgroundJobServerOptions
                {
                    Queues = (ConfigurationManager.AppSettings["Hangfire.Queues"] ?? "default").Split(','),
                    WorkerCount = int.Parse((ConfigurationManager.AppSettings["Hangfire.WorkerCount"] ?? "1"))
                });
            }

            public void Stop()
            {
                _host.Dispose();
            }
        }
    }

    public class HangfireModule: NinjectModule
    {
        public override void Load()
        {
            var restBaseUrl = ConfigurationManager.AppSettings["RestBaseUrl"] ?? "http://localhost:17688/api/calc";
            Bind<ICalculator>().ToConstructor(c => new RestCalculator(new RestClient(restBaseUrl)));
        }
    }

    public class NinjectJobActivator: JobActivator
    {
        private readonly IKernel _kernel;

        public NinjectJobActivator(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override object ActivateJob(Type jobType)
        {
            return _kernel.Get(jobType);
        }
    }
}
