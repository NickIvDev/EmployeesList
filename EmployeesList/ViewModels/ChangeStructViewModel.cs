using System.ComponentModel.DataAnnotations;

namespace EmployeesList.ViewModels
{
    public class ChangeStructViewModel
    {
        [Required (ErrorMessage = "Введите название отдела")]
        public string DepartmentName  { get; set; }

        [Required(ErrorMessage = "Введите название должности")]
        public string PositionName { get; set; }

        [Required(ErrorMessage = "Введите новое название")]
        public string  NewName { get; set; }
        public string ChangeStatus { get; set; }
    }
}
