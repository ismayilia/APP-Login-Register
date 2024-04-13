using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Account;
using Service.Services.Interfaces;

namespace APP.Controllers
{
	
	public class AccountController : BaseAdminController
	{
		private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
		public async Task<IActionResult> CreateRole()
		{
			await _accountService.CreateRoleAsync();
			return Ok();
		}

		[HttpGet]
		public IActionResult GetAllRoles()
		{
			return Ok(_accountService.GetAllRoles());
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> SignUp([FromBody] RegisterDto request)
		{
			if(!ModelState.IsValid ) return BadRequest(ModelState);

			return Ok(await _accountService.SignUpAsync(request));
		}


		//body-den request geldiyine gore
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> SignIn([FromBody] LoginDto request)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			return Ok(await _accountService.SignInAsync(request));
		}

		[HttpGet]
		public IActionResult GetAllUsers()
		{
			return Ok(_accountService.GetAllUsers());
		}

		[HttpPost]
		public async Task<IActionResult> AddRoleToUSer(UserRoleDto request)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			return Ok(await _accountService.AddRoleToUserAsync(request));
		}
	}
}
