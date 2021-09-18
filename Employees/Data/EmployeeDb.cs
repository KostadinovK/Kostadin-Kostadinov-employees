using System;
using System.Collections.Generic;

namespace Data
{
    public class EmployeeDb : IEmployeeDb
    {
        private List<Employee> employees;

        public EmployeeDb()
        {
            employees = new List<Employee>();
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        } 

        public void AddEmployee(Employee emp)
        {
            if (emp == null)
            {
                throw new ArgumentNullException();
            }

            employees.Add(emp);
        }
    }
}
