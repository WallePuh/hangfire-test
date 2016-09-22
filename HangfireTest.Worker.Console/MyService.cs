using System;
using System.Configuration;
using Microsoft.Owin.Hosting;

namespace HangfireTest.Worker.Console
{
    public class MyService
    {
        private IDisposable app;
        public void Start()
        {
            var urlRoot = ConfigurationManager.AppSettings["urlRoot"] ?? "http://localhost:5000/";
            app = WebApp.Start(urlRoot);
        }
        public void Stop()
        {
            app.Dispose();
        }
    }
}