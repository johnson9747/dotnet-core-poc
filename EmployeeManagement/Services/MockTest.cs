using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Services
{
	public class MockTest : ITest
	{
		public string getTestData()
		{
			return "Hello This is mock";
		}
	}
}
