using System.Collections.Generic;
using System.Threading.Tasks;
using JobsAppAndroid.Models;

namespace JobsAppAndroid.Data
{
    public interface IDatabase
    {
        Task<List<Job>> FindAllJobs();
    }
}