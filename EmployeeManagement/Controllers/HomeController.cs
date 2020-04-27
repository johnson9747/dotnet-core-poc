using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
	public class HomeController : Controller
	{
		private IEmployeeRepository _employeeRepository;
		private readonly IHostingEnvironment hostingEnvironment;

		public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
		{
			_employeeRepository = employeeRepository;
			this.hostingEnvironment = hostingEnvironment;
		}
		public ViewResult Index(int id = 1)
		{
			ViewBag.Title = "Home Page Title";
			Employee employee = _employeeRepository.GetEmployeeDetails(id);
			if (employee == null)
			{
				Response.StatusCode = 404;
				return View("EmployeeNotFound", id);
			}
			return View(employee);
		}
		public ViewResult All()
		{
			ViewBag.Title = "All Employees";
			List<Employee> employees = _employeeRepository.GetAllEmployees();
			return View(employees);
		}
		[HttpGet]
		public ViewResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(EmployeeCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				

				Employee newEmployee = new Employee
				{
					Name = model.Name,
					Email = model.Email,
					Department = model.Department,
					// Store the file name in PhotoPath property of the employee object
					// which gets saved to the Employees database table
					PhotoPath = ProcessUploadedFile(model)
				};

				_employeeRepository.Add(newEmployee);
				return RedirectToAction("Index", new { id = newEmployee.Id });
			}
			return View();
		}

		[HttpGet]
		public ViewResult Edit(int id)
		{
			Employee employee = _employeeRepository.GetEmployeeDetails(id);
			EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
			{
				Id = employee.Id,
				Name = employee.Name,
				Email = employee.Email,
				Department = employee.Department,
				ExistingPhotoPath = employee.PhotoPath
			};
			return View(employeeEditViewModel);
		}
		[HttpPost]
		public IActionResult Edit(EmployeeEditViewModel model)
		{
			// Check if the provided data is valid, if not rerender the edit view
			// so the user can correct and resubmit the edit form
			if (ModelState.IsValid)
			{
				Employee employee = _employeeRepository.GetEmployeeDetails(model.Id);
				// Update the employee object with the data in the model object
				employee.Name = model.Name;
				employee.Email = model.Email;
				employee.Department = model.Department;
				// If the user wants to change the photo, a new photo will be
				// uploaded and the Photo property on the model object receives
				// the uploaded photo. If the Photo property is null, user did
				// not upload a new photo and keeps his existing photo
				if (model.Photo != null)
				{
					// If a new photo is uploaded, the existing photo must be
					// deleted. So check if there is an existing photo and delete
					if (model.ExistingPhotoPath != null)
					{
						string filePath = Path.Combine(hostingEnvironment.WebRootPath,
							"images", model.ExistingPhotoPath);
						System.IO.File.Delete(filePath);
					}
					// Save the new photo in wwwroot/images folder and update
					// PhotoPath property of the employee object which will be
					// eventually saved in the database
					employee.PhotoPath = ProcessUploadedFile(model);
				}

				// Call update method on the repository service passing it the
				// employee object to update the data in the database table
				Employee updatedEmployee = _employeeRepository.Update(employee);

				return RedirectToAction("all");
			}
			return View(model);
		}
		public IActionResult TestEexception()
		{
			throw new Exception("An Error Occured");
		}
		private string ProcessUploadedFile(EmployeeCreateViewModel model)
		{
			string uniqueFileName = null;

			if (model.Photo != null)
			{
				string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.Photo.CopyTo(fileStream);
				}
			}

			return uniqueFileName;
		}
	}
}