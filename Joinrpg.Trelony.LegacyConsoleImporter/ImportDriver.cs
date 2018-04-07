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
            await ImportEntity(PolygonMapper, Parser.Polygons.Values);
            await ImportEntity(GamesMapper, Parser.Games.Values);

            return true;
        }

        private async Task<Game> GamesMapper(GameJson arg) 
        {
            var subRegion = await Context.SubRegions.FindAsync(arg.SubRegionId);

            if (subRegion == null)
            {
                Logger.Warning("Game {name} ({id}) has subregion {subregion}, which is not exists",
                    arg.Name, arg.Id, arg.SubRegionId);
                return null;
            }

            Polygon polygon;
            if (arg.Polygon == 29 || arg.Polygon == 0)
            {
                polygon = null;
            }
            else
            {
                polygon = await Context.Polygons.FindAsync(arg.Polygon);

                if (polygon == null)
                {
                    Logger.Warning("Game {name} ({id}) has polygon {polygon}, which is not exists",
                        arg.Name, arg.Id, arg.Polygon);
                    return null;
                }
            }

            return new Game()
            {
                Polygon = polygon,
                SubRegion = subRegion,
                Active = !arg.DeletedFlag,

                Email = arg.Email,
                GameUrl = arg.Uri,
                FacebookLink = arg.FbComm,
                LivejournalLink = arg.LjComm,
                VkontakteLink = arg.VkClub,
                TelegramLink = null,

                GameId = arg.Id,
                GameName = arg.Name,

                GameStatus = GameStatus.UnknownStatus, //TODO
                GameType = GameType.Forest, //TODO

                Organizers = arg.Mg,
                PlayersCount = arg.PlayersCount,

            };
        }

        private async Task<Polygon> PolygonMapper(PolygonJson arg)
        {
            if (arg.MetaPolygon)
            {
                return null; 
            }

            var subRegion = await Context.SubRegions.FindAsync(arg.SubRegionId);

            if (subRegion == null)
            {
                Logger.Warning("Polygon {polygonName} ({polygonId}) has subregion {subregion}, which is not exists",
                    arg.PolygonName, arg.PolygonId, arg.SubRegionId);
                return null;
            }
            return new Polygon()
            {
                Active = !arg.DeletedFlag,
                PolygonId = arg.PolygonId,
                SubRegion = subRegion,
                PolygonName = arg.PolygonName,
            };
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
