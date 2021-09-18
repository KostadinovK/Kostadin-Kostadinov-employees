using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Employees.BindingModels
{
    public class EmployeesFileBindingModel
    {
        [Required]
        public IFormFile EmployeesFile { get; set; }
    }
}
