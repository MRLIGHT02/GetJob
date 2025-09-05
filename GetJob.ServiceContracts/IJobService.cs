using GetJob.Entities;
using GetJob.ServiceContracts.DTOs;

namespace GetJob.ServiceContracts
{
    public interface IJobService
    {
        Task<JobResponseDto> AddJobAsync(JobCreateDto jobDto);
        Task<JobResponseDto> UpdateJobAsync(JobUpdateDto jobDto);
        Task DeleteJobAsync(int id);
        Task<JobResponseDto?> GetJobByIdAsync(int id);
        Task<IEnumerable<JobResponseDto>> GetAllJobsAsync();
    }
}
