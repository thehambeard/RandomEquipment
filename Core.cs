using Kingmaker.PubSubSystem;
using ModMaker;
using System;
using System.Reflection;
using RandomEquipment.Utilities;
using static RandomEquipment.Main;

namespace RandomEquipment
{
    class Core :
        IModEventHandler
    {
        public int Priority => 100;

        public void ResetSettings()
        {
            Mod.Settings.lockedChance = 25;
            Mod.Settings.regularChance = 5;
            Mod.Settings.lastModVersion = Mod.Version.ToString();
        } 

        public void HandleModEnable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            if (!Version.TryParse(Mod.Settings.lastModVersion, out Version version) || version < new Version(1, 0, 2))
                ResetSettings();
            else
            {
                Mod.Settings.lastModVersion = Mod.Version.ToString();
            }
        }

        public void HandleModDisable()
        {
        }
    }
}