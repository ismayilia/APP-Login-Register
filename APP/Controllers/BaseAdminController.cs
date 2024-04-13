using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APP.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer",Roles ="Admin")]
	[Route("api/admin/[controller]/[action]")]
	[ApiController]
	public class BaseAdminController : ControllerBase
	{
	}
}
