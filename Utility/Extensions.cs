using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using Kingmaker.View.MapObjects.InteractionRestrictions;
using System;
using System.Collections.Generic;
using System.Linq;
using static WrathRandomEquipment.RandomEquipment.RandomLootTable;

namespace WrathRandomEquipment.Utility
{
    internal static class Extensions
    {
        private static readonly Random random = new Random();

        public static bool TryAddFromAssetId(this ItemsCollection collection, ItemResult item)
        {
            bool result = false;

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(item.Guid);
            if (result = (blueprint != null))
            {
                collection.Add(blueprint, item.Count);
                Logger.VLog($"Added {blueprint.Name} to the containter! Count: {item.Count}");
            }
            else
                Logger.VLog($"Blueprint not found: {item.Guid}");
            return result;
        }

        public static bool TryAddFromAssetId(this ItemsCollection collection, List<ItemResult> items)
        {
            foreach (var item in items)
            {
                if (!collection.TryAddFromAssetId(item))
                    return false;
            }
            return true;
        }

        public static bool IsLocked(this InteractionLootPart loot)
        {
            return loot.Owner.Get<DisableDeviceRestrictionPart>()?.DC > 0;
        }

        public static bool IsHidden(this InteractionLootPart loot)
        {
            return loot.Owner?.PerceptionCheckDC > 0;
        }

        public static bool IsDroppedByPlayer(this InteractionLootPart loot)
        {
            return loot.Owner.View is DroppedLoot && ((DroppedLoot)loot.Owner.View).IsDroppedByPlayer;
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[random.Next(0, list.Count)];
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}
