using System.Threading.Tasks;
using Joinrpg.Trelony.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    internal static class DatabaseHelper
    {
       public static TrelonyContext CreateContext(this string sqlServerConnectionString)
        {
            var builder = new DbContextOptionsBuilder<TrelonyContext>();
            builder.UseSqlServer(sqlServerConnectionString);
            var trelonyContext = new TrelonyContext(builder.Options);
            return trelonyContext;
        }

        public static async Task<bool> EnsurePresent(this TrelonyContext trelonyContext)
        {
            try
            {
                await trelonyContext.Database.ExecuteSqlCommandAsync("SELECT 1");
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> ContainsData(this TrelonyContext trelonyContext)
        {
            return await trelonyContext.Games.AnyAsync() || await trelonyContext.MacroRegions.AnyAsync() ||
                   await trelonyContext.SubRegions.AnyAsync() || await trelonyContext.Polygons.AnyAsync();
        }

        public static async Task DropAndRecreate(this TrelonyContext trelonyContext)
        {
            await trelonyContext.Database.EnsureDeletedAsync();
            await trelonyContext.Database.MigrateAsync();
        }
    }
}
