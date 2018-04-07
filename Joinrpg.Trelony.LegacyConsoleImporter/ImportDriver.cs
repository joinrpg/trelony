using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Joinrpg.Common.EFCoreHelpers;
using Joinrpg.Common.Helpers;
using Joinrpg.Trelony.DataAccess;
using Joinrpg.Trelony.DataModel;
using Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel;
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
            await ImportEntity(RegionMapper, Parser.Regions.Values);
            await ImportEntity(SubRegionMapper, Parser.SubRegions.Values);

            return true;
        }

        private static MacroRegion RegionMapper(RegionJson regionJson)
        {
            if (regionJson.RegionCode == "")
            {
                return null; //skip meta regions
            }

            return new MacroRegion
            {
                MacroRegionId = regionJson.RegionId,
                MacroRegionName = regionJson.RegionName
            };
        }

        private async Task<SubRegion> SubRegionMapper(SubRegionJson regionJson)
        {
            return new SubRegion()
            {
                SubRegionId = regionJson.SubRegionId,
                MacroRegion = await Context.MacroRegions.FindAsync(regionJson.RegionId),
                SubRegionName = regionJson.SubRegionName,
            };

        }

        private async Task ImportEntity<TOld, TNew>(Func<TOld, Task<TNew>> mapperFunc, IEnumerable<TOld> oldValues) where TNew : class
        {
            foreach (var regionJson in oldValues)
            {
                var region = await mapperFunc(regionJson);

                if (region != null)
                {
                    await Context.Set<TNew>().AddAsync(region);
                }
            }

            Logger.Information($"Before save of {typeof(TNew).Name}");
            await Context.Database.OpenConnectionAsync();
            try
            {
                await Context.EnableIdentityInsert<TNew>();
                await Context.SaveChangesAsync();
                await Context.DisableIdentityInsert<TNew>();
            }
            finally
            {
                Context.Database.CloseConnection();
            }
            Logger.Information($"After save of {typeof(TNew).Name}");
        }

        private Task ImportEntity<TOld, TNew>(Func<TOld, TNew> mapperFunc, IEnumerable<TOld> oldValues) where TNew : class
        {
            return ImportEntity(TaskHelpers.SyncTask(mapperFunc), oldValues);
        }
    }
}
