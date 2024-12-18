﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AoCCore;

public static class Extensions
{
    public static bool DebugOn = false;

    public static T DebugLine<T>(this T value)
    {
        if (DebugOn) Console.WriteLine(value);
        return value;
    }

    public static T Debug<T>(this T value)
    {
        if (DebugOn) Console.Write(value);
        return value;
    }

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

    public static T As<T>(this string input) where T : IParsable<T> => T.Parse(input, null);

    public static int Int(this string input) => input.As<int>();

    public static long Long(this string input) => input.As<long>();

    public static string Join(this IEnumerable<string> input, string separator = ",") => string.Join(separator, input);

    public static MatchCollection Matches(this string input, [StringSyntax("Regex")] string pattern)
    {
        return new Regex(pattern).Matches(input);
    }
}