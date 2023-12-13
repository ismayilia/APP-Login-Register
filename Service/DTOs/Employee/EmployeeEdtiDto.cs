using Service.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Employee
{
	public class EmployeeEdtiDto 
	{
		[Required]
		public string FullName { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public int Age { get; set; }
	}
}
