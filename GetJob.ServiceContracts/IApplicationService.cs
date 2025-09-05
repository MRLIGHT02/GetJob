using GetJob.Entities;

namespace GetJob.ServiceContracts
{
    public interface IApplicationService
    {
        Task<Application> ApplyAsync(Application application);
        Task<Application?> GetByIdAsync(int id);
        Task<IEnumerable<Application>> GetAllAsync();
        Task<IEnumerable<Application>> GetByCandidateIdAsync(int JobseekerId);
        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);

    }
}
