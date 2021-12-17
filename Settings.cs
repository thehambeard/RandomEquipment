using UnityEngine;
using ModMaker.Utility;
using UnityModManagerNet;
using System.Collections.Generic;

namespace RandomEquipment
{
    public class Settings : UnityModManager.ModSettings
    {
        //settings go here
        public string lastModVersion;
        public string localizationFileName;
        public SerializableDictionary<string, HashSet<string>> containersChecked;
        public SerializableDictionary<string, HashSet<string>> itemsGiven;
        public int regularChance;
        public int lockedChance;
    }
}
