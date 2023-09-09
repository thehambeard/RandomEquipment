using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.View.MapObjects;
using System;
using System.Collections.Generic;
using WrathRandomEquipment.Utility;
using static WrathRandomEquipment.RandomEquipment.RandomLootTable;


namespace WrathRandomEquipment.RandomEquipment
{
    internal class EntityLootHandler : IInteractionHandler, IDisposable
    {
        public EntityLootHandler()
        {
            EventBus.Subscribe(this);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnBeforeInteract(UnitEntityData unit, InteractionPart interaction)
        {
            var lootPart = (InteractionLootPart)interaction;

            if (lootPart == null || lootPart.IsViewed || lootPart.Settings.ItemRestriction.Guid != "" || lootPart.Loot == Game.Instance.Player.SharedStash) return;

            List<ItemResult> results = new();

            using (new Logger.ProcessLog("Random item fetch took: "))
            {
                if (lootPart.IsHidden())
                    results = RandomEquipmentController.Instance.LootTablesFromSettings["hidden"].Roll();
                else if (lootPart.IsLocked())
                    results = RandomEquipmentController.Instance.LootTablesFromSettings["locked"].Roll();
                else
                    results = RandomEquipmentController.Instance.LootTablesFromSettings["standard"].Roll();

                lootPart.Loot.TryAddFromAssetId(results);
            }
        }

        public void OnInteract(UnitEntityData unit, InteractionPart interaction) { }

        public void OnInteractionRestricted(UnitEntityData unit, InteractionPart interaction) { }
    }
}
