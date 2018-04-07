using Newtonsoft.Json;

namespace Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel
{
    public partial class AddUriJson
    {
        [JsonProperty("add_uri_id")] public int AddUriId { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }

        [JsonProperty("allrpg_info_id")] public object AllrpgInfoId { get; set; }

        [JsonProperty("resolved")]
        [JsonConverter(typeof(BoolConverter))]
        public bool Resolved { get; set; }
    }
}