using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using ModMenu.NewTypes;
using ModMenu.Settings;
using System.Collections.Generic;
using System.Linq;
using WrathRandomEquipment.RandomEquipment;
using WrathRandomEquipment.Utility;

namespace WrathRandomEquipment.REModMenu
{
    internal class RandomEquipmentSettings
    {
        private static RandomEquipmentSettings _instance;
        public static RandomEquipmentSettings Instance => _instance;
        private static readonly string _rootKey = "randomequipment.settings";
        public static readonly Dictionary<string, SettingKey> SettingKeys = new();
        public static readonly HashSet<string> Keys = new()
        {
            "standard",
            "locked",
            "hidden"
        };

        private RandomEquipmentSettings()
        {
            CreateSettingsStrings();
            CreateSettingKeys();
            CreateSettings();

            foreach (var kvp in SettingKeys)
                kvp.Value.BindAll();

            Logger.VLog("WrathRandomEquipment mod menu initialized");
        }

        internal static RandomEquipmentSettings Initialize()
        {
            _instance = new();
            return _instance;
        }

        private void CreateSettingKeys()
        {
            foreach (var key in Keys)
            {
                SettingKeys.Add(key, new SettingKey(_rootKey, key));
            }
        }

        private void CreateSettingsHelper(SettingsBuilder sb, string key, int mainDefault, int usablesDefault, int minDefault, int maxDefault, float shiftDefault, float scaleDefault)
        {
            sb.AddSubHeader(GetString($"{key}-sub-header"), true)
                .AddSliderInt(
                    SliderInt.New(
                        key: SettingKeys[key].Chance.Key,
                        defaultValue: mainDefault,
                        description: GetString($"chance-{key}-label"),
                        minValue: 0,
                        maxValue: 100)
                    .WithLongDescription(GetString($"chance-{key}-description")))
            .AddSliderInt(
                SliderInt.New(
                    key: SettingKeys[key].UsablesChance.Key,
                    defaultValue: usablesDefault,
                    description: GetString($"usables-{key}-label"),
                    minValue: 0,
                    maxValue: 100)
                .WithLongDescription(GetString($"usables-{key}-description")))
            .AddSliderInt(
                SliderInt.New(
                    key: SettingKeys[key].MinMod.Key,
                    defaultValue: minDefault,
                    description: GetString($"min-mod-{key}-label"),
                    minValue: -4,
                    maxValue: 4)
                .WithLongDescription(GetString($"min-mod-{key}-description"))
                .OnTempValueChanged((value) => OnMinModChanged(value, key)))
            .AddSliderInt(
                SliderInt.New(
                    key: SettingKeys[key].MaxMod.Key,
                    defaultValue: maxDefault,
                    description: GetString($"max-mod-{key}-label"),
                    minValue: -4,
                    maxValue: 4)
                .WithLongDescription(GetString($"max-mod-{key}-description"))
                .OnTempValueChanged((value) => OnMaxModChanged(value, key)))
            .AddSliderFloat(
                SliderFloat.New(
                    key: SettingKeys[key].Shift.Key,
                    defaultValue: shiftDefault,
                    description: GetString($"shift-{key}-label"),
                    minValue: -3,
                    maxValue: 3)
                .WithLongDescription(GetString($"shift-{key}-description"))
                .OnTempValueChanged((value) => OnShiftChanged(value, key)))
            .AddSliderFloat(
                SliderFloat.New(
                    key: SettingKeys[key].Scale.Key,
                    defaultValue: scaleDefault,
                    description: GetString($"scale-{key}-label"),
                    minValue: 0f,
                    maxValue: 5f)
                .WithLongDescription(GetString($"scale-{key}-description"))
                .OnTempValueChanged((value) => OnScaleChanged(value, key)));
        }

        private void CreateSettings()
        {
            var settings = SettingsBuilder.New(_rootKey, CreateString(GetKey("title"), "Random Equipment"));

            CreateSettingsHelper(settings, "standard", 5, 10, -2, 1, .3f, 1.5f);
            CreateSettingsHelper(settings, "hidden", 15, 20, -1, 2, -.5f, 1.8f);
            CreateSettingsHelper(settings, "locked", 20, 30, 0, 3, -2.4f, 2.1f);

            settings.AddSubHeader(CreateString("logging-sub-header", "Logging"), true)
                        .AddToggle(
                            Toggle.New(
                                key: GetKey("bool-verbose-logging"),
                                defaultValue: false,
                                description: GetString("verbose-logging-label"))
                            .WithLongDescription(GetString("verbose-logging-description")))
                        .AddToggle(
                            Toggle.New(
                                key: GetKey("bool-combat-logging"),
                                defaultValue: true,
                                description: GetString("combat-logging-label"))
                            .WithLongDescription(GetString("combat-logging-description")))
                    .AddSubHeader(CreateString("download-sub-header", "Auto Update Loot List"), true)
                        .AddToggle(
                            Toggle.New(
                                key: GetKey("bool-autoupdate"),
                                defaultValue: false,
                                description: GetString("autoupdate-label"))
                            .WithLongDescription(GetString("autoupdate-description")))
                        .AddButton(
                            Button.New(
                                description: GetString("download-button"),
                                buttonText: GetString("download-button-text"),
                                onClick: () => LootListDownloader.Download(true)))
                    .AddSubHeader(CreateString("deault-sub-header", "Defaults"), true)
                        .AddDefaultButton(() => EventBus.RaiseEvent<ISettingsChanged>(h => h.HandleApplySettings()));


            ModMenu.ModMenu.AddSettings(settings);
        }

