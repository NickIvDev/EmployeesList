using EmployeesList.Models;
using EmployeesList.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesList.Controllers
{
    public class HomeController : Controller
    {
        DataBase _db;
        public HomeController(DataBase db)
        {
            _db = db;
            if (!_db.Departments.Any())
            {
                Department d1 = new Department() { DepartmentName = "Разработка ПО" };

                Position p1 = new Position() { PositionName = "Менеджер", Department = d1 };
                Position p2 = new Position() { PositionName = "Программист", Department = d1 };
                Position p3 = new Position() { PositionName = "Аналитик", Department = d1 };
                Position p4 = new Position() { PositionName = "Дизайнер", Department = d1 };
                Position p5 = new Position() { PositionName = "Тестировщик", Department = d1 };

                Employee e1 = new Employee() { FirstName = "Вася", LastName = "Петров", Age = 34, Department = d1, PositionStatus = p1.PositionName };
                Employee e2 = new Employee() { FirstName = "Толя", LastName = "Сидоров", Age = 45, Department = d1, PositionStatus = p2.PositionName };
                Employee e3 = new Employee() { FirstName = "Костя", LastName = "Иванов", Age = 23, Department = d1, PositionStatus = p3.PositionName };
                Employee e4 = new Employee() { FirstName = "Нина", LastName = "Ивашкина", Age = 25, Department = d1, PositionStatus = p4.PositionName };
                Employee e5 = new Employee() { FirstName = "Маша", LastName = "Краснова", Age = 27, Department = d1, PositionStatus = p5.PositionName };

                _db.Employees.AddRange(e1, e2, e3, e4, e5);
                _db.Positions.AddRange(p1, p2, p3, p4, p5);
                _db.Departments.Add(d1);
                _db.SaveChanges();

                Department d2 = new Department() { DepartmentName = "Отдел открытий" };

                Position p6 = new Position() { PositionName = "Ученый", Department = d2 };
                Position p7 = new Position() { PositionName = "Испытатель", Department = d2 };

                Employee e6 = new Employee() { FirstName = "Thomas", LastName = "Edison", Age = 29, Department = d2, PositionStatus = p6.PositionName };
                Employee e7 = new Employee() { FirstName = "Albert ", LastName = "Einstein", Age = 67, Department = d2, PositionStatus = p6.PositionName };
                Employee e8 = new Employee() { FirstName = "Nikola ", LastName = "Tesla", Age = 35, Department = d2, PositionStatus = p6.PositionName };
                Employee e9 = new Employee() { FirstName = "Николай", LastName = "Жуковский", Age = 45, Department = d2, PositionStatus = p6.PositionName };
                Employee e10 = new Employee() { FirstName = "Юрий", LastName = "Гагарин", Age = 68, Department = d2, PositionStatus = p7.PositionName };

                _db.Employees.AddRange(e6, e7, e8, e9, e10);
                _db.Positions.Add(p6);
                _db.Departments.Add(d2);
                _db.SaveChanges();
            }
        }
        public IActionResult AddEmployee()
        {
            AddEmployeeViewModel model = new AddEmployeeViewModel();
            return View(model);
        }
        public IActionResult ChangeStruct()
        {
            return View();
        }
        public IActionResult ShowEmloyeesList(ShowEmloyeesListViewModel model)
        {
            if (model.Department==null || model.Department=="Все отделы")
            {
                List<Employee> employees = _db.Employees.ToList();
                foreach (var item in employees)
                {
                    item.Department = _db.Departments.FirstOrDefault(d => d.Id == item.DepartmentId);
                }
                model.Department = "Все отделы";
                model.Employees = employees;
            }
            else
            {
                Department department = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.Department);
                List<Employee> employees = _db.Employees.Where(e=>e.DepartmentId==department.Id).ToList();
                foreach (var item in employees)
                {
                    item.Department = _db.Departments.FirstOrDefault(d => d.Id == item.DepartmentId);
                }
                model.Employees = employees;
            }
            
            return View(model);
        }
        public IActionResult ShowFormerEmloyeesList()
        {
            List<FormerEmployee> model = _db.FormerEmployees.ToList();
            foreach (var item in model)
            {
                item.Department = _db.Departments.FirstOrDefault(d => d.Id == item.DepartmentId);
            }
            return View(model);
        }
        public IActionResult ChangeEmployee(int idEmployee)
        {
            Employee employee = _db.Employees.FirstOrDefault(e => e.Id == idEmployee);
            Department department = _db.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
            AddEmployeeViewModel model = new AddEmployeeViewModel()
            {
                ItemId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                Department = department.DepartmentName,
                Position = employee.PositionStatus
            };
            return View(model);
        }
        public IActionResult RemoveDepartment(ChangeStructViewModel tempModel)
        {
            Department department = _db.Departments.FirstOrDefault(d => d.DepartmentName == tempModel.DepartmentName);
            List<Employee> employees = _db.Employees.Where(e => e.DepartmentId == department.Id).ToList();
            List<FormerEmployee> formerEmployees = _db.FormerEmployees.Where(f => f.DepartmentId == department.Id).ToList();

            RemoveDepartmentViewModel model = new RemoveDepartmentViewModel()
            {
                DepartmentName = tempModel.DepartmentName,
                Employees = employees,
                FormerEmployees = formerEmployees
            };

            if (model.Employees.Count() == 0 && model.FormerEmployees.Count() == 0)
            {
                _db.Departments.Remove(department);
                _db.SaveChanges();
                return RedirectToAction("ChangeStruct");
            }
            else
                return View(model);
        }
        public IActionResult CheckDepartmentOrePositionName() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult RemoveDepartmentAndAllEmployees(RemoveDepartmentViewModel model) 
        {
            Department department = _db.Departments.FirstOrDefault(d => d.DepartmentName == model.DepartmentName);
            _db.Departments.Remove(department);
            _db.SaveChanges();
            return RedirectToAction("ShowEmloyeesList");
        }
        [HttpPost]
        public IActionResult AddEmployeeToDb(AddEmployeeViewModel model) 
        {
            Department department = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.Department);
            string posName = model.Position;
            if (posName==null)
            {
                posName = "/Не установлена/";
            }

            Employee employee = new Employee()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Department = department,
                PositionStatus = posName
            };
            _db.Employees.Add(employee);
            _db.SaveChanges();

            return RedirectToAction("AddEmployee");
        }
        [HttpPost]
        public IActionResult ChangeEmployeeToDb(AddEmployeeViewModel model) 
        {
            Employee employee = _db.Employees.FirstOrDefault(e=>e.Id==model.ItemId);
            Department department = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.Department);

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Age = model.Age;
            if (model.Position==null)
            {
                employee.PositionStatus = "/Не установлена/";
            }
            else
            employee.PositionStatus = model.Position;
            employee.Department = department;
            employee.DepartmentId = department.Id;
            _db.SaveChanges();

            return RedirectToAction("ShowEmloyeesList");
        }
        public void ChangeEmployeeFrom_RemovedDepartment_ToDb(int idEmployee, string newDepartment, string newPosition, string statusEmployee) 
        {
            Department department = _db.Departments.FirstOrDefault(d => d.DepartmentName == newDepartment);
            switch (statusEmployee)
            {
                case "employee":
                    Employee employee = _db.Employees.FirstOrDefault(e => e.Id == idEmployee);
                    employee.DepartmentId = department.Id;
                    employee.PositionStatus = newPosition;
                    break;
                case "formerEmployee":
                    FormerEmployee formerEmployee = _db.FormerEmployees.FirstOrDefault(f => f.Id == idEmployee);
                    formerEmployee.DepartmentId = department.Id;
                    formerEmployee.PositionStatus = newPosition;
                    break;
            }
            _db.SaveChanges();
        }
        public IActionResult RestoreEmployee(int idEmployee)
        {
            FormerEmployee formerEmployee = _db.FormerEmployees.FirstOrDefault(f => f.Id == idEmployee);
            Employee employee = new Employee()
            {
                FirstName = formerEmployee.FirstName,
                LastName = formerEmployee.LastName,
                Age = formerEmployee.Age,
                PositionStatus = formerEmployee.PositionStatus,
                DepartmentId = formerEmployee.DepartmentId
            };
            _db.Employees.Add(employee);
            _db.FormerEmployees.Remove(formerEmployee);
            _db.SaveChanges();

            return RedirectToAction("ShowFormerEmloyeesList");
        }       
        public IActionResult RemoveEmloyeeToBasket(int idEmployee, string setDepartment) 
        {
            Employee employee = _db.Employees.FirstOrDefault(e => e.Id == idEmployee);
            FormerEmployee formerEmployee = new FormerEmployee() 
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                PositionStatus = employee.PositionStatus,
                DepartmentId = employee.DepartmentId
            };
            _db.FormerEmployees.Add(formerEmployee);
            _db.Employees.Remove(employee);
            _db.SaveChanges();

            return RedirectToAction("ShowEmloyeesList", new ShowEmloyeesListViewModel { Department=setDepartment });
        }       
        public IActionResult RemoveFormerEmloyeeFromDb(int idEmployee) 
        {
            FormerEmployee formerEmployee = _db.FormerEmployees.FirstOrDefault(f => f.Id == idEmployee);
            _db.FormerEmployees.Remove(formerEmployee);
            _db.SaveChanges();

            return RedirectToAction("ShowFormerEmloyeesList");
        }
        [HttpPost]
        public IActionResult ChangeStructToDb(ChangeStructViewModel model)
        {
            switch (model.ChangeStatus)
            {
                case "AddDepartment":
                    Department tempDepartment = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.DepartmentName);
                    if (tempDepartment!=null)
                    {
                        return RedirectToAction("CheckDepartmentOrePositionName");
                    }

                    Department d1 = new Department() { DepartmentName = model.DepartmentName };
                    _db.Departments.Add(d1);
                    _db.SaveChanges();
                    break;

                case "AddPositionToDepartment":                    
                    Department d2 = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.DepartmentName);

                    List<Position> positions = _db.Positions.Where(p => p.DepartmentId == d2.Id && p.PositionName==model.PositionName).ToList();
                    if (positions.Count()!=0)
                    {
                        return RedirectToAction("CheckDepartmentOrePositionName");
                    }

                    Position p1 = new Position()
                    {
                        PositionName = model.PositionName,
                        DepartmentId = d2.Id
                    };
                    _db.Positions.Add(p1);
                    _db.SaveChanges();
                    break;

                case "RemovePosition":
                    Department d6 = _db.Departments.FirstOrDefault(d => d.DepartmentName == model.DepartmentName);
                    Position p3 = _db.Positions.FirstOrDefault(p => p.PositionName == model.PositionName && p.DepartmentId == d6.Id);

                    List<Employee> e3 = _db.Employees.Where(e => e.DepartmentId == d6.Id && e.PositionStatus == p3.PositionName).ToList();
                    foreach (var item in e3)
                    {
                        item.PositionStatus ="/Не установлена/" ;
                    }

                    List<FormerEmployee> f3 = _db.FormerEmployees.Where(f => f.DepartmentId == d6.Id && f.PositionStatus == p3.PositionName).ToList();
                    foreach (var item in f3)
                    {
                        item.PositionStatus = "/Не установлена/";
                    }

                    _db.Positions.Remove(p3);
                    _db.SaveChanges();
                    break;

                case "ChangeDepartmentName":
                    Department checkDepartment = _db.Departments.FirstOrDefault(d=>d.DepartmentName==model.NewName);
                    if (checkDepartment!=null)
                    {
                        return RedirectToAction("CheckDepartmentOrePositionName");
                    }

                    Department d3 = _db.Departments.FirstOrDefault(d => d.DepartmentName == model.DepartmentName);
                    d3.DepartmentName = model.NewName;
                    _db.SaveChanges();
                    break;

                case "ChangePositionName":
                    Department d4 = _db.Departments.FirstOrDefault(d => d.DepartmentName == model.DepartmentName);

                    Position checkPosition = _db.Positions.FirstOrDefault(p => p.DepartmentId == d4.Id && p.PositionName == model.NewName);
                    if (checkPosition!=null)
                    {
                        return RedirectToAction("CheckDepartmentOrePositionName");
                    }

                    Position p2 = _db.Positions.FirstOrDefault(p => p.PositionName == model.PositionName && p.DepartmentId == d4.Id);
                    p2.PositionName = model.NewName;

                    List<Employee> e1 = _db.Employees.Where(e => e.PositionStatus == model.PositionName && e.DepartmentId == d4.Id).ToList();
                    foreach (var item in e1)
                    {
                        item.PositionStatus = model.NewName;
                    }

                    List<FormerEmployee> f1 = _db.FormerEmployees.Where(f => f.PositionStatus == model.PositionName && f.DepartmentId == d4.Id).ToList();
                    foreach (var item in f1)
                    {
                        item.PositionStatus = model.NewName;
                    }

                    _db.SaveChanges();
                    break;
            }
            return RedirectToAction("ChangeStruct");
        }

        public List<string> GetDepatmentsToView()
        {
            List<Department> departments = _db.Departments.ToList();
            List<string> departmentsNames = new List<string>();

            foreach (var item in departments)
            {
                departmentsNames.Add(item.DepartmentName);
            }
            return departmentsNames;
        }
        public List<string> GetDepatmentPositionsToView(string depatmentName)
        {
            Department department = _db.Departments.FirstOrDefault(d => d.DepartmentName == depatmentName);

            List<Position> positions = _db.Positions.Where(p => p.DepartmentId == department.Id).ToList();

            List<string> strFromPositionNameToView = new List<string>();

            if (positions!=null)
            {
                foreach (var item in positions)
                {
                    strFromPositionNameToView.Add(item.PositionName);
                }
            }
            return strFromPositionNameToView;
        }
    }
}   
