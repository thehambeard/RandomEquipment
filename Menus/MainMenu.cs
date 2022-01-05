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
using RandomEquipment.Utilities;

namespace RandomEquipment.Menus
{
    class MainMenu : IMenuTopPage
    {
        public string Name => "Main Menu";

        public int Priority => 100;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Chance to generate in regular container: {SetWrap.RegularChance}%  ", GUILayout.Width(300));
                    SetWrap.RegularChance = (int)GUILayout.HorizontalSlider(SetWrap.RegularChance, 0, 100, GUILayout.Width(400));
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Chance to generate in locked container: {SetWrap.LockedChance}%  ", GUILayout.Width(300));
                    SetWrap.LockedChance = (int)GUILayout.HorizontalSlider(SetWrap.LockedChance, 0, 100, GUILayout.Width(400));
                }
                using (new GUILayout.HorizontalScope())
                {
                    SetWrap.LogGen = GUILayout.Toggle(SetWrap.LogGen, "Log chest generation output", GUILayout.ExpandWidth(false));
                }
            }
#if DEBUG
            if (GUILayout.Button("Test"))
            {
                Loot.LootHandler test = new Loot.LootHandler();
                test.PrepareAreaLootRoll();
            }
#endif
        }
    }
}
