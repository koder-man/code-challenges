namespace AoCCore;

public static class Y2023
{
    public static void Day01A()
    {
        /*
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet

         */
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
        /*
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen

         */
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

    /*
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green

     */
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

    /*
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11

     */
    public static void Day04A()
    {
        double total = 0;
        foreach (var line in Read.StringBatch().SelectIndex())
        {
            var parts = line.Value.Split(':')[1].Split('|');

            var winning = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            var mine = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet()
                .Where(winning.Contains)
                .Count();

            if (mine > 0)
            {
                total += Math.Pow(2, mine - 1);
            }
        }

        Console.WriteLine(total);
    }

    public static void Day04B()
    {
        var cards = Read.StringBatch().ToArray();
        var copies = Enumerable.Repeat(1, cards.Length).ToArray();

        for (int i = 0; i < cards.Length; i++)
        {
            var parts = cards[i].Split(':')[1].Split('|');

            var winning = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            var mine = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet()
                .Where(winning.Contains)
                .Count();

            for (int j = 1; j <= mine; j++)
            {
                if (j + i == cards.Length) break;

                copies[j + i] += copies[i];
            }
        }

        Console.WriteLine(copies.Sum());
    }

    /*
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4

     */
    public static void Day05A()
    {
        var input = Read.StringBatches().ToArray();

        var seeds = input[0][0].Substring("seeds: ".Length).Split(' ').Select(long.Parse).ToArray();

        var maps = input.Skip(1).Select(paragraph => new Day05AMap(paragraph.Skip(1).Select(row => Parse(row.Split(' ').Select(long.Parse).ToArray())).ToArray())).ToArray();

        var locations = seeds.Select(seed => maps.Aggregate(seed, (input, map) => map.Transform(input))).ToArray();

        Console.WriteLine(locations.Min());

        static Day05AMapping Parse(long[] row) => new Day05AMapping(row[1], row[0], row[2]);
    }

    private record class Day05AMap(IReadOnlyCollection<Day05AMapping> Mappings)
    {
        public long Transform(long input)
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.TryTransform(ref input))
                    break;
            }

            return input;
        }
    }

    private record class Day05AMapping(long FromStart, long ToStart, long Range)
    {
        public bool TryTransform(ref long input)
        {
            if (FromStart <= input && FromStart + Range > input)
            {
                input = ToStart + input - FromStart;
                return true;
            }

            return false;
        }
    }

    public static void Day05B()
    {
        var input = Read.StringBatches().ToArray();

        var seedRanges = input[0][0].Substring("seeds: ".Length).Split(' ').Select(long.Parse).Chunk(2)
            .Select(seed => new Day05BRange(seed[0], seed[0] + seed[1]))
            .ToArray();

        var maps = input.Skip(1).Select(paragraph => new Day05BMap(paragraph[0], paragraph.Skip(1).Select((row, i) => Parse(i, row.Split(' ').Select(long.Parse).ToArray())).ToArray())).ToArray();

        var seedEndRanges = seedRanges
            .Select(
                seedRange => maps.Aggregate(
                    new[] { seedRange, }, (ranges, map) => map.Transform(ranges)))
            .ToArray();

        var locations = seedEndRanges.Select(ranges => ranges.Min(r => r.Start)).ToArray();

        Console.WriteLine(locations.Min());

        static Day05BMapping Parse(int Index, long[] row) => new Day05BMapping(Index, row[1], row[0] - row[1], row[2]);
    }

    private record class Day05BMap(string Mapping, IReadOnlyCollection<Day05BMapping> Mappings)
    {
        public Day05BRange[] Transform(IEnumerable<Day05BRange> input)
        {
            return input.SelectMany(Transform).ToArray();
        }

        public Day05BRange[] Transform(Day05BRange input)
        {
            var transformed = new List<Day05BRange>();

            var toDo = new List<Day05BRange>() { input, };

            foreach (var map in Mappings)
            {
                var nextTodo = new List<Day05BRange>();
                foreach (var range in toDo)
                {
                    map.Transform(range, ref nextTodo, ref transformed);
                }

                toDo = nextTodo;
            }

            return toDo.Union(transformed).ToArray();
        }
    }

    private record class Day05BMapping(int Index, long FromStart, long Offset, long Range) // 2 0 1
    {
        public long FromEnd => FromStart + Range; // 3

        public void Transform(Day05BRange input, ref List<Day05BRange> toDo, ref List<Day05BRange> transformed) // 1 4
        {
            if (input.Start < FromStart)
            {
                var preEnd = Math.Min(input.End, FromStart); // 2 = 4 2

                toDo.Add(new Day05BRange(input.Start, preEnd));
            }

            var transformStart = Math.Max(input.Start, FromStart); // 2 = 1 2
            var transformEnd = Math.Min(input.End, FromEnd); // 3 = 4 3

            if (transformStart < transformEnd)
                transformed.Add(new Day05BRange(transformStart + Offset, transformEnd + Offset));

            if (input.End > FromEnd)
            {
                var rightStart = Math.Max(input.Start, FromEnd); // 3 = 1 3

                toDo.Add(new Day05BRange(rightStart, input.End));
            }
        }
    }

    private record struct Day05BRange(long Start, long End);

    public static void Day06A()
    {
        var times = Console.ReadLine().Substring("Time:    ".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var distances = Console.ReadLine().Substring("Time:    ".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        var ways = 1L;

        for (int i = 0; i < times.Length; i++)
        {
            var time = times[i];
            var discriminant = Math.Sqrt(time * time - 4 * distances[i]);

            var minimum = (int)((double)time / 2 - discriminant / 2) + 1;

            ways *= time - 2 * minimum + 1;
        }

        Console.WriteLine(ways);
    }

    public static void Day06B()
    {
        var time = long.Parse(Console.ReadLine().Substring("Time:    ".Length).Replace(" ", string.Empty));
        var distance = long.Parse(Console.ReadLine().Substring("Time:    ".Length).Replace(" ", string.Empty));

        var discriminant = Math.Sqrt(time * time - 4 * distance);

        var minimum = (long)((double)time / 2 - discriminant / 2) + 1;

        var ways = time - 2 * minimum + 1;

        Console.WriteLine(ways);
    }

    public static void Current() // Day07()
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

    public static void CurrentSample() // Day08()
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