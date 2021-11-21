using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesList.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указан возраст")]
        [Range(1,110, ErrorMessage ="Недопустимый возраст")]
        public int Age { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }

        public int ItemId { get; set; }
    }
}
