using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Responses
{
	public class RegisterResponse
	{
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
    }
}
