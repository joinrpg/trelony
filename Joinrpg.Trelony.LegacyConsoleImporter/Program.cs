using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;
using Serilog;

namespace Joinrpg.Trelony.LegacyConsoleImporter
{
    [Command(Name=nameof(LegacyConsoleImporter), Description = "Used to import json file from old kogda-igra database")]
    [HelpOption]
    public class Program
    {
        [Option(Description = "The file to import", LongName = "file", ShortName = "f")]
        private string JsonFileToImport { get; } = "db_kogda_1.json";

        [Option(Description = "Connection string for sql", LongName = "sql", ShortName = "s")]
        private string SqlServerConnectionString { get; } =
            "Server=(localdb)\\mssqllocaldb;Database=Trelony;Trusted_Connection=True;";

        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [UsedImplicitly]
        private async Task<int> OnExecuteAsync(CommandLineApplication commandLineApplication)
        {
            try
            {
                var log = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

                log.Information($"File to import: {JsonFileToImport}");
                log.Information($"Sql database to write: {SqlServerConnectionString}");
                AskForConfirmation('i', "import");

            

                log.Information("Loading file...");
                var parser = await JsonParser.Create(JsonFileToImport);
                log.Information("File loaded");

                bool importResult;

                using (var context = SqlServerConnectionString.CreateContext())
                {

                    if (!await context.EnsurePresent())
                    {
                        log.Error("Couldn't found database");
                        return 1;
                    }

                    if (await context.ContainsData())
                    {
                        log.Warning(
                            "Sql database already contains data. To continue operation we need to drop it");
                        AskForConfirmation('d', "dropping everything");
                        await context.DropAndRecreate();
                        log.Information("Dropping database complete");
                    }

                    var driver = new ImportDriver(parser, context, log);

                    importResult = await driver.Import();

                    log.Information("Operation completed");
                    Console.ReadLine();
                }

                return importResult ? 0 : 1;
            }
            catch (CancelException)
            {
                Console.ReadLine();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                return 1;
            }
        }

        private static void AskForConfirmation(char character, string operation)
        {
            Console.WriteLine($"Press {character} to start {operation}, anything else to cancel");
            var key = Console.ReadKey(intercept: true);
            if (key.KeyChar != character)
            {
                Console.WriteLine("Cancelled");
                throw new CancelException();
            }
        }
    }
}
