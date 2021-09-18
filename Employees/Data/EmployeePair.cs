using System.Collections.Generic;

namespace Data
{
    public class EmployeePair
    {
        public int EmployeeId1 { get; set; }

        public int EmployeeId2 { get; set; }

        public List<int> ProjectIds { get; set; }

        public int DaysWorked { get; set; }
    }
}
