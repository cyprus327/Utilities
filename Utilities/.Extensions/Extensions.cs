using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Extensions {
    internal static class EnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action) {
            foreach (var item in values) {
                action(item);
            }
        }
    }

    internal static class CharExtensions {
        public static bool Check(this char c, params char[] checks) {
            bool found = false;
            checks.ForEach(check => { if (check == c) found = true; });
            return found;
        }
    }
}
