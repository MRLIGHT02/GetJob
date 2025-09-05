using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.ServiceContracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GetJob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// Retrieves all jobs.
        /// </summary>
        /// <returns>A list of all jobs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobs();

            // Map the entities to DTOs for the public response
            var jobDtos = jobs.Select(job => new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company,
                Location = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployerId = job.EmployerId
            }).ToList();

            return Ok(jobDtos);
        }

        /// <summary>
        /// Retrieves a job by its ID.
        /// </summary>
        /// <param name="id">The job ID.</param>
        /// <returns>The job if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetJobById(id);
            if (job == null) return NotFound();

            // Map the entity to a DTO for the public response
            var jobDto = new JobResponseDto
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company,
                Location = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployerId = job.EmployerId
            };

            return Ok(jobDto);
        }

        /// <summary>
        /// Adds a new job.
        /// </summary>
        /// <param name="jobDto">The job data to add.</param>
        /// <returns>The created job.</returns>
        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobRequestDto jobDto)
        {
            // Map the DTO to the entity
            var newJob = new Job
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                Company = jobDto.Company,
                Location = jobDto.Location,
                Salary = jobDto.Salary,
                EmployerId = jobDto.EmployerId,
                PostedDate = DateTime.UtcNow // Set the server-side generated date
            };

            await _jobService.AddJob(newJob);

            // Map the entity to the DTO for the response
            var createdJobDto = new JobResponseDto
            {
                JobId = newJob.JobId,
                Title = newJob.Title,
                Description = newJob.Description,
                Company = newJob.Company,
                Location = newJob.Location,
                Salary = newJob.Salary,
                PostedDate = newJob.PostedDate,
                EmployerId = newJob.EmployerId
            };

            return CreatedAtAction(nameof(GetJobById), new { id = createdJobDto.JobId }, createdJobDto);
        }

        /// <summary>
        /// Updates an existing job.
        /// </summary>
        /// <param name="id">The job ID.</param>
        /// <param name="jobDto">The job with updated data.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobRequestDto jobDto)
        {
            // First, retrieve the existing job from the service
            var existingJob = await _jobService.GetJobById(id);
            if (existingJob == null)
            {
                return NotFound();
            }

            // Update the entity with the DTO data
            existingJob.Title = jobDto.Title;
            existingJob.Description = jobDto.Description;
            existingJob.Company = jobDto.Company;
            existingJob.Location = jobDto.Location;
            existingJob.Salary = jobDto.Salary;
            existingJob.EmployerId = jobDto.EmployerId;

            await _jobService.UpdateJob(existingJob);

            return NoContent();
        }

        /// <summary>
        /// Deletes a job by its ID.
        /// </summary>
        /// <param name="id">The job ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            await _jobService.DeleteJob(id);
            return NoContent();
        }
    }
}
