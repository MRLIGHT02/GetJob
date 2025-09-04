using GetJob.Entities;
using GetJob.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.Services
{
    public class ApplicationService : IApplicationService
    {
        public Task<Application> ApplyAsync(Application application)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application>> GetByCandidateIdAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task<Application?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStatusAsync(int id, string status)
        {
            throw new NotImplementedException();
        }
    }
}