        private void CreateSettingsStrings()
        {
            foreach (var key in Keys)
            {
                CreateString($"{key}-sub-header", $"{key.ToUpperFirstLetter()} Containers");
                CreateString($"chance-{key}-label", $"{key.ToUpperFirstLetter()} Chance of Random Loot");
                CreateString($"usables-{key}-label", $"{key.ToUpperFirstLetter()} Chance of Random Usables");
                CreateString($"min-mod-{key}-label", $"{key.ToUpperFirstLetter()} Minimum Level Modifier");
                CreateString($"max-mod-{key}-label", $"{key.ToUpperFirstLetter()} Maximum Level Modifier");
                CreateString($"shift-{key}-label", $"{key.ToUpperFirstLetter()} Random Modifier Curve Shift");
                CreateString($"scale-{key}-label", $"{key.ToUpperFirstLetter()} Random Curve Scale");
                CreateString($"chance-{key}-description", $"Percent chance that a {key} loot container contains random loot. The random loot will be based off your level. The main random item will be only given once and never given as a random award again");
                CreateString($"usables-{key}-description", "Chance of a random usable items to be added to the chest in addition to the main random item.");
                CreateString($"min-mod-{key}-description", "Sometimes your more or less lucky in the quality of the items found. This is the minimum level modifier that could be applied. A random modifier will be selected between the min and the max on each container. Results in the middle of the two modifiers are much more likely than the ends.");
                CreateString($"max-mod-{key}-description", "Sometimes your more or less lucky in the quality of the items found. This is the maximum level modifier that could be applied. A random modifier will be selected between the min and the max on each container. Results in the middle of the two modifiers are much more likely than the ends.");
                CreateString($"shift-{key}-description", "While the default random distribution curve makes the middle numbers much more likely to be picked than the min or max this will adjust where the peak of the curve is. Setting this number to negative will make the lower end of numbers more likely and the higher numbers less likely. Positive will have the reverse effect. Setting this too extreme could make the numbers at the other end of the range impossible to be choosen.");
                CreateString($"scale-{key}-description", "As stated above, the ends of the min-max range are much less likely than the middle. This setting will adjust the peak of the curve. A positive value will flatten the curve increasing the end of the range chances. A negative setting will increase the peak making the middle numbers even more likely to be picked.");

            }
            CreateString("combat-logging-label", "Show rolls in the combat history");
            CreateString("combat-logging-description", "This will show the chance of their being loot in the container and the roll you got in the combat history log when you interact with the container.");
            CreateString("verbose-logging-label", "Verbose logging");
            CreateString("verbose-logging-description", "Logs all mod activity to the Player.log file");
            CreateString("autoupdate-label", "Update the loot list upon game start up.");
            CreateString("autoupdate-description", "The list of all possible random loot items will update when the game starts up to ensure the latest changes are in effect.");
            CreateString("download-button", "Download latest loot list.");
            CreateString("download-button-text", "Download");
        }

        public void Update()
        {
            foreach (var key in Keys)
            {
                OnScaleChanged(SettingKeys[key].Scale.TempValue, key);
                OnShiftChanged(SettingKeys[key].Shift.TempValue, key);
            }
        }

        public void UpdateTables()
        {
            foreach (var key in Keys)
                UpdateTable(key);
        }

        private void UpdateTable(string key, string current = "")
        {
            List<string> tables = new() { "shift", "scale", "min-mod", "max-mod" };

            var distribution = new RandomNormalModifier(SettingKeys[key].MinMod.TempValue, SettingKeys[key].MaxMod.TempValue, SettingKeys[key].Shift.TempValue, SettingKeys[key].Scale.TempValue).GetDistribution(4000);

            foreach (var table in tables.Where(x => x != current))
            {
                CurveDescriptionUpdater.Update(
                    title: GetString($"{table}-{key}-label"),
                    original: GetString($"{table}-{key}-description"),
                    distribution: distribution);
            }

            CurveDescriptionUpdater.Update(
                    title: GetString($"{current}-{key}-label"),
                    original: GetString($"{current}-{key}-description"),
                    distribution: distribution);
        }

        private void OnShiftChanged(float value, string key)
        {
            UpdateTable(key, "shift");
        }

        private void OnScaleChanged(float value, string key)
        {
            UpdateTable(key, "scale");
        }

        private void OnMinModChanged(int value, string key)
        {
            var slider = SettingKeys[key].MaxMod.Setting;
            if (value > slider.m_TempValue)
                slider.SetTempValue(value);

            UpdateTable(key, "min");
        }

        private void OnMaxModChanged(int value, string key)
        {
            var slider = SettingKeys[key].MinMod.Setting;
            if (value < slider.m_TempValue)
                slider.SetTempValue(value);

            UpdateTable(key, "max");
        }

        private LocalizedString CreateString(string partialKey, string text)
        {
            return ModMenuHelpers.CreateString(GetKey(partialKey), text);
        }

        private LocalizedString GetString(string partialKey)
        {
            return ModMenuHelpers.GetString(GetKey(partialKey));
        }

        public static string GetKey(string partialKey)
        {
            return $"{_rootKey}.{partialKey}";
        }
    }
}
