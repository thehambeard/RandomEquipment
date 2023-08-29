using ModMaker;
using System;
using static WrathRandomEquipment.Main;
namespace WrathRandomEquipment
{
    class Core :
        IModEventHandler
    {
        public int Priority => 200;

        public void ResetSettings()
        {
            Mod.ResetSettings();
            Mod.Settings.lastModVersion = Mod.Version.ToString();
        }

        public void HandleModEnable()
        {
            if (!Version.TryParse(Mod.Settings.lastModVersion, out Version version) || version < new Version(0, 0, 0))
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