using System;
using Newtonsoft.Json;

namespace Joinrpg.Trelony.LegacyConsoleImporter.JsonDataModel
{
    class GameJsonTypeConverter : JsonConverter
    {
        private static GameJsonType? ValueForString(string str)
        {
            switch (str)
            {
                case "1": return GameJsonType.Forest;
                case "2": return GameJsonType.City;
                case "3": return GameJsonType.OnRentedPlace;
                case "4": return GameJsonType.Room;
                case "5": return GameJsonType.Convention;
                case "6": return GameJsonType.Ball;
                case "7": return GameJsonType.Bugurt;
                case "8": return GameJsonType.CityAndForest;
                case "9": return GameJsonType.CityAndRentedPlace;
                case "10": return GameJsonType.Tournament;
                case "11": return GameJsonType.Underground;
                case "12": return GameJsonType.AirsoftEvent;
                case "13": return GameJsonType.Festival;
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

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GameJsonType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (GameJsonType) value;
            switch (val)
            {
                case GameJsonType.Forest: serializer.Serialize(writer, "1"); break;
                case GameJsonType.City: serializer.Serialize(writer, "2"); break;
                case GameJsonType.OnRentedPlace: serializer.Serialize(writer, "3"); break;
                case GameJsonType.Room: serializer.Serialize(writer, "4"); break;


                case GameJsonType.Convention:
                    serializer.Serialize(writer, "5");
                    break;
                case GameJsonType.Ball:
                    serializer.Serialize(writer, "6");
                    break;
                case GameJsonType.Bugurt:
                    serializer.Serialize(writer, "7");
                    break;
                case GameJsonType.CityAndForest:
                    serializer.Serialize(writer, "8");
                    break;
                case GameJsonType.CityAndRentedPlace:
                    serializer.Serialize(writer, "9");
                    break;
                case GameJsonType.Tournament:
                    serializer.Serialize(writer, "10");
                    break;
                case GameJsonType.Underground:
                    serializer.Serialize(writer, "11");
                    break;
                case GameJsonType.AirsoftEvent:
                    serializer.Serialize(writer, "12");
                    break;
                case GameJsonType.Festival:
                    serializer.Serialize(writer, "13");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}