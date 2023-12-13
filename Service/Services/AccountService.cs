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
		private readonly JWTSettings _jwt;

        public AccountService(RoleManager<IdentityRole> roleManager, 
                                    UserManager<AppUser> userManager,
                                    IMapper mapper,
									IOptions<JWTSettings> options)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
			_jwt = options.Value;
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

			string token = GenerateJwtToken(existUser, (List<string>)userRoles);

			return new LoginResponse { IsSuccess = true, Errors = null, Token = token };
		}

		private string GenerateJwtToken(AppUser user, List<string> roles)
		{
			var claims = new List<Claim>
		{
				// adi sub olsun type-i username olsun
			new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.NameIdentifier, user.UserName),
			new Claim(ClaimTypes.Email, user.Email)
		}; // claim-in nameidentifer propertisi username olsun

			// bashga neyise elave etmek istesek onuda foreache saliriq 
			roles.ForEach(role =>
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			});

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwt.ExpireDays));

			var token = new JwtSecurityToken(
				_jwt.Issuer,
				_jwt.Issuer,
				claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
