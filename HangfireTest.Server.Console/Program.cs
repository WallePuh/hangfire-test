using System;
using System.IO;
using Hangfire;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using HangfireTest.Core;
using Ninject;
using Ninject.Modules;
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

                _host = new BackgroundJobServer();
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
            Bind<ICalculator>().To<LocalCalculator>();
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
