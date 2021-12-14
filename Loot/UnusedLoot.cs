using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace RandomEquipment.Loot
{
    [Serializable]
    internal class UnusedLootItem
    {
        [JsonProperty("Name")]
        public string Name;

        [JsonProperty("File")]
        public string File;

        [JsonProperty("Cost")]
        public int Cost;

        [JsonProperty("CR")]
        public int CR;

        [JsonProperty("Description")]
        public string Description;
    }

    internal class UnusedLootDict
    {
        [JsonProperty("Dict")]
        public Dictionary<string, UnusedLootItem> LootDict = new Dictionary<string, UnusedLootItem>();
    }

    internal static class LootJSON
    {
        public static UnusedLootDict LoadJSON(string json)
        {
            return JsonConvert.DeserializeObject<UnusedLootDict>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
