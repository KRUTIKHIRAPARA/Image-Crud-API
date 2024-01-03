using Microsoft.EntityFrameworkCore;

namespace ImageTest1.Models
{
    public class LaptopDBContext : DbContext
    {
        public LaptopDBContext(DbContextOptions option) : base(option)
        {
            
        }

        public DbSet<Laptop>  Laptops { get; set; }
    }
}
