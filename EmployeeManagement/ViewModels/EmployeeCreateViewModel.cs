using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
	public class EmployeeCreateViewModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public Dept? Department { get; set; }
		[Required]
		public string Email { get; set; }
		public IFormFile Photo { get; set; }
	}
}
