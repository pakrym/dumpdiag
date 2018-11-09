using System;
using System.Collections.Generic;

namespace DumpDiag.Console
{
    public static class TableExtensions
    {
        public static IEnumerable<IEnumerable<string>> ToTable<T, T1>(this IDictionary<T, T1> dictionary, string keyColumn, string valueColumn)
        {
            yield return new[] { keyColumn, valueColumn };
            foreach (var pair in dictionary)
            {
                yield return new[] { Convert.ToString(pair.Key), Convert.ToString(pair.Value) };
            }
        }

        public static IEnumerable<IEnumerable<string>> ToTable<T>(
            this IEnumerable<T> list,
            Func<T, IEnumerable<string>> selector,
            params string[] columns
            )
        {
            yield return columns;
            foreach (var pair in list)
            {
                yield return selector(pair);
            }
        }
    }
}