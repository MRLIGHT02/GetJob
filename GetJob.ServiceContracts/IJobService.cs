using GetJob.Entities;

namespace GetJob.ServiceContracts
{
    public interface IJobService
    {
       public Task<IEnumerable<Job>> GetAllJobs();
        public Task<Job> GetJobById(int id);
        public Task AddJob(Job job);
        public Task UpdateJob(Job job);
        public Task DeleteJob(int id);
    }
}
