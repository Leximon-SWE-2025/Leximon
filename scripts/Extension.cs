using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count)
    {
        int[] indexes = [.. Enumerable.Range(0, source.Count())];
        //for (int i = 0; i < knownMoves.Count; i++)
        //    indexes[i] = i;

        // Shuffle only first n items
        for (int i = 0; i < count; i++)
        {

            int j = System.Random.Shared.Next(i, source.Count());
            (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
        }


        return source.Where((_, index) => indexes[..count].Contains(index));
    }

    public static T RandomItem<T>(this IEnumerable<T> source)
    {
        if (source.Count() == 1)
        {
            return source.Single();
        }
        var index = System.Random.Shared.Next(source.Count());
        return source.Where((_, i) => i == index).First();
    }
}
