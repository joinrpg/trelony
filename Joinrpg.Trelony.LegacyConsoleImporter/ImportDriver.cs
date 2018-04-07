using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Joinrpg.Common.EFCoreHelpers;
using Joinrpg.Common.Helpers;
using Joinrpg.Trelony.DataAccess;
using Joinrpg.Trelony.DataModel;
using Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
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
//            await ImportEntity(GameDateMapper, Parser.GameDates.Values);
//            await ImportEntity(AssocMapper, Parser.AllrpgAssocs.Values);

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

                GameStatus = ConvertStatus(arg.Status),
                GameType = ConvertType(arg.Type),

                Organizers = arg.Mg,
                PlayersCount = arg.PlayersCount,

            };
        }

        private GameType ConvertType(GameJsonType argType)
        {
            switch (argType)
            {
                case GameJsonType.Forest:
                    return GameType.Forest;
                case GameJsonType.City:
                    return GameType.City;
                case GameJsonType.OnRentedPlace:
                    return GameType.OnRentedPlace;
                case GameJsonType.Room:
                    return GameType.Room;
                case GameJsonType.Convention:
                    return GameType.Convention;
                case GameJsonType.Ball:
                    return GameType.Ball;
                case GameJsonType.Bugurt:
                    return GameType.Bugurt;
                case GameJsonType.CityAndForest:
                    return GameType.CityAndForest;
                case GameJsonType.CityAndRentedPlace:
                    return GameType.CityAndRentedPlace;
                case GameJsonType.Tournament:
                    return GameType.Tournament;
                case GameJsonType.Underground:
                    return GameType.Underground;
                case GameJsonType.AirsoftEvent:
                    return GameType.AirsoftEvent;
                case GameJsonType.Festival:
                    return GameType.Festival;
                default:
                    throw new ArgumentOutOfRangeException(nameof(argType), argType, null);
            }
        }

        private GameStatus ConvertStatus(Status argStatus)
        {
            switch (argStatus)
            {
                case Status.Ok:
                    return GameStatus.ProbablyHappen;
                case Status.Finish:
                    return GameStatus.DefinitelyPassed;
                case Status.Unknown:
                    return GameStatus.UnknownStatus;
                case Status.Postponed:
                    return GameStatus.PostponedWithoutDate;
                case Status.Date:
                    return GameStatus.DateUnknown;
                case Status.Cancelled:
                    return GameStatus.DefinitelyCancelled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(argStatus), argStatus, null);
            }
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
