using AutoMapper;
using GetJob.Entities;
using GetJob.ServiceContracts.DTOs;

namespace GetJob.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 🔹 DTO → Entity
            CreateMap<ApplicationCreateDto, Application>();

            // 🔹 Entity → Response DTO
            CreateMap<Application, ApplicationResponseDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : null))
                .ForMember(dest => dest.JobseekerName, opt => opt.MapFrom(src => src.Jobseeker != null ? src.Jobseeker.Name : null));
        }
    }
}
