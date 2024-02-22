using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WrathRandomEquipment.Utility;


namespace WrathRandomEquipment.RandomEquipment
{
    [Serializable]
    internal class RandomEquipmentHandler : IAreaLoadingStagesHandler, IDisposable
    {
        private static RandomEquipmentHandler _instance;

        private RandomItemDictionary _itemDictionary;
        private readonly string _fileName = "UsedItems.json";
        private string _playerGuid;

        [JsonProperty("UsedItems")]
        public HashSet<string> UsedItems;
        private RandomEquipmentHandler()
        {
            _playerGuid = string.Empty;
            _itemDictionary = new();
            _itemDictionary.Load();

            EventBus.Subscribe(this);
        }

        public static RandomEquipmentHandler Instance => _instance;

        public static RandomEquipmentHandler Intitialize()
        {
            _instance = new();
            return _instance;
        }

        public async Task Refresh()
        {
            _itemDictionary = new();
            _itemDictionary.Load();
        }

        public void PostLoad()
        {
            if (Game.Instance.Player != null && _playerGuid != Game.Instance.Player.GameId)
            {
                _playerGuid = Game.Instance.Player.GameId;

                var path = Path.Combine(Main.ModEntry.Path, "UsedLists", $"{Game.Instance.Player.MainCharacter.Value.CharacterName}_{_playerGuid}_{_fileName}");

                if (File.Exists(path))
                    UsedItems = JSON.FromJSON<HashSet<string>>(File.ReadAllText(path));
                else
                    UsedItems = new();
            }
        }

        public void Save()
        {
            var path = Path.Combine(Main.ModEntry.Path, "UsedLists");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var file = Path.Combine(path, $"{Game.Instance.Player.MainCharacter.Value.CharacterName}_{_playerGuid}_{_fileName}");

            File.WriteAllText(file, JSON.ToJSON(UsedItems));
        }

        public string GetRandom(Func<RandomItemEntry, bool> itemFilter = null, REFilters.LevelFilter levelFilter = null, int retries = 1)
        {
            if (levelFilter != null)
            {
                Logger.VLog($"Level modifier of {levelFilter.Modifier} will be applied to the party level for determining the quality of the random item...");
            }

            var results = _itemDictionary.Items.Where(item => (itemFilter != null ? itemFilter(item.Value) : true) && (levelFilter != null ? levelFilter.IsValid(item.Value) : true))
                .Select(k => k.Key)
                .Except(UsedItems);

            Logger.VLog($"{results.Count()} items found to pick from");
            var blueprint = results.Random();

            if (blueprint.IsNullOrEmpty())
            {
                if (retries > 0)
                {
                    Logger.VLog($"Valid blueprint not found, attempting {retries} more time(s)...");
                    blueprint = GetRandom(itemFilter, levelFilter + 1, retries - 1);
                }
            }
            else
            {
                Logger.VLog($"Blueprint found {blueprint}");

                if (!REFilters.ItemFilters.UsableBasicFilter(_itemDictionary.Items[blueprint]))
                {
                    UsedItems.Add(blueprint);
                }
            }
            return blueprint;
        }

        public void Dispose() => EventBus.Unsubscribe(this);

        public void OnAreaScenesLoaded() { }

        public void OnAreaLoadingComplete() => PostLoad();
    }
}
