using GetJob.Entities;
using GetJob.ServiceContracts.DTOs;

namespace GetJob.ServiceContracts
{
    public interface IJobService
    {
        Task<JobResponseDto> AddJobAsync(JobCreateDto jobDto);
        Task<JobResponseDto> UpdateJobAsync(JobUpdateDto jobDto);
        Task DeleteJobAsync(int id);
        Task<Job?> GetJobByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllJobsAsync();
    }
}
