using AutoMapper;
using GetJob.Entities;
using GetJob.ServiceContracts;
using GetJob.ServiceContracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GetJob.Controllers
{
    [ApiController]
    [Route("api/[conroller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;

        ApplicationController(IApplicationService applicationService,IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }

        [HttpPost("apply")]
        public async Task<Application> Apply([FromBody] ApplicationCreateDto dto) 
        {
            var application = _mapper.Map<Application>(dto);
            var created = await _applicationService.ApplyAsync(application);
            var response = _mapper.Map<ApplicationResponseDto>(created);

            return Ok(response);
        }


    }
}
