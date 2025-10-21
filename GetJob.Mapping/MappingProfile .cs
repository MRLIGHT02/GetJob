using AutoMapper;
using GetJob.Entities;
using GetJob.ServiceContracts.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationCreateDto, Application>();
        CreateMap<Application, ApplicationResponseDto>();
    }
}
