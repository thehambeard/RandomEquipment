using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModMaker;
using UnityModManagerNet;
using UnityEngine;
using System.IO;
using RandomEquipment.Loot;
using static RandomEquipment.Main;

namespace RandomEquipment.Menus
{
    class MainMenu : IMenuTopPage
    {
        public string Name => "Main Menu";

        public int Priority => 100;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
#if DEBUG
            if(GUILayout.Button("Test"))
            {
                Loot.LootHandler test = new Loot.LootHandler();
                test.PrepareAreaLootRoll();
            }
#endif
        }
    }
}
