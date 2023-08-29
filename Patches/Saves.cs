using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrathRandomEquipment.RandomEquipment;

namespace WrathRandomEquipment.Patches
{
    internal class Saves
    {
        [HarmonyPatch(typeof(Game))]
        static class SaveGame_Patch
        {
            [HarmonyPatch(nameof(Game.SaveGame)), HarmonyPostfix]
            static void Postfix()
            {
                RandomEquipmentHandler.Instance.Save();
            }
        }
    }
}
