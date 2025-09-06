using GetJob.Data;
using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.ServiceContracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GetJob.Services
{
    public class JobService : IJobService
    {
        private readonly JobPortalContext _context;

        public JobService(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<JobResponseDto> AddJobAsync(JobCreateDto jobDto)
        {
            // Pehle employer fetch kar
            var employer = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == jobDto.EmployerId && u.Role == UserRole.Employer && u.EmployerId > 0);

            if (employer == null)
            {
                throw new Exception("Invalid employer.");
            }

            var job = new Job
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                Company = jobDto.Company,
                Location = jobDto.Location,
                Salary = jobDto.Salary,
                EmployerId = employer.EmployerId!.Value, 
                PostedDate = DateTime.UtcNow
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company,
                Location = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployerId = job.EmployerId,
                EmployerName = employer.Name   
            };
        }


        public async Task<JobResponseDto> UpdateJobAsync(JobUpdateDto jobDto)
        {
            var job = await _context.Jobs.FindAsync(jobDto.JobId);
            if (job == null) throw new Exception("Job not found");

            job.Title = jobDto.Title;
            job.Description = jobDto.Description;
            job.Company = jobDto.Company;
            job.Location = jobDto.Location;
            job.Salary = jobDto.Salary;

            await _context.SaveChangesAsync();

            return new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company,
                Location = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployerId = job.EmployerId,
                EmployerName = job.Employer?.Name ?? ""
            };
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) throw new Exception("Job not found");

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {

            return await _context.Jobs.FindAsync(id);
           
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.ToListAsync();
        }
    }
}
