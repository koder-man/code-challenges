using System.Diagnostics.CodeAnalysis;

namespace AoCCore;

internal static class Read
{
    /// <inheritdoc cref="Console.ReadLine"/>
    public static string Line()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    /// <summary>
    /// Parses line as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <returns>Parsed value.</returns>
    public static T Value<T>()
        where T : IParsable<T>
    {
        return T.Parse(Line(), null);
    }

    /// <summary>
    /// Tryies to parse line as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <returns>Parsed value.</returns>
    public static bool Try<T>([NotNullWhen(true)] out T? result)
        where T : IParsable<T>
    {
        return T.TryParse(Line(), null, out result);
    }

    /// <summary>
    /// <code>
    /// Words words
    /// Words
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <returns>Enumeration of lines.</returns>
    public static IEnumerable<string> StringBatch()
    {
        string line;
        while ((line = Line()).Length > 0)
        {
            yield return line;
        }
    }

    /// <summary>
    /// <code>
    /// Words words
    /// Words words
    /// 
    /// Words
    /// 
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <returns>Enumration of line batches.</returns>
    public static IEnumerable<string[]> StringBatches()
    {
        while (true)
        {
            var batch = StringBatch().ToArray();

            if (batch.Length == 0) yield break;

            yield return batch;
        }
    }

    /// <summary>
    /// <code>
    /// [<typeparamref name="T"/>1] [<typeparamref name="T"/>2]
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <param name="separator">Values separator. Default: ' '</param>
    /// <returns>Enumeration of values in line.</returns>
    public static IEnumerable<T> HBatch<T>(char separator = ' ') where T : IParsable<T> => Line().Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(x => T.Parse(x, null));

    /// <summary>
    /// <code>
    /// [<typeparamref name="T"/>1] [<typeparamref name="T"/>2]
    /// [<typeparamref name="T"/>3]
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <param name="separator">Values separator. Default: ' '</param>
    /// <returns>Enumeration of value batches.</returns>
    public static IEnumerable<T[]> HBatches<T>(char separator = ' ') where T : IParsable<T> => StringBatch().Select(row => row.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(x => T.Parse(x, null)).ToArray());

    /// <summary>
    /// <code>
    /// [<typeparamref name="T"/>1]
    /// [<typeparamref name="T"/>2]
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <returns>Enumeration of values.</returns>
    public static IEnumerable<T> Batch<T>() where T : IParsable<T> => StringBatch().Select(x => T.Parse(x, null));

    /// <summary>
    /// <code>
    /// [<typeparamref name="T"/>1]
    /// 
    /// [<typeparamref name="T"/>2]
    /// [<typeparamref name="T"/>3]
    /// 
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <returns>Enumeration of value batches.</returns>
    public static IEnumerable<T[]> Batches<T>() where T : IParsable<T> => StringBatches().Select(x => x.Select(v => T.Parse(v, null)).ToArray());

    /// <summary>
    /// <code>
    /// 1000
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static int Int() => Value<int>();

    /// <summary>
    /// <code>
    /// 1000
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static bool TryInt(out int value) => Try(out value);

    /// <summary>
    /// <code>
    /// 1000
    /// 2000
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static IEnumerable<int> IntBatch() => Batch<int>();

    /// <summary>
    /// <code>
    /// 1000
    /// 
    /// 1000
    /// 2000
    /// 
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static IEnumerable<int[]> IntBatches() => Batches<int>();

    /// <summary>
    /// <code>
    /// 1000
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static long Long() => Value<long>();

    /// <summary>
    /// <code>
    /// 1000
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static bool TryLong(out long value) => Try(out value);

    /// <summary>
    /// <code>
    /// 1000
    /// 2000
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static IEnumerable<long> LongBatch() => Batch<long>();

    /// <summary>
    /// <code>
    /// 1000
    /// 
    /// 1000
    /// 2000
    /// 
    /// 
    /// .<c>EOF</c>
    /// </code>
    /// </summary>
    public static IEnumerable<long[]> LongBatches() => Batches<long>();
}