using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Exceptions;
using Service.DTOs.Account;
using Service.Helpers;
using Service.Helpers.Enums;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
		private readonly ITokkenService _tokkenService;

        public AccountService(RoleManager<IdentityRole> roleManager, 
                                    UserManager<AppUser> userManager,
                                    IMapper mapper,
									ITokkenService tokkenService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
			_tokkenService = tokkenService;
        }
        public async Task CreateRoleAsync()
		{
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name=role.ToString() });
                }
            }
        }

		public  List<string> GetAllRoles()
		{
			return  _roleManager.Roles.Select(m=>m.Name).ToList();
		}

		public List<UserDto> GetAllUsers()
		{
            return _mapper.Map<List<UserDto>>(_userManager.Users);
		}

		

		public async Task<RegisterResponse> SignUpAsync(RegisterDto request)
		{
            ArgumentNullException.ThrowIfNull(nameof(request));

            AppUser user = _mapper.Map<AppUser>(request);


			IdentityResult response = await _userManager.CreateAsync(user, request.Password);

            if (!response.Succeeded)
            {
                return new RegisterResponse { IsSuccess = false, Errors = response.Errors.Select(m => m.Description).ToList() };
            }

            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

            return new RegisterResponse { IsSuccess=true, Errors = null};
		}

		public async Task<LoginResponse> SignInAsync(LoginDto request)
		{
			ArgumentNullException.ThrowIfNull(nameof(request));

			AppUser existUser = await _userManager.FindByEmailAsync(request.Email);

			if(existUser is null) return new LoginResponse { IsSuccess = false, Token = null, Errors = new List<string> { "Email or password wrong" } };

			if(!await _userManager.CheckPasswordAsync(existUser, request.Password))
			{
				return new LoginResponse { IsSuccess = false, Token = null, Errors = new List<string> { "Email or password wrong" } };
			}

			var userRoles = await _userManager.GetRolesAsync(existUser);

			string token = _tokkenService.GenerateJwtToken(existUser, (List<string>)userRoles);

			return new LoginResponse { IsSuccess = true, Errors = null, Token = token };
		}

		public async Task<BaseResponse> AddRoleToUserAsync(UserRoleDto request)
		{
			AppUser user = await _userManager.FindByIdAsync(request.UserId);
			if (user is null) throw new NotFoundException("User not found");

			IdentityRole role = await _roleManager.FindByIdAsync(request.RoleId);
			if (role is null) throw new NotFoundException("Role not found");

			IList<string> userRoles = await _userManager.GetRolesAsync(user);

			if (userRoles.Any(m=> m == role.Name))
			{
				return new BaseResponse { IsSuccess = false, ErrorMessage = $"{role.Name} already exist" };
			}

			await _userManager.AddToRoleAsync(user, role.Name);

			return new BaseResponse { IsSuccess = true, ErrorMessage = null};
		}
	}
}
