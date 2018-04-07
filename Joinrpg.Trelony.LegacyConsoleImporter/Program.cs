using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    [Command(Name=nameof(Joinrpg.Trelony.LegacyConsoleImporter), Description = "Used to import json file from old kogda-igra database")]
    [HelpOption]
    public class Program
    {
        [Option(Description = "The file to import", LongName = "file", ShortName = "f")]
        private string JsonFileToImport { get; } = "db_kogda_1.json";

        [Option(Description = "Connection string for sql", LongName = "sql", ShortName = "s")]
        private string SqlServerConnectionString { get; } =
            "Server=(localdb)\\mssqllocaldb;Database=Trelony;Trusted_Connection=True;";

        static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }

        [UsedImplicitly]
        async Task<int> OnExecuteAsync(CommandLineApplication commandLineApplication)
        {
            Console.WriteLine($"File to import: {JsonFileToImport}");
            Console.WriteLine($"Sql database to write: {SqlServerConnectionString}");
            Console.WriteLine("Press i to start import, anything else to cancel");
            var key = Console.ReadKey(intercept: true);
            if (key.KeyChar != 'i')
            {
                Console.WriteLine("Cancelled");
                return 1;
            }

            Console.WriteLine("Import");
            var importer = new Importer(JsonFileToImport, SqlServerConnectionString);
            return await importer.Import() ? 0 : 1;
        }
    }
}
