using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
	public class MockEmployeeRepository : IEmployeeRepository
	{
		private List<Employee> _employees;

		public MockEmployeeRepository()
		{
			_employees = new List<Employee>();
			_employees.Add(new Employee() { Id=1,Name="Johnson",Department=Dept.IT,Email="test@123.com"});
			_employees.Add(new Employee() { Id = 2, Name = "Thomas", Department = Dept.Teacher, Email = "test12@123.com" });
			_employees.Add(new Employee() { Id = 3, Name = "Mathew", Department = Dept.Police, Email = "test34@123.com" });
		}

		public Employee Add(Employee employee)
		{
			employee.Id = _employees.Select(x => x.Id).Max() + 1;
			_employees.Add(employee);
			return employee;
		}

		public Employee Delete(int id)
		{
			throw new NotImplementedException();
		}

		public List<Employee> GetAllEmployees()
		{
			return _employees;
		}

		public Employee GetEmployeeDetails(int id)
		{
			return _employees.Where(x => x.Id == id).FirstOrDefault();
		}

		public Employee Update(Employee employeeChanges)
		{
			throw new NotImplementedException();
		}
	}
}
