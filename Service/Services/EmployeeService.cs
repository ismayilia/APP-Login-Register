using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.DTOs.Employee;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeRepository _employeeRepo;
		private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
			_mapper = mapper;
        }

		public async Task CreateAsync(EmployeeCreateDto request)
		{
			await _employeeRepo.CreateAsync(_mapper.Map<Employee>(request));
		}

		public async Task<BaseResponse> DelteAsync(int? id)
		{
			try
			{
				ArgumentNullException.ThrowIfNull(id);
				var data = await _employeeRepo.GetAsync(id);
				await _employeeRepo.DeleteAsync(data);
				return new BaseResponse { IsSuccess = true, ErrorMessage=null };
			}
			catch (Exception)
			{

				return new BaseResponse { IsSuccess = false, ErrorMessage = "Data not found" };
			}
			
		}

		public async Task EditAsync(int id, EmployeeEdtiDto request)
		{
			var data = await _employeeRepo.GetAsync(id);
			await _employeeRepo.UpdateAsync(_mapper.Map(request,data));

		}

		public async Task<List<EmployeeDto>> GetAllAsync()
		{
			return _mapper.Map<List<EmployeeDto>>(await _employeeRepo.GetAllAsync());
		}

		public async Task<EmployeeDto> GetById(int? id)
		{

			ArgumentNullException.ThrowIfNull(id);
			return _mapper.Map<EmployeeDto>(await _employeeRepo.GetAsync(id));
		}

		public async Task<List<Employee>> SearchByFullName(string fullName)
		{
			return await _employeeRepo.GetAllByFullNameAsync(fullName);
		}

		public async Task<List<Employee>> Sort(string sort)
		{
			return await _employeeRepo.SortAsync(sort);
		}
	}
}
