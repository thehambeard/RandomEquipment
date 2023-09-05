using Kingmaker;
using System;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class REFilters
    {
        public class ItemFilters
        {
            public static Func<RandomItemEntry, bool> WeaponFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Weapon);
            public static Func<RandomItemEntry, bool> ArmorFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Armor);
            public static Func<RandomItemEntry, bool> ShieldFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Shield);
            public static Func<RandomItemEntry, bool> EquipFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Equipment);
            public static Func<RandomItemEntry, bool> AllExceptUsable = (item) => WeaponFilter(item) || ArmorFilter(item) || ShieldFilter(item) || EquipFilter(item);
            public static Func<RandomItemEntry, bool> PotionFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Potion);
            public static Func<RandomItemEntry, bool> WandFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Wand);
            public static Func<RandomItemEntry, bool> ScrollFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.Scroll);
            public static Func<RandomItemEntry, bool> OtherUsableFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.OtherUsable);
            public static Func<RandomItemEntry, bool> UtilityUsableFilter = (item) => (item.Type == RandomItemEntry.RandomItemType.UtilityUsable);
            public static Func<RandomItemEntry, bool> UsableFilter = (item) => (WandFilter(item) || ScrollFilter(item) || PotionFilter(item) || OtherUsableFilter(item));
            public static Func<RandomItemEntry, bool> UsableBasicFilter = (item) => (WandFilter(item) || ScrollFilter(item) || PotionFilter(item));
        }

        public class LevelFilter
        {
            private readonly RandomNormalModifier _modifier;
            private int _retryMod = 0;

            public int Modifier => _modifier.CurrentModifier + _retryMod;

            public LevelFilter(RandomNormalModifier modifier)
            {
                _modifier = modifier;
            }

            public void Update()
            {
                _modifier.GetModifier();
            }

            public bool IsValid(RandomItemEntry item)
            {
                int level = Game.Instance.Player.PartyLevel;
                int minLevel;
                int modLevel;

                modLevel = level + _modifier.CurrentModifier + _retryMod;
                minLevel = modLevel - 1;

                var CRFlag = item.CR >= (minLevel >= 1 && minLevel < level ? minLevel : 1) && item.CR <= modLevel;
                var CLFLag = item.CL >= (minLevel >= 1 && minLevel < level ? minLevel : 1) && item.CL <= modLevel;
                return (CRFlag && CLFLag);
            }

            public static LevelFilter operator +(LevelFilter lf, int mod)
            {
                var result = new LevelFilter(lf._modifier);
                result._retryMod += mod;
                return result;
            }
        }
    }
}
