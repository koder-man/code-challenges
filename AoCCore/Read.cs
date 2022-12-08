﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoCCore;

internal static class Read
{
    public static T Value<T>()
        where T : IParsable<T>
    {
        return T.Parse(Console.ReadLine(), null);
    }

    public static bool Try<T>(out T result)
        where T : IParsable<T>
    {
        return T.TryParse(Console.ReadLine(), null, out result);
    }

    public static IEnumerable<string> StringBatch()
    {
        string line;
        while ((line = Console.ReadLine()).Length > 0)
        {
            yield return line;
        }
    }

    public static IEnumerable<T> Batch<T>() where T : IParsable<T> => StringBatch().Select(x => T.Parse(x, null));

    public static IEnumerable<IEnumerable<T>> Batches<T>() where T : IParsable<T>
    {
        while (true) yield return Batch<T>();
    }

    public static int Int() => Value<int>();

    public static bool TryInt(out int value) => Try(out value);

    public static IEnumerable<int> IntBatch() => Batch<int>();

    public static IEnumerable<IEnumerable<int>> IntBatches() => Batches<int>();

    public static long Long() => Value<long>();

    public static bool TryLong(out long value) => Try(out value);

    public static IEnumerable<long> LongBatch() => Batch<long>();

    public static IEnumerable<IEnumerable<long>> LongBatches() => Batches<long>();
}