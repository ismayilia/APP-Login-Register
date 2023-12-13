using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Account;
using Service.Services.Interfaces;

namespace APP.Controllers
{
	
	public class AccountController : BaseController
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

		[HttpPost]
		public async Task<IActionResult> SignUp([FromBody] RegisterDto request)
		{
			if(!ModelState.IsValid ) return BadRequest(ModelState);

			return Ok(await _accountService.SignUpAsync(request));
		}


		//body-den request geldiyine gore
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
	}
}
