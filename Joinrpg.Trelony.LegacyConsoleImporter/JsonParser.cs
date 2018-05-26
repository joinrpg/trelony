using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Joinrpg.Common.Helpers;
using Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel;
using Newtonsoft.Json.Linq;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    internal class JsonParser
    {
        private readonly Lazy<IReadOnlyDictionary<int, AddUriJson>> _addUrisLazy;
        public IReadOnlyDictionary<int, AddUriJson> AddUris => _addUrisLazy.Value;

        private readonly Lazy<IReadOnlyDictionary<int, GameJson>> _gameJsonLazy;
        public IReadOnlyDictionary<int, GameJson> Games => _gameJsonLazy.Value;

        private readonly Lazy<IReadOnlyDictionary<int, GameDateJson>> _gameDateJsonLazy;
        public IReadOnlyDictionary<int, GameDateJson> GameDates => _gameDateJsonLazy.Value;

        private readonly Lazy<IReadOnlyDictionary<int, PolygonJson>> _gamePolygonsLazy;
        public IReadOnlyDictionary<int, PolygonJson> Polygons => _gamePolygonsLazy.Value;


        private readonly Lazy<IReadOnlyDictionary<int, RegionJson>> _gameRegionsLazy;
        public IReadOnlyDictionary<int, RegionJson> Regions => _gameRegionsLazy.Value;


        private readonly Lazy<IReadOnlyDictionary<int, SubRegionJson>> _gameSubRegionsLazy;
        public IReadOnlyDictionary<int, SubRegionJson> SubRegions => _gameSubRegionsLazy.Value;

        private readonly Lazy<IReadOnlyDictionary<int, AllrpgAssocJson>> _gameAllrpgLazy;
        public IReadOnlyDictionary<int, AllrpgAssocJson> AllrpgAssocs => _gameAllrpgLazy.Value;

        public static async Task<JsonParser> Create(string filename)
        {
            var jsonFile = await File.ReadAllTextAsync(filename);

            var jobject = JArray.Parse(jsonFile);
            return new JsonParser(jobject);
        }

        private JsonParser(JArray jobject)
        {
            _addUrisLazy = MakeLazy<AddUriJson>(jobject, "ki_add_uri", x => x.AddUriId);
            _gameJsonLazy = MakeLazy<GameJson>(jobject, "ki_games", x => x.Id);
            _gameDateJsonLazy = MakeLazy<GameDateJson>(jobject, "ki_game_date", x => x.GameDateId);
            _gamePolygonsLazy = MakeLazy<PolygonJson>(jobject, "ki_polygons", x => x.PolygonId);

            _gameRegionsLazy = MakeLazy<RegionJson>(jobject, "ki_regions", x => x.RegionId);
            _gameSubRegionsLazy = MakeLazy<SubRegionJson>(jobject, "ki_sub_regions", x => x.SubRegionId);
            _gameAllrpgLazy = MakeLazy<AllrpgAssocJson>(jobject, "ki_zayavka_allrpg", x => x.AllrpgZayvkaId);
        }

        private static Lazy<IReadOnlyDictionary<int, T>> MakeLazy<T>(JArray jobject, string tableName,
            Func<T, int> keySelector) =>
            new Lazy<IReadOnlyDictionary<int, T>>(() =>
                ParseTable<DataTable<T>>(jobject, tableName).Data
                    .ToDictionary(keySelector).AsReadOnly());

        private static T ParseTable<T>(JArray jobject, string tableName) => jobject.Single(o => o["name"]?.Value<string>() == tableName).ToObject<T>();
    }
}