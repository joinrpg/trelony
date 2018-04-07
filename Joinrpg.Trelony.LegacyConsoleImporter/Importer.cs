using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Joinrpg.Trelony.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    class Importer
    {
        public string JsonFileToImport { get; }
        public string SqlServerConnectionString { get; }

        public Importer([NotNull] string jsonFileToImport, [NotNull] string sqlServerConnectionString)
        {
            JsonFileToImport = jsonFileToImport ?? throw new ArgumentNullException(nameof(jsonFileToImport));
            SqlServerConnectionString = sqlServerConnectionString ?? throw new ArgumentNullException(nameof(sqlServerConnectionString));
        }

        [MustUseReturnValue]
        public async Task<bool> Import()
        {
            try
            {
                var parsers = await JsonParser.Create(JsonFileToImport);

                var data = parsers.Polygons;
                var z = parsers.Regions;
                var y = parsers.SubRegions;
                var x = parsers.AllrpgAssocs;
            }
            catch (Exception e)
            {
                Console.WriteLine("Parsing exception");
                Console.WriteLine(e);
                return false;
            }

            var builder = new DbContextOptionsBuilder<TrelonyContext>();
            builder.UseSqlServer(SqlServerConnectionString);
            using (var trelonyContext = new TrelonyContext(builder.Options))
            {
                
            }

            return true;
        }
    }
}
