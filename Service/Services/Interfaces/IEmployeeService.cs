using Domain.Models;
using Service.DTOs.Employee;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IEmployeeService
	{
		Task<List<EmployeeDto>> GetAllAsync();
		Task CreateAsync(EmployeeCreateDto request);
		Task<BaseResponse> DelteAsync(int? id);
		Task<EmployeeDto> GetById(int? id);
		Task EditAsync(int id,EmployeeEdtiDto request);
		Task<List<Employee>> SearchByFullName(string fullName);
		Task<List<Employee>> Sort(string sort);
	}
}
