using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
namespace Repository.Repositories
{
	public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
	{

		public EmployeeRepository(AppDbContext context) : base(context) { }

		public async Task<List<Employee>> GetAllByFullNameAsync(string fullName)
		{
			return fullName != null ? await _context.Employees.Where(m => m.FullName.Trim().ToLower().Contains(fullName.Trim().ToLower()))
				.ToListAsync() : await _context.Employees.ToListAsync();
		}

		public async Task<List<Employee>> SortAsync(string sort)
		{
			if (sort != null && sort.Trim().ToLower() == "asc")
			{
				return await _context.Employees.OrderBy(m => m.Age).ToListAsync();
			}
			else if (sort != null && sort.Trim().ToLower() == "desc")
			{
				return await _context.Employees.OrderByDescending(m => m.Age).ToListAsync();
			}
			else
			{
				return await _context.Employees.ToListAsync();
			}
		}
	}
}
