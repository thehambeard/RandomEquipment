using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomEquipment.Utilities
{
    public static class Extensions
    {
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        Func<TValue, bool> predicate)
        {
            var keys = dict.Keys.Where(k => predicate(dict[k])).ToList();
            foreach (var key in keys)
            {
                dict.Remove(key);
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
        }
    }

}
