using Newtonsoft.Json;

namespace WrathRandomEquipment.Utility
{
    internal class JSON
    {
        public static string ToJSON<T>(T input)
        {
            return JsonConvert.SerializeObject(input, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });
        }

        public static T FromJSON<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
