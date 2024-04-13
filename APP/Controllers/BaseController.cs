﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.Controllers
{
	[Authorize(AuthenticationSchemes ="Bearer")]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
	}
}
