using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Services
{
	public interface IEmployeeRepository
	{
		Employee GetEmployeeDetails(int id);
		List<Employee> GetAllEmployees();
		Employee Add(Employee employee);
		Employee Update(Employee employeeChanges);
		Employee Delete(int id);
	}
}
