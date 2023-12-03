namespace AoCCore;

public static class Y2023
{
    public static void Day01A()
    {
        var total = 0L;

        foreach (var line in Read.StringBatch())
        {
            var all = line.Where(char.IsDigit).ToArray();
            var value = int.Parse(new string(new[] { all[0], all[^1], }));
            total += value;
        }

        Console.WriteLine(total);
    }

    private static readonly List<string> Day01BValid = new() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", };
    public static void Day01B()
    {
        var total = 0L;

        foreach (var line in Read.StringBatch())
        {
            int first = -1;
            for (var i = 0; i < line.Length; i++)
            {
                if (TryParse(line[i], line[i..], (a, b) => a.StartsWith(b), out first))
                    break;
            }

            int last = default;
            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (TryParse(line[i], line[..(i + 1)], (a, b) => a.EndsWith(b), out last))
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

            foreach (var number in Day01BValid)
            {
                if (comparer(sub, number))
                {
                    result = Day01BValid.IndexOf(number) + 1;
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
            int index = int.Parse(parts[0][5..]);

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
                    var (count, color) = Parse(group);
                    if (count > limits[color])
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
            int index = int.Parse(parts[0][5..]);

            total += ProductOfMins(parts[1]);
        }

        Console.WriteLine(total);

        static int ProductOfMins(string sets)
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
                    var (count, color) = Parse(group);
                    if (count > maxes[color])
                    {
                        maxes[color] = count;
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

    /*
0123456789

467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..

     */
    public static void Day03A()
    {
        var r = new Regex("^(\\d+)");
        var lines = Read.StringBatch().ToArray();
        var maxH = lines.Length;
        var maxV = lines[0].Length;

        int total = 0;

        for (int i = 0; i < maxH; i++)
        {
            for (int j = 0; j < maxV; j++)
            {
                var line = lines[i].Substring(j);
                var match = r.Match(line);
                if (match.Success)
                {
                    var str = match.Groups[1].Value;
                    if (NearSymbol(i, j, str.Length))
                    {
                        var number = int.Parse(str);
                        total += number;
                    }
                    j += str.Length;
                }
            }
        }
        Console.WriteLine(total);

        bool NearSymbol(int i, int j, int len)
        {
            var startI = Math.Max(0, j - 1);
            var endI = Math.Min(maxV, j + len + 1);
            var endLen = endI - startI;
            if (i > 0)
            {
                if (ContainsSymbol(lines[i - 1].Substring(startI, endLen)))
                    return true;
            }
            if (i < maxH - 1)
            {
                if (ContainsSymbol(lines[i + 1].Substring(startI, endLen)))
                    return true;
            }
            if (j > 0)
            {
                if (IsSymbol(lines[i][j - 1]))
                    return true;
            }
            if (j + len < maxV - 1)
            {
                if (IsSymbol(lines[i][j + len]))
                    return true;
            }
            return false;

            static bool ContainsSymbol(string text) => text.Any(IsSymbol);
            static bool IsSymbol(char c) => !char.IsDigit(c) && c != '.';
        }
    }
    public static void Day03B()
    {
        var r = new Regex("^(\\d+)");
        var lines = Read.StringBatch().ToArray();
        var maxH = lines.Length;
        var maxV = lines[0].Length;

        var gears = new Dictionary<Day03Point, List<int>>();

        for (int i = 0; i < maxH; i++)
        {
            for (int j = 0; j < maxV; j++)
            {
                var line = lines[i].Substring(j);
                var match = r.Match(line);
                if (match.Success)
                {
                    var str = match.Groups[1].Value;
                    var point = NearGear(i, j, str.Length);
                    if (point != default)
                    {
                        var number = int.Parse(str);
                        if (gears.TryGetValue(point, out var nears))
                        {
                            nears.Add(number);
                        }
                        else
                        {
                            gears.Add(point, new List<int>() { number, });
                        }
                    }
                    j += str.Length;
                }
            }
        }
        var total = gears.Values.Where(x => x.Count > 1).Select(x => x[0] * x[1]).Sum();
        Console.WriteLine(total);

        Day03Point NearGear(int i, int j, int len)
        {
            var startJ = Math.Max(0, j - 1);
            var endJ = Math.Min(maxV, j + len + 1);
            var endLen = endJ - startJ;
            if (i > 0)
            {
                var index = lines[i - 1].Substring(startJ, endLen).IndexOf('*');
                if (index >= 0)
                    return new Day03Point(i - 1, startJ + index);
            }
            if (i < maxH - 1)
            {
                var index = lines[i + 1].Substring(startJ, endLen).IndexOf('*');
                if (index >= 0)
                    return new Day03Point(i + 1, startJ + index);
            }
            if (j > 0)
            {
                if (IsGear(lines[i][j - 1]))
                    return new(i, j - 1);
            }
            if (j + len < maxV - 1)
            {
                if (IsGear(lines[i][j + len]))
                    return new(i, j + len);
            }
            return default;

            static bool IsGear(char c) => c == '*';
        }
    }
    private record struct Day03Point(int X, int Y);

    public static void Current() // Day04A()
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

    public static void CurrentSample() // Day05A()
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