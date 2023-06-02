using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ForMember("FullAddress", 
                c => c.MapFrom(x => x.Address + "" + x.Country));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyCreationDto, Company>();

            CreateMap<EmployeeCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<CompanyForUpdateDto, Company>();

        }
    }
}
