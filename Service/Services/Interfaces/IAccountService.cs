using Service.DTOs.Account;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
	public interface IAccountService
	{
		Task CreateRoleAsync();
		List<string> GetAllRoles();
		Task<RegisterResponse> SignUpAsync(RegisterDto request);
		List<UserDto> GetAllUsers();
		Task<LoginResponse> SignInAsync(LoginDto request);
		Task<BaseResponse> AddRoleToUserAsync(UserRoleDto request);
	}
}
