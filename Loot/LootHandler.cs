using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModMaker;
using static RandomEquipment.Main;
using Kingmaker.PubSubSystem;
using Kingmaker;
using Kingmaker.EntitySystem;
using Kingmaker.View.MapObjects;
using Kingmaker.Utility;
using Kingmaker.EntitySystem.Entities;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.Blueprints.Area;
using RandomEquipment.Utilities;
using UnityEngine;
using System.IO;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.Blueprints.Loot;
using ModMaker.Utility;
using Kingmaker.RuleSystem;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Dungeon.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.View.MapObjects.InteractionRestrictions;

namespace RandomEquipment.Loot
{
    class LootHandler : IModEventHandler, IAreaLoadingStagesHandler, IAreaPartHandler
    {
        public int Priority => 300;

        public UnusedLootDict RegLoot = LootJSON.LoadJSON(File.ReadAllText($"{ModPath}/regular.json"));

        public void PrepareAreaLootRoll()
        {
            SetWrap.EnsureContainersChecked();
            
            foreach (var container in LootHelper.GetContainersCurrentArea())
            {
                var roll = RulebookEvent.Dice.D100;
                int? trickDc = container.InteractionLoot?.Owner?.Get<DisableDeviceRestrictionPart>()?.DC;
                if (roll <= SetWrap.RegularChance || (roll <= SetWrap.LockedChance && trickDc > 0))
                {
                    GiveRandom(container, CalculateLevel());
                }
                if (!SetWrap.ContainersChecked[Game.Instance.Player.GameId].Contains(container.InteractionLoot.Owner.UniqueId))
                    SetWrap.ContainersChecked[Game.Instance.Player.GameId].Add(container.InteractionLoot.Owner.UniqueId);
            }
        }

        public int Cost2Level(int cost)
        {
            return (int)(Math.Sqrt(cost / 160));
        }

        public int Level2Cost(int level)
        {
            return (int)(Math.Pow(level, 2) * 160);
        }
        
        public void GiveRandom(LootWrapper container, int level)
        {
            SetWrap.EnsureItemsGiven();
            var blueprint = RegLoot.LootDict.Where(p => p.Value.CR >= (level - 5 >= 1 ? level - 5 : 1) && p.Value.CR <= level && p.Value.Cost <= Level2Cost(level))?.Random().Key;
            if (blueprint.IsNullOrEmpty() || SetWrap.ItemsGiven[Game.Instance.Player.GameId].Contains(blueprint))
            {
                if (3 < level && level < 20)
                    GiveRandom(container, level + 1);
                return;
            }

            var item = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(blueprint).CreateEntity();

            Mod.Debug(container.InteractionLoot.Source.name);
            if (container.InteractionLoot.Settings.ItemRestriction.guid.IsNullOrEmpty())
            {
                if (!SetWrap.ItemsGiven[Game.Instance.Player.GameId].Contains(blueprint))
                    SetWrap.ItemsGiven[Game.Instance.Player.GameId].Add(blueprint);
                Mod.Debug($"Gave: {blueprint}");
                container.InteractionLoot.Loot.Add(item);
            }
        }

        public int CalculateLevel()
        {
            var roll = RulebookEvent.Dice.D100;
            var level = Game.Instance.Player.PartyLevel;
            if (0 <= roll && roll < 10)
            {
                level -= 3;
            }
            else if (10 <= roll && roll < 25)
            {
                level -= 2;
            }
            else if (25 <= roll && roll < 40)
            {
                level -= 1;
            }
            else if (40 <= roll && roll < 80)
            {
                //no change
            }
            else if (80 <= roll && roll < 95)
            {
                level += 1;
            }
            else if (95 <= roll && roll < 99)
            {
                level += 2;
            }
            else if (roll == 100)
            {
                level += 3;
            }
            else
            {
                //no change
            }

            if (level < 1)
                level = 1;

            return level;
        }
        public void HandleModDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void HandleModEnable()
        {
            EventBus.Subscribe(this);
        }

        public void OnAreaLoadingComplete()
        {
            PrepareAreaLootRoll();
        }

        public void OnAreaPartChanged(BlueprintAreaPart previous)
        {
            PrepareAreaLootRoll();
        }

        public void OnAreaScenesLoaded()
        {
        }
    }
}
