﻿// Generated by https://quicktype.io

using Newtonsoft.Json;

namespace Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel
{
    public class RegionJson
    {
        [JsonProperty("region_id")]
        public int RegionId { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_experimental")]
        [JsonConverter(typeof(BoolConverter))]
        public bool RegionExperimental { get; set; }
    }
}