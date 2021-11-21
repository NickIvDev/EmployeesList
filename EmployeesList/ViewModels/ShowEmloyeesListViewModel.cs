using EmployeesList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesList.ViewModels
{
    public class ShowEmloyeesListViewModel
    {
        public string Department { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
