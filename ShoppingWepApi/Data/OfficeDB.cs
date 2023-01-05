using Microsoft.EntityFrameworkCore;
using ShoppingWepApi.Models;


namespace ShoppingWepApi.Data
{
    public class OfficeDB : DbContext
    {
        public OfficeDB(DbContextOptions<OfficeDB> options ) : base(options)
        {
            
        }
        public DbSet<Employee> Employees => Set<Employee>();
    }
}
