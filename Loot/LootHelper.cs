using Kingmaker;
using Kingmaker.EntitySystem;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using RandomEquipment.Utilities;
using ModMaker.Utility;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace RandomEquipment.Loot
{
    public static class LootHelper
    {
        public static HashSet<string> DenialList = new HashSet<string>()
        {
            
        };
        public static IEnumerable<LootWrapper> GetContainersCurrentArea()
        {
            SetWrap.EnsureContainersChecked();
            var lootWrapperList = new List<LootWrapper>();

            var interactionLootParts = Game.Instance.State.MapObjects.All
                .Where<EntityDataBase>(e => e.IsInGame)
                .Select<EntityDataBase, InteractionLootPart>(i => i.Get<InteractionLootPart>())
                .Where<InteractionLootPart>(i => i?.Loot != Game.Instance.Player.SharedStash)
                .NotNull<InteractionLootPart>();

            var source = TempList.Get<InteractionLootPart>();

            foreach (var interactionLootPart in interactionLootParts)
            {
                if (!interactionLootPart.IsViewed && interactionLootPart.Settings.ItemRestriction.Guid == "" && !SetWrap.ContainersChecked[Game.Instance.Player.GameId].Contains(interactionLootPart?.Owner.UniqueId))
                    source.Add(interactionLootPart);
                else
                    if (SetWrap.LogGen) Main.Mod.Log("Excluding chest due to either already being viewed by player, restricted inventory, or already rolled");
            }

            var collection = source.Distinct<InteractionLootPart>((IEqualityComparer<InteractionLootPart>)new MassLootHelper.LootDuplicateCheck()).Select<InteractionLootPart, LootWrapper>((Func<InteractionLootPart, LootWrapper>)(i => new LootWrapper()
            {
                InteractionLoot = i
            }));

            lootWrapperList.AddRange(collection);
            return (IEnumerable<LootWrapper>)lootWrapperList;
        }

        public static int Worth(this LootWrapper container)
        {
            return container.InteractionLoot.Loot.Items.Select(i => i.Cost).Sum();
        }
    }
}
