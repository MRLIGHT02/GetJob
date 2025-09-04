using Azure.Core;
using GetJob.Data;
using GetJob.Entities;
using GetJob.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace GetJob.Services
{
    public class JobService: IJobService
    {
        private readonly JobPortalContext _jobPortalContext;

        public JobService(JobPortalContext jobPortalContext)
        {
            _jobPortalContext = jobPortalContext;
        }

        public async Task AddJob(Job job) 
        {
            _jobPortalContext.Jobs.Add(job);
            await _jobPortalContext.SaveChangesAsync();

        }

        public async Task DeleteJob(int id)
        {
            var job = _jobPortalContext.Jobs.Find(id);
            if (job != null) {
            
            _jobPortalContext.Jobs.Remove(job);
                await _jobPortalContext.SaveChangesAsync();
            }

            else
            {
                throw new Exception("Id Not Found");
            }

        }

        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return await _jobPortalContext.Jobs.ToListAsync();
        }

        public async Task<Job> GetJobById(int id)
        {

           var FoundJob = await _jobPortalContext.Jobs.FindAsync(id);
            if (FoundJob !=null)
            {
                return FoundJob;
            }
            else
            {
                throw new Exception("Job Not Found");
            }

        }

        public async Task UpdateJob(Job job)
        {
           
            _jobPortalContext.Entry(job).State = EntityState.Modified;
            await _jobPortalContext.SaveChangesAsync();
        }
    }
}
