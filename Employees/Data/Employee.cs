using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Employee
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
