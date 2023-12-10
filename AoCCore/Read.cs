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

    public static IEnumerable<string[]> StringBatches()
    {
        while (true)
        {
            var batch = StringBatch().ToArray();

            if (batch.Length == 0) yield break;

            yield return batch;
        }
    }

    public static IEnumerable<T> HBatch<T>() where T : IParsable<T> => Console.ReadLine().Split(' ').Select(x => T.Parse(x, null));
    public static IEnumerable<T[]> HBatches<T>() where T : IParsable<T> => StringBatch().Select(row => row.Split(' ').Select(x => T.Parse(x, null)).ToArray());

    public static IEnumerable<T> Batch<T>() where T : IParsable<T> => StringBatch().Select(x => T.Parse(x, null));

    public static IEnumerable<T[]> Batches<T>() where T : IParsable<T> => StringBatches().Select(x => x.Select(v => T.Parse(v, null)).ToArray());

    public static int Int() => Value<int>();

    public static bool TryInt(out int value) => Try(out value);

    /// <summary>
    /// 1000 /n 2000 /n 3000 /n /n
    /// </summary>
    public static IEnumerable<int> IntBatch() => Batch<int>();

    /// <summary>
    /// 1000 /n 2000 /n 3000 /n /n 7000 /n 8000 /n /n /n
    /// </summary>
    public static IEnumerable<int[]> IntBatches() => Batches<int>();

    public static long Long() => Value<long>();

    public static bool TryLong(out long value) => Try(out value);

    /// <summary>
    /// 1000 /n 2000 /n 3000 /n /n
    /// </summary>
    public static IEnumerable<long> LongBatch() => Batch<long>();

    /// <summary>
    /// 1000 /n 2000 /n 3000 /n /n 7000 /n 8000 /n /n /n
    /// </summary>
    public static IEnumerable<long[]> LongBatches() => Batches<long>();
}