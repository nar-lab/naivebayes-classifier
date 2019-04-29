using System.Data.Entity;

namespace NBayes.Models
{
    public class NbContext:DbContext
    {
        public NbContext():base("name=NbContext")
        {
            
        }
        public DbSet<Person> People { get; set; }
    }
}