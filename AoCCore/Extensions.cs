using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AoCCore;

public static class Extensions
{
    public static IEnumerable<(TSource Value, int Index)> SelectIndex<TSource>(this IEnumerable<TSource> source)
    {
        return source.Select((x, i) => (x, i));
    }

    public static void Assert(this bool test)
    {
        if (!test)
        {
            P("ASSERT");
        }
    }

    public static void Assert(this Expression<Func<bool>> test)
    {
        if (!test.Compile()())
        {
            P($"ASSERT: {test}");
        }
    }

    public static void P(this string error)
    {
        Console.WriteLine(error);
    }
}