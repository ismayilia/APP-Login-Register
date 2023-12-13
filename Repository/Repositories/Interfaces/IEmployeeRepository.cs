using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
	public interface IEmployeeRepository : IBaseRepository<Employee>
	{
		Task<List<Employee>> GetAllByFullNameAsync(string fullName);
		Task<List<Employee>> SortAsync(string sort);
	}
}
