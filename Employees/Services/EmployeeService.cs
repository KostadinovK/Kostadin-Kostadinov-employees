using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Data;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDb employeeDb;

        public static string[] DateFormats =
        {
            "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd",
            "yyyy-dd-MM", "yyyy/dd/MM", "yyyy.dd.MM",
            "MM-dd-yyyy", "MM/dd/yyyy", "MM.dd.yyyy",
        };

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
                        employee.DateTo = DateTime.ParseExact(dateTo, DateFormats, CultureInfo.InvariantCulture);
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

        public List<EmployeePair> GetEmployeePairs()
        {
            var employees = employeeDb.GetEmployees();

            var dict = new Dictionary<Tuple<int, int>, Dictionary<int, int>>();

            var employeesGroupedByProjects = employees
                .GroupBy(e => e.ProjectId)
                .Select(g => g.OrderBy(o => o.DateFrom).ToList())
                .Where(g => g.Count > 1)
                .ToList();

            var employeePairs = new List<EmployeePair>();

            foreach (var employeesInProject in employeesGroupedByProjects)
            {
                var employeesInProjectById = employeesInProject
                    .GroupBy(e => e.EmployeeId)
                    .Select(g => g.ToList())
                    .ToList();

                for (int i = 0; i < employeesInProjectById.Count; i++)
                {
                    foreach (var emp in employeesInProjectById[i])
                    {
                        for (int j = i + 1; j < employeesInProjectById.Count; j++)
                        {
                            foreach (var nextEmp in employeesInProjectById[j])
                            {
                                if (emp.DateFrom > nextEmp.DateTo || emp.DateTo < nextEmp.DateFrom)
                                {
                                    continue;
                                }

                                var daysTogether = GetDaysTogetherOfAPair(emp.DateFrom, nextEmp.DateFrom, emp.DateTo, nextEmp.DateTo);

                                employeePairs.Add(new EmployeePair
                                {
                                    EmployeeId1 = emp.EmployeeId,
                                    EmployeeId2 = nextEmp.EmployeeId,
                                    ProjectId = emp.ProjectId,
                                    DaysWorked = daysTogether
                                });
                            }
                        }
                    }
                }
            }

            return employeePairs.OrderByDescending(p => p.DaysWorked).ToList();
        }

        private int GetDaysTogetherOfAPair(DateTime start1, DateTime start2, DateTime end1, DateTime end2)
        {
            var daysTogether = 0;

            if (start1 <= start2 && end1 <= end2)
            {
                daysTogether = (int)(end1 - start2).TotalDays;
            }
            else if (start1 >= start2 && end1 >= end2)
            {
                daysTogether = (int)(end2 - start1).TotalDays;
            }
            else if (start1 >= start2 && end1 <= end2)
            {
                daysTogether = (int)(end1 - start1).TotalDays;
            }
            else if (start1 <= start2 && end1 >= end2)
            {
                daysTogether = (int)(end2 - start2).TotalDays;
            }

            return daysTogether;
        }
    }
}
