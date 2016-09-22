using Topshelf;
using Topshelf.ServiceConfigurators;

namespace HangfireTest.Worker.Console
{
    static class Program
    {
        static int Main(string[] args)
        {
            var exitCode = HostFactory.Run(c =>
            {
                c.Service<MyService>(service =>
                {
                    ServiceConfigurator<MyService> s = service;
                    s.ConstructUsing(() => new MyService());
                    s.WhenStarted(a => a.Start());
                    s.WhenStopped(a => a.Stop());
                });
            });
            return (int)exitCode;
        }
    }
}