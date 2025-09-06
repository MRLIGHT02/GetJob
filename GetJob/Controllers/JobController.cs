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
        /// Get all jobs (visible to everyone)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        /// <summary>
        /// Get a job by Id
        /// </summary>
      
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            var GotJob = new JobResponseDto()
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company,
                Location = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployerId = job.EmployerId,
                
            };
            return job == null
                ? NotFound(new { Message = "Job not found" })
                : Ok(job);
        }

        /// <summary>
        /// Employer creates a new job
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<JobResponseDto>> CreateJob([FromBody] JobCreateDto jobDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdJob = await _jobService.AddJobAsync(jobDto);
            return CreatedAtAction(nameof(GetJobById), new { id = createdJob.JobId }, createdJob);
        }

        /// <summary>
        /// Employer updates an existing job
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<JobResponseDto>> UpdateJob(int id, [FromBody] JobUpdateDto jobDto)
        {
            if (id != jobDto.JobId) return BadRequest("JobId mismatch");

            try
            {
                var updatedJob = await _jobService.UpdateJobAsync(jobDto);
                return Ok(updatedJob);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Employer deletes a job
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                await _jobService.DeleteJobAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
