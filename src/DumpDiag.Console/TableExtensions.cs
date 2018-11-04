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

        public static IEnumerable<IEnumerable<string>> ToTable<T, T1, T2>(
            this IList<T> list,
            string column1,
            string column2,
            Func<T, T1> selector1,
            Func<T, T2> selector2)
        {
            yield return new[] { column1, column2 };
            foreach (var pair in list)
            {
                yield return new[] { Convert.ToString(selector1(pair)), Convert.ToString(selector2(pair)) };
            }
        }
    }
}