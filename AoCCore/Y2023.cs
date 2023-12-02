using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Markup;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AoCCore;

public static class Y2023
{
    private static List<string> valid = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", };

    public static void Day01A()
    {
        var total = 0L;

        foreach (var line in Read.StringBatch())
        {
            var all = line.Where(char.IsDigit).ToArray();
            var value = int.Parse(new string(new[] { all[0], all[all.Length - 1], }));
            total += value;
        }

        Console.WriteLine(total);
    }

    public static void Day01B()
    {

        var total = 0L;

        foreach (var line in Read.StringBatch())
        {
            int first = -1;
            for (var i = 0; i < line.Length; i++)
            {
                if (TryParse(line[i], line.Substring(i), (a, b) => a.StartsWith(b), out first))
                    break;
            }

            int last = default;
            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (TryParse(line[i], line.Substring(0, i + 1), (a, b) => a.EndsWith(b), out last))
                    break;
            }

            var value = first * 10 + last;
            total += value;
        }

        Console.WriteLine(total);

        static bool TryParse(char @char, string sub, Func<string, string, bool> comparer, out int result)
        {
            if (char.IsDigit(@char))
            {
                result = int.Parse(@char.ToString());
                return true;
            }

            foreach (var number in valid)
            {
                if (comparer(sub, number))
                {
                    result = valid.IndexOf(number) + 1;
                    return true;
                }
            }

            result = -1;
            return false;
        }
    }

    enum Day02Color { blue, red, green }
    public static void Day02A()
    {
        // only 12 red cubes, 13 green cubes, and 14 blue cubes
        var limits = new Dictionary<Day02Color, int>()
        {
            { Day02Color.blue, 14 },
            { Day02Color.red, 12 },
            { Day02Color.green, 13 },
        };
        int total = 0;
        /*Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green*/
        foreach (var line in Read.StringBatch())
        {
            var parts = line.Split(':');
            int index = int.Parse(parts[0].Substring(5));

            if (!IsOver(parts[1]))
            {
                total += index;
            }
        }

        Console.WriteLine(total);

        bool IsOver(string sets)
        {
            var subsets = sets.Split(';');
            foreach (var subset in subsets)
            {
                var groups = subset.Split(',');
                foreach (var group in groups)
                {
                    var parse = Parse(group);
                    if (parse.Count > limits[parse.Color])
                    {
                        return true;
                    }
                }

                static (int Count, Day02Color Color) Parse(string input)
                {
                    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    return (int.Parse(parts[0]), Enum.Parse<Day02Color>(parts[1]));
                }
            }

            return false;
        }
    }

    public static void Day02B()
    {
        int total = 0;
        /*Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green*/
        foreach (var line in Read.StringBatch())
        {
            var parts = line.Split(':');
            int index = int.Parse(parts[0].Substring(5));

            total += ProductOfMins(parts[1]);
        }

        Console.WriteLine(total);

        int ProductOfMins(string sets)
        {
            var maxes = new Dictionary<Day02Color, int>()
            {
                { Day02Color.blue, 0 },
                { Day02Color.red, 0 },
                { Day02Color.green, 0 },
            };

            var subsets = sets.Split(';');
            foreach (var subset in subsets)
            {
                var groups = subset.Split(',');
                foreach (var group in groups)
                {
                    var parse = Parse(group);
                    if (parse.Count > maxes[parse.Color])
                    {
                        maxes[parse.Color] = parse.Count;
                    }
                }

                static (int Count, Day02Color Color) Parse(string input)
                {
                    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    return (int.Parse(parts[0]), Enum.Parse<Day02Color>(parts[1]));
                }
            }

            return maxes.Values.Aggregate(1, (x, y) => x * y);
        }
    }

    public static void Current() // Day03A()
    {
        var max = 0L;

        var all = new List<long>();
        foreach (var batch in Read.LongBatches())
        {
            var sum = batch.Sum();

            all.Add(sum);
        }
        Console.WriteLine(all.OrderDescending().Take(3).Sum());

        var lines = new List<string>();
        foreach (var line in Read.StringBatch())
        {
            if (char.IsDigit(line[1])) break;

            lines.Add(line);
        }

        foreach (var batch in Read.StringBatches())
        {
            lines.Add(batch[0]);
        }
        Console.WriteLine(new string(lines.Select(Enumerable.First).ToArray()));
    }

    public static void CurrentSample() // Day04A()
    {
        var sum1 = Read.LongBatch().Sum();
        Console.WriteLine(sum1);

        var all = new List<long>();
        foreach (var batch in Read.LongBatches())
        {
            var sum = batch.Sum();

            all.Add(sum);
        }
        Console.WriteLine(all.OrderDescending().Take(3).Sum());

        var lines = new List<string>();
        foreach (var line in Read.StringBatch())
        {
            if (char.IsDigit(line[1])) break;

            lines.Add(line);
        }

        foreach (var batch in Read.StringBatches())
        {
            lines.Add(batch[0]);
        }
        Console.WriteLine(new string(lines.Select(Enumerable.First).ToArray()));
    }
}