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

        public ApplicationController(IApplicationService applicationService, IMapper mapper) // 👉 Public constructor
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }

        [HttpPost("apply")]
        public async Task<ActionResult<ApplicationResponseDto>> ApplyApplication([FromBody] ApplicationCreateDto dto)
        {
            var application = _mapper.Map<Application>(dto);
            var created = await _applicationService.ApplyAsync(application);
            var response = _mapper.Map<ApplicationResponseDto>(created);
            return Ok(response);
        }

    }
}
