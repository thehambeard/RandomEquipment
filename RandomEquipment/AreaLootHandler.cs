using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using Newtonsoft.Json;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class AreaLootHandler
    {
        private readonly string fileName = "_UsedChests.json";
        [JsonProperty("UsedChests")]
        //public Dictionary<string, List<ItemResult>> UsedChests;
        private string GUID;

        public void Init()
        {
            //UsedChests = new();
        }

        public void PostLoad()
        {
            //UsedChests.Clear();

            if (Game.Instance.Player == null) return;

            GUID = Game.Instance.Player.GameId;

            var path = Path.Combine(Main.ModPath, "UsedLists", $"{Game.Instance.Player.MainCharacter.Value.CharacterName}_{GUID}", $"{Game.Instance.CurrentlyLoadedArea.AssetGuid}_{fileName}");

            //if (File.Exists(path))
            //    UsedChests = JSON.FromJSON<Dictionary<string, List<ItemResult>>>(File.ReadAllText(path));
        }
        public void Finish()
        {
            var path = Path.Combine(Main.ModPath, "UsedLists", $"{Game.Instance.Player.MainCharacter.Value.CharacterName}_{GUID}");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var file = Path.Combine(path, $"{Game.Instance.CurrentlyLoadedArea.AssetGuid}_{fileName}");

            //if (!File.Exists(file))
            //    File.WriteAllText(file, JSON.ToJSON(UsedChests));
        }

        public IEnumerable<LootWrapper> GetAreaLoot()
        {
            if (Game.Instance.Player == null) return default(IEnumerable<LootWrapper>);

            var lootWrapperList = new List<LootWrapper>();

            var interactionLootParts = Game.Instance.State.MapObjects.All
                .Where<MapObjectEntityData>(e => e.IsInGame && !(e.View is DroppedLoot))
                .Select<MapObjectEntityData, InteractionLootPart>(i => i.Get<InteractionLootPart>())
                .Where<InteractionLootPart>(i => i?.Loot != Game.Instance.Player.SharedStash)
                .NotNull<InteractionLootPart>();

            var source = TempList.Get<InteractionLootPart>();

            foreach (var interactionLootPart in interactionLootParts)
            {
                if (interactionLootPart.Settings.ItemRestriction.Guid == "")
                    source.Add(interactionLootPart);
            }

            var collection = source.Distinct<InteractionLootPart>((IEqualityComparer<InteractionLootPart>)new MassLootHelper.LootDuplicateCheck()).Select<InteractionLootPart, LootWrapper>((Func<InteractionLootPart, LootWrapper>)(i => new LootWrapper()
            {
                InteractionLoot = i
            }));

            lootWrapperList.AddRange(collection);
            return (IEnumerable<LootWrapper>)lootWrapperList;
        }

        public void AddRandomLoot(RandomEquipmentHandler reh)
        {
            if (Game.Instance.Player == null) return;

            //var tables = new RandomTableWrapper(Game.Instance.Player.PartyLevel, reh);

            foreach (var loot in GetAreaLoot().Where(x => !x.InteractionLoot.IsViewed))
            {
                var id = loot.InteractionLoot.Owner.UniqueId;

                //if (UsedChests.ContainsKey(id) && !UsedChests[id].Any(item => loot.InteractionLoot.Loot.Select(x => x.Blueprint.AssetGuid.ToString()).Contains(item.GUID)))
                //{
                //    VLog($"Restoring container: {id}");
                //    foreach(var item in UsedChests[id])
                //        VLog($"{item.GUID} count: {item.Count}");
                //    loot.InteractionLoot.Loot.TryAddFromAssetId(UsedChests[id]);
                //}
                //else
                //{
                //    var result = new List<ItemResult>();

                //    if (loot.InteractionLoot.Owner.Get<DisableDeviceRestrictionPart>()?.DC > 0)
                //    {
                //        //VLog($"Rolling locked container: {id}");
                //    }
                //    else if (loot.InteractionLoot.Owner?.PerceptionCheckDC > 0)
                //    {
                //        //VLog($"Rolling hidden container: {id}");
                //    }
                //    else
                //    {
                //        VLog($"Rolling standard container: {id}");
                //        result = tables.StandardContainer.Roll();
                //    }


                //    if (result != null && loot.InteractionLoot.Loot.TryAddFromAssetId(result))
                //    {
                //        UsedChests[id] = result;
                //        foreach (var item in result)
                //            VLog($"{item.GUID} count: {item.Count}");
                //    }
                //}
            }
            Finish();
        }
    }
}
