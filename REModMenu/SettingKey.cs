using Kingmaker.Settings;

namespace WrathRandomEquipment.REModMenu
{
    internal class SettingKey
    {
        public readonly ModSetting<int> Chance;
        public readonly ModSetting<int> UsablesChance;
        public readonly ModSetting<int> MinMod;
        public readonly ModSetting<int> MaxMod;
        public readonly ModSetting<float> Shift;
        public readonly ModSetting<float> Scale;

        public SettingKey(string rootKey, string key)
        {
            Chance = new($"{rootKey}.int.{key}.chance");
            UsablesChance = new($"{rootKey}.int.{key}.usables.chance");
            MinMod = new($"{rootKey}.int.{key}.minmod");
            MaxMod = new($"{rootKey}.int.{key}.maxmod");
            Shift = new($"{rootKey}.float.{key}.shift");
            Scale = new($"{rootKey}.float.{key}.scale");
        }

        public void BindAll()
        {
            Chance.BindSetting();
            UsablesChance.BindSetting();
            MinMod.BindSetting();
            MaxMod.BindSetting();
            Shift.BindSetting();
            Scale.BindSetting();
        }

        internal class ModSetting<T>
        {
            public readonly string Key;
            public SettingsEntity<T> Setting { get; private set; }
            public T Value => Setting.GetValue();
            public T TempValue => Setting.m_TempValue;

            public ModSetting(string key)
            {
                Key = key;
            }

            public void BindSetting()
            {
                Setting = ModMenu.ModMenu.GetSetting<SettingsEntity<T>, T>(Key);
            }
        }
    }
}
