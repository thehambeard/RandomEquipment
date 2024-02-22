using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WrathRandomEquipment.Utility;

namespace WrathRandomEquipment.RandomEquipment
{
    [Serializable]
    public class RandomItemEntry
    {
        public enum RandomItemType
        {
            Unassigned,
            Weapon,
            Armor,
            Shield,
            Equipment,
            Potion,
            Scroll,
            Wand,
            OtherUsable,
            UtilityUsable
        }

        [JsonProperty("GUID")]
        public string GUID;

        [JsonProperty("CL")]
        public int CL;

        [JsonProperty("CR")]
        public int CR;

        [JsonProperty("Cost")]
        public int Cost;

        [JsonProperty("Type")]
        public RandomItemType Type;

        public RandomItemEntry(string guid, int cl, int cr, int cost, RandomItemType type)
        {
            GUID = guid;
            CL = cl;
            CR = cr;
            Cost = cost;
            Type = type;
        }

        public RandomItemEntry() { }


    }

    [Serializable]
    public class RandomItemDictionary
    {
        [JsonProperty("Items")]
        public Dictionary<string, RandomItemEntry> Items = new();

        public void Load()
        {
            Items = JSON.FromJSON<RandomItemDictionary>(File.ReadAllText($@"{Main.ModEntry.Path}/LootList.json")).Items;
        }
    }
}
