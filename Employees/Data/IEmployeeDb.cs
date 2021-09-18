using System.Collections.Generic;

namespace Data
{
    public interface IEmployeeDb
    {
        List<Employee> GetEmployees();

        void AddEmployee(Employee emp);

        List<EmployeePair> GetEmployeePairs();
    }
}
