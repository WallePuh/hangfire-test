using System.Web.Http;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace HangfireTest.Worker.Console
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            config.EnableSystemDiagnosticsTracing();
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            //kernel.Load(Assembly.GetExecutingAssembly());
            DependencyInjection(kernel);
            return kernel;
        }

        private static void DependencyInjection(IKernel kernel)
        {
            //kernel.Bind<ISampleDependency>().To<SampleDependency1>();
        }
    }
}