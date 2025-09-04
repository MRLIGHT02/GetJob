using GetJob.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.ServiceContracts
{
    public interface IApplicationService
    {
        Task<Application> ApplyAsync(Application application);
        Task<Application?> GetByIdAsync(int id);
        Task<IEnumerable<Application>> GetAllAsync();
        Task<IEnumerable<Application>> GetByCandidateIdAsync(int candidateId);
        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);

    }
}
