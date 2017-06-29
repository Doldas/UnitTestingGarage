using System.Data.Entity;

namespace GarageAngularJS.DataAccess
{
    public class GarageContext : DbContext
    {
            public DbSet<Models.Vehicle> Vehicles { get; set; }
            public GarageContext() : base("DefaultConnection") { }
    }
}