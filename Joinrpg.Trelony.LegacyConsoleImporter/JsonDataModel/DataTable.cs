using Newtonsoft.Json;

namespace Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel
{
    public partial class DataTable<T>
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("database")] public string Database { get; set; }

        [JsonProperty("data")] public T[] Data { get; set; }
    }
}