using System.Collections.Generic;
using Data;

namespace Services
{
    public interface IEmployeeService
    {
        List<Employee> ReadEmployeesFile(string filePath);

        void AddEmployeesToDb(List<Employee> employees);
    }
}
