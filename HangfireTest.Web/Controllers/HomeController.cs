using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using HangfireTest.Core;

namespace HangfireTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBackgroundJobClient _jobClient;

        public HomeController()
            :this(new BackgroundJobClient())
        {
        }

        public HomeController(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddDummyJob()
        {
            var id = _jobClient.Enqueue<ICalculator>(x => x.Add(1.4, 1.7));
            Debug.WriteLine($"Enqueued job #{id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddCalcJob(double arg1 = 0, double arg2 = 0)
        {
            _jobClient.Enqueue<ICalculator>(x => x.Add(arg1, arg2));

            return RedirectToAction("Index");
        }
    }
}