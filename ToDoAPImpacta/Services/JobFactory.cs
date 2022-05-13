using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPImpacta.Interface;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Services
{
    public class JobFactory : IJobFactory
    {
        public Job buildJob(string jobName, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context, string user)
        {
            bool validJobName = verifyCorrectJobName(jobName);

            if(validJobName == true)
            {
                Job job = new Job()
                {
                    Name = jobName,
                    CreationDate = DateTime.Now,
                    InOperation = true,
                    User = user
                };
                context.Jobs.Add(job);
                context.SaveChanges();
                return job;
            }

            return null;
        }

        public Job FinalizeJob(string jobID, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context)
        {
            Job jobToFinalize = context.Jobs.Where(r => r.Id == jobID).FirstOrDefault();

            if(jobToFinalize != null)
            {
                jobToFinalize.InOperation = false;
                jobToFinalize.FinallyDate = DateTime.Now;

                context.Update(jobToFinalize);
                context.SaveChanges();
            }

            return jobToFinalize;
        }

        public List<Job> MountJobList(string userId, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context)
        {
            return context.Jobs.Where(r => r.User == userId).ToList();
        }



        private bool verifyCorrectJobName(string jobName)
        {
            if(jobName.Length >3 && jobName.Length <= 200)
            {
                return true;
            }
            return false;
        }
    }
}
