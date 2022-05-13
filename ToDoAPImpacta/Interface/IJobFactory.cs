using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Interface
{
    public interface IJobFactory
    {
        Job buildJob(string jobName, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context, string user);
        List<Job> MountJobList(string userId, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context);

        Job FinalizeJob(string jobID, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context);
    }
}
