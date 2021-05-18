using Microsoft.EntityFrameworkCore;

namespace Dal.Ef.Base
{
    public abstract class ResetDbContext : DbContext
    {
        protected ResetDbContext() 
        {
        }

        protected ResetDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        internal void Reset()
        {
            ChangeTracker.Clear();
        }
    }
}