using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel;
using Newtonsoft.Json.Linq;
using QuickType;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    internal class JsonParser
    {
        private readonly Lazy<IReadOnlyCollection<AddUriJson>> _addUrisLazy;
        public IReadOnlyCollection<AddUriJson> AddUris => _addUrisLazy.Value;

        private readonly Lazy<IReadOnlyCollection<GameJson>> _gameJsonLazy;
        public IReadOnlyCollection<GameJson> Games => _gameJsonLazy.Value;

        private readonly Lazy<IReadOnlyCollection<GameDateJson>> _gameDateJsonLazy;
        public IReadOnlyCollection<GameDateJson> GameDates => _gameDateJsonLazy.Value;

        private readonly Lazy<IReadOnlyCollection<PolygonJson>> _gamePolygonsLazy;
        public IReadOnlyCollection<PolygonJson> Polygons => _gamePolygonsLazy.Value;


        private readonly Lazy<IReadOnlyCollection<RegionJson>> _gameRegionsLazy;
        public IReadOnlyCollection<RegionJson> Regions => _gameRegionsLazy.Value;


        private readonly Lazy<IReadOnlyCollection<SubRegionJson>> _gameSubRegionsLazy;
        public IReadOnlyCollection<SubRegionJson> SubRegions => _gameSubRegionsLazy.Value;

        private readonly Lazy<IReadOnlyCollection<AllrpgAssocJson>> _gameAllrpgLazy;
        public IReadOnlyCollection<AllrpgAssocJson> AllrpgAssocs => _gameAllrpgLazy.Value;

        private JArray Jobject { get; }

        public static async Task<JsonParser> Create(string filename)
        {
            var jsonFile = await File.ReadAllTextAsync(filename);

            var jobject = JArray.Parse(jsonFile);
            return new JsonParser(jobject);
        }

        private JsonParser(JArray jobject)
        {
            Jobject = jobject;
            _addUrisLazy = MakeLazy<AddUriJson>(jobject, "ki_add_uri");
            _gameJsonLazy = MakeLazy<GameJson>(jobject, "ki_games");
            _gameDateJsonLazy = MakeLazy<GameDateJson>(jobject, "ki_game_date");
            _gamePolygonsLazy = MakeLazy<PolygonJson>(jobject, "ki_polygons");

            _gameRegionsLazy = MakeLazy<RegionJson>(jobject, "ki_regions");
            _gameSubRegionsLazy = MakeLazy<SubRegionJson>(jobject, "ki_sub_regions");
            _gameAllrpgLazy = MakeLazy<AllrpgAssocJson>(jobject, "ki_zayavka_allrpg");
        }

        private static Lazy<IReadOnlyCollection<T>> MakeLazy<T>(JArray jobject, string tableName) =>
            new Lazy<IReadOnlyCollection<T>>(() => ParseTable<DataTable<T>>(jobject, tableName).Data);

        private static T ParseTable<T>(JArray jobject, string tableName) => jobject.Single(o => o["name"]?.Value<string>() == tableName).ToObject<T>();
    }
}