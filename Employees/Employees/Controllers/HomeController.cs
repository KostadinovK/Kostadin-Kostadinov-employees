using Employees.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Employees.BindingModels;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Upload(EmployeesFileBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var employees = employeeService.ReadEmployeesFile(model.EmployeesFile.FileName);

            return RedirectToAction("Index", "Home");
        }
    }
}
