using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using WrathRandomEquipment.REModMenu;
using static WrathRandomEquipment.REModMenu.RandomEquipmentSettings;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class RandomEquipmentController : ISettingsChanged, IDisposable
    {
        private static RandomEquipmentController _instance;
        public static RandomEquipmentController Instance => _instance;
        public Dictionary<string, RandomLootTable> LootTablesFromSettings { get; set; }
        public EntityLootHandler EntityLootHandler { get; private set; }
        public void HandleSettingsChanged() => BuildLootTableFromSettings();

        public RandomEquipmentController()
        {
            EntityLootHandler = new();
            RandomEquipmentHandler.Intitialize();
            EventBus.Subscribe(this);
        }

        public static RandomEquipmentController Initialize()
        {
            _instance = new();
            _instance.BuildLootTableFromSettings();
            return _instance;
        }

        public void BuildLootTableFromSettings()
        {
            LootTablesFromSettings = new();

            foreach (var key in Keys)
            {
                LootTablesFromSettings.Add(key, BuildLootTableFromSettingsHelper(key));
            }
        }

        private RandomLootTable BuildLootTableFromSettingsHelper(string key)
        {
            var levelFilter = new REFilters.LevelFilter(new(
                SettingKeys[key].MinMod.Value,
                SettingKeys[key].MaxMod.Value,
                SettingKeys[key].Shift.Value,
                (double)SettingKeys[key].Scale.Value));

            return RandomLootTable.New("Equipment")
                .SetChanceToRollTable(SettingKeys[key].Chance.Value)
                .SetDefaultLevelFilter(levelFilter)
                .AddTableEntry(new
                (
                    percentChance: 100,
                    amount: DiceType.One,
                    tries: DiceType.One,
                    itemFilter: REFilters.ItemFilters.AllExceptUsable
                ))
                .AddSubTable
                (
                    RandomLootTable.New("Usables")
                    .SetChanceToRollTable(SettingKeys[key].UsablesChance.Value)
                    .SetDefaultLevelFilter(levelFilter)
                    .AddTableEntry(new
                    (
                        percentChance: 40,
                        amount: DiceType.D2,
                        tries: DiceType.D2,
                        itemFilter: REFilters.ItemFilters.PotionFilter
                    ))
                    .AddTableEntry(new
                    (
                        percentChance: 30,
                        amount: DiceType.One,
                        tries: DiceType.D3,
                        itemFilter: REFilters.ItemFilters.ScrollFilter
                    ))
                    .AddTableEntry(new
                    (
                        percentChance: 20,
                        amount: DiceType.One,
                        itemFilter: REFilters.ItemFilters.WandFilter
                    ))
                    .AddTableEntry(new
                    (
                        percentChance: 9,
                        amount: DiceType.One,
                        itemFilter: REFilters.ItemFilters.OtherUsableFilter
                    ))
                    .AddTableEntry(new
                    (
                        percentChance: 1,
                        amount: DiceType.One,
                        itemFilter: REFilters.ItemFilters.UtilityUsableFilter
                    ))
                );
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
            EntityLootHandler.Dispose();
        }

        public void OnAreaScenesLoaded() { }
    }
}
