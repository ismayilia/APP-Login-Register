using AutoMapper;
using Domain.Models;
using Service.DTOs.Account;
using Service.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mappings
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();
			CreateMap<EmployeeCreateDto, Employee>();
			CreateMap<EmployeeEdtiDto, Employee>();
			CreateMap<RegisterDto, AppUser>();
			CreateMap<AppUser, UserDto>();
		}
    }
}
