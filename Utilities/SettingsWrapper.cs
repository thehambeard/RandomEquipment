using System.Collections.Generic;
using static RandomEquipment.Main;
using ModMaker.Utility;
using Kingmaker;

namespace RandomEquipment.Utilities
{
    public static class SetWrap
    {
        public static string LocalizationFileName
        {
            get => Mod.Settings.localizationFileName;
            set => Mod.Settings.localizationFileName = value;
        }
        public static SerializableDictionary<string, HashSet<string>> ContainersChecked
        {
            get => Mod.Settings.containersChecked;
            set => Mod.Settings.containersChecked = value;
        }

        public static SerializableDictionary<string, HashSet<string>> ItemsGiven
        {
            get => Mod.Settings.itemsGiven;
            set => Mod.Settings.itemsGiven = value;
        }

        public static int RegularChance
        {
            get => Mod.Settings.regularChance;
            set => Mod.Settings.regularChance = value;
        }

        public static int LockedChance
        {
            get => Mod.Settings.lockedChance;
            set => Mod.Settings.lockedChance = value;
        }

        public static bool LogGen
        {
            get => Mod.Settings.logGen;
            set => Mod.Settings.logGen = value;
        }

        public static void EnsureContainersChecked()
        {
            if (SetWrap.ContainersChecked == null)
            {
                SetWrap.ContainersChecked = new SerializableDictionary<string, HashSet<string>>();
                SetWrap.ContainersChecked.Add(Game.Instance.Player.GameId, new HashSet<string>());
            }
            else if (!SetWrap.ContainersChecked.ContainsKey(Game.Instance.Player.GameId))
            {
                SetWrap.ContainersChecked.Add(Game.Instance.Player.GameId, new HashSet<string>());
            }
            else if (SetWrap.ContainersChecked[Game.Instance.Player.GameId] == null)
            {
                SetWrap.ContainersChecked[Game.Instance.Player.GameId] = new HashSet<string>();
            }
        }

        public static void EnsureItemsGiven()
        {
            if (SetWrap.ItemsGiven == null)
            {
                SetWrap.ItemsGiven = new SerializableDictionary<string, HashSet<string>>();
                SetWrap.ItemsGiven.Add(Game.Instance.Player.GameId, new HashSet<string>());
            }
            else if (!SetWrap.ItemsGiven.ContainsKey(Game.Instance.Player.GameId))
            {
                SetWrap.ItemsGiven.Add(Game.Instance.Player.GameId, new HashSet<string>());
            }
            else if (SetWrap.ItemsGiven[Game.Instance.Player.GameId] == null)
            {
                SetWrap.ItemsGiven[Game.Instance.Player.GameId] = new HashSet<string>();
            }
        }
    }
}