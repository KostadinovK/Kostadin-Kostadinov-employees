using System;
using System.Collections.Generic;
using Employees.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Employees.BindingModels;
using Employees.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Services;

namespace Employees.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IUploadService uploadService;

        public HomeController(IEmployeeService employeeService, IUploadService uploadService)
        {
            this.employeeService = employeeService;
            this.uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var employeePairs = employeeService.GetEmployeePairs();

            var viewModel = new EmployeePairsViewModel
            {
                EmployeePairs = new List<EmployeePairViewModel>()
            };

            foreach (var pair in employeePairs)
            {
                var employeePairViewModel = new EmployeePairViewModel
                {
                    EmployeeId = pair.EmployeeId1,
                    EmpoyeeId2 = pair.EmployeeId2,
                    DaysWorked = pair.DaysWorked,
                    ProjectIds = String.Join(", ", pair.ProjectIds)
                };

                viewModel.EmployeePairs.Add(employeePairViewModel);
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Upload(EmployeesFileBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var path = await uploadService.UploadCSVFileAsync(model.EmployeesFile);

            var employees = employeeService.ReadEmployeesFile(path);

            employeeService.AddEmployeesToDb(employees);

            uploadService.DeleteFile(path);

            return RedirectToAction("Index", "Home");
        }
    }
}
