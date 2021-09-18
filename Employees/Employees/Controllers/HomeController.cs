using System;
using Employees.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Employees.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using Services;

namespace Employees.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public HomeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public IActionResult Index()
        {
            return View();
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

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles");

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = "data.csv";

            var fullPath = Path.Combine(pathToSave, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                model.EmployeesFile.CopyTo(stream);
            }

            var employees = employeeService.ReadEmployeesFile(fullPath);

            employeeService.AddEmployeesToDb(employees);

            System.IO.File.Delete(fullPath);

            return RedirectToAction("Index", "Home");
        }
    }
}
