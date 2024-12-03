using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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

    public static void P<T>(this T error)
    {
        Console.WriteLine(error);
    }

    public static void P<T>(this T value, [CallerArgumentExpression(nameof(value))] string? message = null)
    {
        Console.WriteLine($"{message}: {value}");
    }

    public static void P<T>(this IEnumerable<T> values)
    {
        foreach (T value in values)
            value.P();
    }

    public static void P<T>(this IEnumerable<T> values, string message)
    {
        Console.WriteLine($"{message}:");
        foreach (T value in values)
            value.P();
    }
}