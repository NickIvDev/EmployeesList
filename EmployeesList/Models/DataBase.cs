using Microsoft.EntityFrameworkCore;

namespace EmployeesList.Models
{
    public class DataBase : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FormerEmployee> FormerEmployees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DataBase(DbContextOptions<DataBase> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
