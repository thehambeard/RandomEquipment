using HarmonyLib;
using Kingmaker;
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
