using AutoMapper;
using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.ServiceContracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GetJob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationService applicationService, IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }

        // ✅ POST: api/Application/apply
        [HttpPost("apply")]
        public async Task<ActionResult<ApplicationResponseDto>> ApplyApplication([FromBody] ApplicationCreateDto dto)
        {
            try
            {
                var application = _mapper.Map<Application>(dto);
                var created = await _applicationService.ApplyAsync(application);
                var response = _mapper.Map<ApplicationResponseDto>(created);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ GET: api/Application
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetAllApplications()
        {
            var applications = await _applicationService.GetAllAsync();
            var response = _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
            return Ok(response);
        }

        // ✅ GET: api/Application/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationResponseDto>> GetApplicationById(int id)
        {
            try
            {
                var application = await _applicationService.GetByIdAsync(id);
                var response = _mapper.Map<ApplicationResponseDto>(application);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ✅ GET: api/Application/byCandidate/{jobseekerId}
        [HttpGet("byCandidate/{jobseekerId}")]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetApplicationsByCandidate(int jobseekerId)
        {
            try
            {
                var applications = await _applicationService.GetByCandidateIdAsync(jobseekerId);
                var response = _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ✅ GET: api/Application/byJob/{jobId}
        [HttpGet("byJob/{jobId}")]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetApplicationsByJob(int jobId)
        {
            var applications = await _applicationService.GetByJobIdAsync(jobId);
            var response = _mapper.Map<IEnumerable<ApplicationResponseDto>>(applications);
            return Ok(response);
        }

        // ✅ PUT: api/Application/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] string status)
        {
            var success = await _applicationService.UpdateStatusAsync(id, status);
            if (!success)
                return NotFound(new { message = "Application not found." });

            return Ok(new { message = "Application status updated successfully." });
        }

        // ✅ DELETE: api/Application/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var deleted = await _applicationService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Application not found or could not be deleted." });

            return Ok(new { message = "Application deleted successfully." });
        }
    }
}
