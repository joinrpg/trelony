using Joinrpg.Trelony.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.DataAccess
{
    public class TrelonyContext : DbContext
    {
        public DbSet<Game> Games { get; protected set; }

        public DbSet<MacroRegion> MacroRegions {get; protected set; }

        public DbSet<SubRegion> SubRegions { get; protected set; }

        public DbSet<Polygon> Polygons { get; private set; }

        public DbSet<User> Users { get; private set; }

        public TrelonyContext(DbContextOptions<TrelonyContext> options) : base (options)
        {

        }
    }
}
