//

namespace EmployeesList.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
