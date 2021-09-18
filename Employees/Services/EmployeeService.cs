using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Data;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDb employeeDb;

        public EmployeeService(IEmployeeDb employeeDb)
        {
            this.employeeDb = employeeDb;
        }

        public List<Employee> ReadEmployeesFile(string filePath)
        {
            var employees = new List<Employee>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var employee = new Employee()
                    {
                        EmployeeId = csv.GetField<int>("EmpID"),
                        ProjectId = csv.GetField<int>(" ProjectID"),
                        DateFrom = csv.GetField<DateTime>(" DateFrom")
                    };

                    var dateTo = csv.GetField(" DateTo").Trim();

                    if (dateTo == "NULL")
                    {
                        employee.DateTo = DateTime.Now;
                    } else
                    {
                        employee.DateTo = DateTime.ParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    employees.Add(employee);
                }
            }

            return employees;
        }

        public void AddEmployeesToDb(List<Employee> employees)
        {
            if (employees == null)
            {
                return;
            }

            foreach (var employee in employees)
            {
                employeeDb.AddEmployee(employee);
            }
        }
    }
}
