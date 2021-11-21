using EmployeesList.Models;
using System.Collections.Generic;

namespace EmployeesList.ViewModels
{
    public class RemoveDepartmentViewModel
    {
        public string DepartmentName { get; set; }
        public List<Employee> Employees { get; set; }
        public List<FormerEmployee> FormerEmployees { get; set; }
    }
}
