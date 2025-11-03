using GetJob.Entities;
using GetJob.ServiceContracts.DTOs;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetJob.AutoMapper
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            CreateMap<ApplicationCreateDto, Application>();
            CreateMap<Application, ApplicationResponseDto>();
        }
    }
}
