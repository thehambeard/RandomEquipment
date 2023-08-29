﻿using Kingmaker.RuleSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using static WrathRandomEquipment.Utility.Logger;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class RandomLootTable
    {
        public int ChanceToRollTable { get; private set; }
        public REFilters.LevelFilter DefaultLevelFilter { get; private set; }
        public Func<RandomItemEntry, bool> DefaultItemFilter { get; private set; }

        public string Name;

        private List<TableEntry> _tableEntries;
        private List<RandomLootTable> _lootTables;
        private RandomLootTable _parent;

        private RandomLootTable(string name)
        {
            Name = name;
            _tableEntries = new();
            _lootTables = new();
        }

        public static RandomLootTable New(string name)
        {
            return new RandomLootTable(name);
        }

        public RandomLootTable AddSubTable(RandomLootTable subTable)
        {
            subTable._parent = this;
            _lootTables.Add(subTable);
            return this;
        }

        public RandomLootTable AddTableEntry(TableEntry tableEntry)
        {
            _tableEntries.Add(tableEntry);
            return this;
        }

        public RandomLootTable SetChanceToRollTable(int chanceToRoll)
        {
            ChanceToRollTable = chanceToRoll;
            return this;
        }

        public RandomLootTable SetDefaultLevelFilter(REFilters.LevelFilter levelFilter)
        {
            DefaultLevelFilter = levelFilter;
            return this;
        }

        public RandomLootTable SetDefaultItemFilter(Func<RandomItemEntry, bool> itemFilter)
        {
            DefaultItemFilter = itemFilter;
            return this;
        }

        public List<ItemResult> Roll()
        {
            var roll = RulebookEvent.Dice.D(1, DiceType.D100);

            VLog($"Chance of container having random loot: {ChanceToRollTable}. A {roll} was rolled...");
            CLog($"Chance ({Name}): {ChanceToRollTable}% Rolled: {roll}");

            if (roll > ChanceToRollTable)
                return null;

            VLog($"Success! Rolling {_lootTables.Count} subtables now...");

            if (DefaultLevelFilter != null)
                DefaultLevelFilter.Update();

            List<ItemResult> result = new();

            foreach (var table in _lootTables)
            {
                var r = table.Roll();
                if (r != null && !r.Empty())
                    result.AddRange(r);
            }

            int accumulator = 0;
            roll = RulebookEvent.Dice.D(1, DiceType.D100);

            VLog($"Rolling on table {Name}: Rolled a {roll}");

            ItemResult rollResult = new();

            foreach (var tableEntry in _tableEntries)
            {
                accumulator += tableEntry.PercentChance;

                if (roll <= accumulator)
                {
                    Func<RandomItemEntry, bool> itemFilter = tableEntry.ItemFilter == null ? TryGetItemFilter() : tableEntry.ItemFilter;
                    REFilters.LevelFilter levelFilter;

                    if (tableEntry.LevelFilter == null)
                        levelFilter = TryGetLevelFilter();
                    else
                    {
                        levelFilter = tableEntry.LevelFilter;
                        levelFilter.Update();
                    }

                    rollResult.Guid = RandomEquipmentHandler.Instance.GetRandom(itemFilter, levelFilter);
                    rollResult.Count = RulebookEvent.Dice.D(1, tableEntry.Amount);

                    break;
                }
            }

            if (!rollResult.IsEmpty)
                result.Add(rollResult);
            else
                VLog("No items added...");

            return result;
        }

        public REFilters.LevelFilter TryGetLevelFilter()
        {
            if (DefaultLevelFilter != null) return DefaultLevelFilter;
            if (_parent == null) return null;
            return TryGetLevelFilter(_parent);
        }

        private REFilters.LevelFilter TryGetLevelFilter(RandomLootTable instance)
        {
            return instance.TryGetLevelFilter();
        }

        public Func<RandomItemEntry, bool> TryGetItemFilter()
        {
            if (DefaultItemFilter != null) return DefaultItemFilter;
            if (_parent == null) return null;
            return TryGetItemFilter(_parent);
        }

        private Func<RandomItemEntry, bool> TryGetItemFilter(RandomLootTable instance)
        {
            return instance.TryGetItemFilter();
        }

        public class TableEntry
        {
            public int PercentChance;
            public Func<RandomItemEntry, bool> ItemFilter;
            public REFilters.LevelFilter LevelFilter;
            public DiceType Amount;

            public TableEntry(int percentChance = 100, DiceType amount = DiceType.One, REFilters.LevelFilter levelFilter = null, Func<RandomItemEntry, bool> itemFilter = null)
            {
                PercentChance = percentChance;
                ItemFilter = itemFilter;
                LevelFilter = levelFilter;
                Amount = amount;
            }
        }

        public class ItemResult
        {
            public string Guid;
            public int Count;

            public bool IsEmpty => Guid.IsNullOrEmpty();
        }
    }
}
