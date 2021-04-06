using Microsoft.EntityFrameworkCore;

namespace Server.Dal.Base
{
    public abstract class ResetDbContext : DbContext
    {
        protected ResetDbContext(DbContextOptions options) : base(options)
        {
        }

        internal abstract void Reset();
    }
}