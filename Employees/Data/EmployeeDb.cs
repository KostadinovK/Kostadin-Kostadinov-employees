using System;
using System.Collections.Generic;

namespace Data
{
    public class EmployeeDb : IEmployeeDb
    {
        private List<Employee> employees;
        private List<EmployeePair> employeePairs;

        public EmployeeDb()
        {
            employees = new List<Employee>();
            employeePairs = new List<EmployeePair>();
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

        public List<EmployeePair> GetEmployeePairs()
        {
            var e = new EmployeePair
            {
                EmployeeId1 = 1,
                EmployeeId2 = 2,
                DaysWorked = 12,
                ProjectIds = new List<int>() {12}
            };

            var e2 = new EmployeePair
            {
                EmployeeId1 = 123,
                EmployeeId2 = 44,
                DaysWorked = 132,
                ProjectIds = new List<int>() { 12,1,2 }
            };

            var e3 = new EmployeePair
            {
                EmployeeId1 = 5,
                EmployeeId2 = 4,
                DaysWorked = 2,
                ProjectIds = new List<int>() { 12,34 }
            };

            var e4 = new EmployeePair
            {
                EmployeeId1 = 21,
                EmployeeId2 = 25,
                DaysWorked = 52,
                ProjectIds = new List<int>() { 12 }
            };

            var e5 = new EmployeePair
            {
                EmployeeId1 = 43,
                EmployeeId2 = 4,
                DaysWorked = 12,
                ProjectIds = new List<int>() { 32 }
            };

            var e6 = new EmployeePair
            {
                EmployeeId1 = 6,
                EmployeeId2 = 9,
                DaysWorked = 122,
                ProjectIds = new List<int>() { 12 }
            };

            employeePairs.Add(e);
            employeePairs.Add(e2);
            employeePairs.Add(e3);
            employeePairs.Add(e4);
            employeePairs.Add(e5);
            employeePairs.Add(e6);

            return employeePairs;
        }
    }
}
