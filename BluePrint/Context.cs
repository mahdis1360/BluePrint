using System.Data.Entity;

namespace BluePrint
{
    public class Context : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }

        public Context():base("name=BluePrint")
        {
                
        }
    }
}