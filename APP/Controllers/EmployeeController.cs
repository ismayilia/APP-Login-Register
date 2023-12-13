using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Employee;
using Service.Services.Interfaces;

namespace APP.Controllers
{
	
	public class EmployeeController : BaseController
	{
		private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _employeeService.GetAllAsync());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] EmployeeCreateDto request)
		{
			if(!ModelState.IsValid) return BadRequest(ModelState);
			await _employeeService.CreateAsync(request);
			return Ok();
		}

		[HttpDelete]
		
		public async Task<IActionResult> Delete([FromQuery] int? id)
		{
			return Ok(await _employeeService.DelteAsync(id));
		}


		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetById([FromRoute] int? id)
		{
			var data = await _employeeService.GetById(id);
			if (data is null)
			{
				return NotFound("Data not found");
			}
			return Ok(data);
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Edit([FromRoute] int? id, [FromBody] EmployeeEdtiDto request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id is null) return BadRequest();

			var data = await _employeeService.GetById(id);

			if (data == null) return NotFound();

			await _employeeService.EditAsync((int)id,request);
			return Ok();

		}

		[HttpGet]
		public async Task<IActionResult> Search([FromQuery] string search)
		{
			return Ok(await _employeeService.SearchByFullName(search));
		}

		[HttpGet]
		public async Task<IActionResult> Sort([FromQuery] string sort)
		{
			return Ok(await _employeeService.Sort(sort));
		}
	}
}
