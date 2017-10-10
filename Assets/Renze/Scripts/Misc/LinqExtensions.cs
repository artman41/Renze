using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq {
    public static class LinqExtensions {

        public static void ForEach<T1>(this IEnumerable<T1> collection, Action<T1> a) {
            foreach (var item in collection) {
                a(item);
            }

        }
    }
}
