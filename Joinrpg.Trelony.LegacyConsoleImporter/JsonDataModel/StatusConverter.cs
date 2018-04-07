using System;
using Newtonsoft.Json;

namespace Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel
{
    public enum Status { Ok, Finish, Unknown, Postponed, Date, Cancelled };

    internal class StatusConverter: JsonConverter
    {
        private static Status? ValueForString(string str)
        {
            switch (str)
            {
                case "0": return Status.Ok;
                case "1": return Status.Finish;
                case "2": return Status.Unknown;
                case "3": return Status.Postponed;
                case "4": return Status.Date;
                case "5": return Status.Cancelled;
                default: return null;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case Status.Ok: serializer.Serialize(writer, "0"); break;
                case Status.Finish: serializer.Serialize(writer, "1"); break;
                case Status.Unknown: serializer.Serialize(writer, "2"); break;
                case Status.Postponed: serializer.Serialize(writer, "3"); break;
                case Status.Date: serializer.Serialize(writer, "4"); break;
                case Status.Cancelled: serializer.Serialize(writer, "5"); break;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Status);
        }
    }
}