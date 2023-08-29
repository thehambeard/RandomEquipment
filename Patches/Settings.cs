using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._PCView.Settings.Entities;
using Kingmaker.UI.MVVM._VM.Settings;
using WrathRandomEquipment.REModMenu;

namespace WrathRandomEquipment.Patches
{
    internal class Settings
    {
        [HarmonyPatch(typeof(SettingsVM))]
        static class ApplySettings_Patch
        {
            [HarmonyPatch(nameof(SettingsVM.ApplySettings)), HarmonyPostfix]
            static void Postfix()
            {
                EventBus.RaiseEvent<ISettingsChanged>(h => h.HandleSettingsChanged());
            }
        }

        [HarmonyPatch(typeof(SettingsEntitySliderPCView))]
        static class Initilialize_Patch
        {
            [HarmonyPatch(nameof(SettingsEntitySliderPCView.BindViewImplementation))]
            static void Postfix()
            {
                RandomEquipmentSettings.Instance.UpdateTables();
            }
        }
    }
}
