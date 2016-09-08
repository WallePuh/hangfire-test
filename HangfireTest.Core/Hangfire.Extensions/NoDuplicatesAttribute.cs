using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage.Monitoring;
using RestSharp.Extensions;

namespace HangfireTest.Core.HangfireExtensions
{
    public static class JobListExtensions
    {
        public static bool HasDuplicate(this JobList<EnqueuedJobDto> jobList, Job job)
        {
            var newJobHash = job.GetDuplicationHashCode();

            foreach (var kvp in jobList)
            {
                var oldHash = kvp.Value.Job.GetDuplicationHashCode();
                if (newJobHash == oldHash)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetDuplicationHashCode(this Job job)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + job.Method.GetHashCode();
                foreach (var arg in job.Args)
                {
                    hash = hash * 31 + arg.GetHashCode();
                }
                return hash;
            }
        }
    }

    public class NoDuplicatesAttribute : JobFilterAttribute,
        IClientFilter
    {
        public void OnCreating(CreatingContext filterContext)
        {
            var queue = filterContext.Storage.GetMonitoringApi().EnqueuedJobs("default", 0, 100);
            if (queue.HasDuplicate(filterContext.Job))
            {
                filterContext.Canceled = true;
            }
        }

        public void OnCreated(CreatedContext filterContext)
        {
            // Method intentionally left empty.
        }
    }
}
