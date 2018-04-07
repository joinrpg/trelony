using System.Threading.Tasks;
using Joinrpg.Common.EFCoreHelpers;
using Joinrpg.Trelony.DataAccess;
using Joinrpg.Trelony.DataModel;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    internal class ImportDriver
    {
        private JsonParser Parser { get; }
        private TrelonyContext Context { get; }
        private ILogger Logger { get; }

        public ImportDriver(JsonParser parser, TrelonyContext context, ILogger logger)
        {
            Parser = parser;
            Context = context;
            Logger = logger;
        }

        public async Task<bool> Import()
        {
            Logger.Information($"Start importing {nameof(MacroRegion)}");
            foreach (var regionJson in Parser.Regions.Values)
            {
                if (regionJson.RegionCode == "")
                {
                    continue; //skip meta regions
                }
                var region = new MacroRegion
                {
                    MacroRegionId = regionJson.RegionId,
                    MacroRegionName = regionJson.RegionName
                };

                await Context.MacroRegions.AddAsync(region);
            }

            await SaveWithIdentyInsert<MacroRegion>();

            foreach(var regionJson in Parser.SubRegions.Values)
            {
                var region = new SubRegion()
                {
                    SubRegionId = regionJson.SubRegionId,
                    MacroRegion = await Context.MacroRegions.FindAsync(regionJson.RegionId),
                    SubRegionName = regionJson.SubRegionName,
                };

                await Context.SubRegions.AddAsync(region);
            }

            await SaveWithIdentyInsert<SubRegion>();

            return true;
        }

        private async Task SaveWithIdentyInsert<T>()
        {
            Logger.Information($"Before save of {typeof(T).Name}");
            await Context.Database.OpenConnectionAsync();
            try
            {
                await Context.EnableIdentityInsert<T>();
                await Context.SaveChangesAsync();
                await Context.DisableIdentityInsert<T>();
            }
            finally
            {
                Context.Database.CloseConnection();
            }
            Logger.Information($"After save of {typeof(T).Name}");
        }
    }
}
