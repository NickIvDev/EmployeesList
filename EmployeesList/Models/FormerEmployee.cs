//

namespace EmployeesList.Models
{
    public class FormerEmployee
    {             
        public int Id { get; set; }       
        public string FirstName { get; set; }       
        public string LastName { get; set; }       
        public int Age { get; set; }
        public string PositionStatus { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
