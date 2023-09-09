using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using WrathRandomEquipment.RandomEquipment;

namespace WrathRandomEquipment.Patches
{
    internal class Initializations
    {
        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCache_Patch
        {
            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            static void Postfix()
            {
                REModMenu.RandomEquipmentSettings.Initialize();
                RandomEquipmentController.Initialize();
                LootListDownloader.Download();
            }
        }
    }
}
