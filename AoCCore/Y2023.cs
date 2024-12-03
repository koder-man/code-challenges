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
        var times = Read.Line().Substring("Time:    ".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var distances = Read.Line().Substring("Time:    ".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

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
        var time = long.Parse(Read.Line().Substring("Time:    ".Length).Replace(" ", string.Empty));
        var distance = long.Parse(Read.Line().Substring("Time:    ".Length).Replace(" ", string.Empty));

        var discriminant = Math.Sqrt(time * time - 4 * distance);

        var minimum = (long)((double)time / 2 - discriminant / 2) + 1;

        var ways = time - 2 * minimum + 1;

        Console.WriteLine(ways);
    }

    /*
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483

     */
    private enum Day07Type
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPairs,
        OnePair,
        Nothing,
    }

    public static void Day07A()
    {
        var hands = Read.StringBatch().Select(x => x.Split(' ')).Select(x => new Day07AHand(x[0], int.Parse(x[1]))).ToArray();

        var ordered = hands.OrderDescending(new Day07AHand.HandComparer()).ToArray();

        foreach (var hand in ordered)
            Console.WriteLine(hand);

        var result = ordered.Select((x, i) => (long)x.Bid * (i + 1)).ToArray();

        Console.WriteLine(result.Sum());
    }

    private struct Day07AHand
    {
        private static readonly (Day07Type Type, Regex Regex)[] Matching = new (Day07Type Type, Regex Regex)[]
        {
            (Day07Type.FiveOfAKind, new ("(?<C1>.)\\1{4,}")),
            (Day07Type.FourOfAKind,  new ("(?<C1>.)\\1{3,}")),
            (Day07Type.FullHouse,  new ("(?<C2>.)\\1{1,}(?<C1>.)\\2{2,}")),
            (Day07Type.FullHouse,  new ("(?<C1>.)\\1{2,}(?<C2>.)\\2{1,}")),
            (Day07Type.ThreeOfAKind,  new ("(?<C1>.)\\1{2,}")),
            (Day07Type.TwoPairs,  new ("(?<C1>.)\\1{1,}.?(?<C2>.)\\2{1,}")),
            (Day07Type.OnePair,  new ("(?<C1>.)\\1{1,}")),
            (Day07Type.Nothing,  new (".")),
        };

        public static Dictionary<char, int> Sorting = "A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, 2".Split(", ").Select((x, i) => (Char: x[0], i)).ToDictionary(x => x.Char, x => x.i);

        public Day07AHand(string cards, int bid)
        {
            var sorted = new string(cards.Order().ToArray());

            var match = Matching.Select(r => (r.Type, Match: r.Regex.Match(sorted))).First(m => m.Match.Success);

            Letters = cards;
            Bid = bid;
            Type = match.Type;
        }

        public static int GetSorting(char x) => Sorting[x];

        public string Letters { get; }

        public int Bid { get; }

        public Day07Type Type { get; }

        public override string ToString() => $"{Letters} {Bid} {Type}";

        public class HandComparer : IComparer<Day07AHand>
        {
            int IComparer<Day07AHand>.Compare(Day07AHand x, Day07AHand y)
            {
                var compare = x.Type.CompareTo(y.Type);
                if (compare != 0) return compare;

                var x1 = x.Letters.Select(GetSorting).ToArray();
                var y1 = y.Letters.Select(GetSorting).ToArray();

                compare = x.Letters.Select(GetSorting).Zip(y.Letters.Select(GetSorting)).Select(x => x.First.CompareTo(x.Second)).FirstOrDefault(x => x != 0);
                return compare;
            }
        }
    }

    public static void Day07B()
    {
        var hands = Read.StringBatch().Select(x => x.Split(' ')).Select(x => new Day07BHand(x[0], int.Parse(x[1]))).ToArray();

        var ordered = hands.OrderDescending(new Day07BHand.HandComparer()).ToArray();

        foreach (var hand in ordered)
            Console.WriteLine(hand);

        var result = ordered.Select((x, i) => (long)x.Bid * (i + 1)).ToArray();

        Console.WriteLine(result.Sum());
    }

    private struct Day07BHand
    {
        private static readonly (Day07Type Type, Regex Regex)[] Matching = new (Day07Type Type, Regex Regex)[]
        {
            (Day07Type.FiveOfAKind, new ("(?<C1>.)\\1{4,}")),
            (Day07Type.FourOfAKind,  new ("(?<C1>.)\\1{3,}")),
            (Day07Type.FullHouse,  new ("(?<C2>.)\\1{1,}(?<C1>.)\\2{2,}")),
            (Day07Type.FullHouse,  new ("(?<C1>.)\\1{2,}(?<C2>.)\\2{1,}")),
            (Day07Type.ThreeOfAKind,  new ("(?<C1>.)\\1{2,}")),
            (Day07Type.TwoPairs,  new ("(?<C1>.)\\1{1,}.?(?<C2>.)\\2{1,}")),
            (Day07Type.OnePair,  new ("(?<C1>.)\\1{1,}")),
            (Day07Type.Nothing,  new (".")),
        };

        public static Dictionary<char, int> Sorting = "A, K, Q, T, 9, 8, 7, 6, 5, 4, 3, 2, J, X".Split(", ").Select((x, i) => (Char: x[0], i)).ToDictionary(x => x.Char, x => x.i);

        public Day07BHand(string cards, int bid)
        {
            var sorted = new string(cards.Order().ToArray());

            var match = Matching.Select(r => (r.Type, Match: r.Regex.Match(sorted))).First(m => m.Match.Success);
            var type = match.Type;

            var cnt = cards.Count(x => x == 'J');
            switch (cnt)
            {
                case 0: break;
                case 1:
                    type = type switch
                    {
                        // Day07Type.FiveOfAKind => type,
                        Day07Type.FourOfAKind => Day07Type.FiveOfAKind,
                        // Day07Type.FullHouse => type,
                        Day07Type.ThreeOfAKind => Day07Type.FourOfAKind,
                        Day07Type.TwoPairs => Day07Type.FullHouse,
                        Day07Type.OnePair => Day07Type.ThreeOfAKind,
                        _ => Day07Type.OnePair,
                    };
                    break;
                case 2:
                    type = type switch
                    {
                        // Day07Type.FiveOfAKind => type,
                        // Day07Type.FourOfAKind => Day07Type.FiveOfAKind,
                        Day07Type.FullHouse => Day07Type.FiveOfAKind,
                        Day07Type.ThreeOfAKind => Day07Type.FiveOfAKind,
                        Day07Type.TwoPairs => Day07Type.FourOfAKind,
                        Day07Type.OnePair => Day07Type.ThreeOfAKind,
                        _ => throw new NotSupportedException(),
                    };
                    break;
                case 3:
                    type = type switch
                    {
                        // Day07Type.FiveOfAKind => type,
                        // Day07Type.FourOfAKind => Day07Type.FiveOfAKind,
                        Day07Type.FullHouse => Day07Type.FiveOfAKind,
                        Day07Type.ThreeOfAKind => Day07Type.FourOfAKind,
                        // Day07Type.TwoPairs => Day07Type.FourOfAKind,
                        // Day07Type.OnePair => Day07Type.FourOfAKind,
                        _ => throw new NotSupportedException(),
                    };
                    break;
                case 4:
                    type = type switch
                    {
                        // Day07Type.FiveOfAKind => type,
                        Day07Type.FourOfAKind => Day07Type.FiveOfAKind,
                        // Day07Type.FullHouse => Day07Type.FiveOfAKind,
                        // Day07Type.ThreeOfAKind => Day07Type.FourOfAKind,
                        // Day07Type.TwoPairs => Day07Type.FourOfAKind,
                        // Day07Type.OnePair => Day07Type.FourOfAKind,
                        _ => throw new NotSupportedException(),
                    };
                    break;
                case 5:
                    type = type switch
                    {
                        Day07Type.FiveOfAKind => type,
                        // Day07Type.FourOfAKind => Day07Type.FiveOfAKind,
                        // Day07Type.FullHouse => Day07Type.FiveOfAKind,
                        // Day07Type.ThreeOfAKind => Day07Type.FourOfAKind,
                        // Day07Type.TwoPairs => Day07Type.FourOfAKind,
                        // Day07Type.OnePair => Day07Type.FourOfAKind,
                        _ => throw new NotSupportedException(),
                    };
                    break;
            }

            Letters = cards;
            Bid = bid;
            Type = type;
        }

        public static int GetSorting(char x) => Sorting[x];

        public string Letters { get; }

        public int Bid { get; }

        public Day07Type Type { get; }

        public override string ToString() => $"{Letters} {Bid} {Type}";

        public class HandComparer : IComparer<Day07BHand>
        {
            int IComparer<Day07BHand>.Compare(Day07BHand x, Day07BHand y)
            {
                var compare = x.Type.CompareTo(y.Type);
                if (compare != 0) return compare;

                var x1 = x.Letters.Select(GetSorting).ToArray();
                var y1 = y.Letters.Select(GetSorting).ToArray();

                compare = x.Letters.Select(GetSorting).Zip(y.Letters.Select(GetSorting)).Select(x => x.First.CompareTo(x.Second)).FirstOrDefault(x => x != 0);
                return compare;
            }
        }
    }

    public static void Day08A()
    {
        var instructions = Read.Line();
        Read.Line();

        var lines = Read.StringBatch()
            .Select(x => (x.Substring(0, 3), Left: x.Substring(7, 3), Right: x.Substring(12, 3)))
            .ToDictionary(x => x.Item1, x => (x.Left, x.Right));

        var current = "AAA";
        var steps = 0;
        while (true)
        {
            foreach (var instruction in instructions)
            {
                steps++;
                current = instruction == 'L' ? lines[current].Left : lines[current].Right;

                if (current == "ZZZ")
                    break;
            }

            if (current == "ZZZ")
                break;
        }

        Console.WriteLine(steps);
    }

    public static void Day08B()
    {
        var instructions = Read.Line();
        Read.Line();

        var lines = Read.StringBatch()
            .Select(x => (x.Substring(0, 3), Left: x.Substring(7, 3), Right: x.Substring(12, 3)))
            .ToDictionary(x => x.Item1, x => (x.Left, x.Right));

        var current = lines.Keys.Where(x => x[2] == 'A').ToArray();
        var steps = current.Select(x => Process(x, lines, instructions)).ToArray();

        var result = Calc.LCM(steps);

        Console.WriteLine(result);

        static long Process(string current, Dictionary<string, (string Left, string Right)> lines, string instructions)
        {
            var steps = 0;
            while (true)
            {
                foreach (var instruction in instructions)
                {
                    steps++;

                    current = instruction == 'L' ? lines[current].Left : lines[current].Right;
                    if (current[2] == 'Z')
                        return steps;
                }
            }
        }
    }

    /*
    0 3 6 9 12 15
    1 3 6 10 15 21
    10 13 16 21 30 45

    */
    public static void Day09A()
    {
        var sums = new List<long>();

        foreach (var batch in Read.HBatches<int>())
        {
            var stack = new List<int[]>() { batch };

            while (true)
            {
                var current = stack.Last();
                var diffs = current.Skip(1).Select((x, i) => x - current[i]).ToArray();

                if (diffs.All(x => x == 0))
                {
                    sums.Add(stack.Select(x => x.Last()).Sum());
                    break;
                }

                stack.Add(diffs);
            }
        }

        foreach (var sum in sums)
            Console.WriteLine(sum);

        Console.WriteLine(sums.Sum());
    }

    public static void Day09B()
    {
        var sums = new List<long>();

        foreach (var batch in Read.HBatches<int>())
        {
            var stack = new List<int[]>() { batch };

            while (true)
            {
                var current = stack.Last();
                var diffs = current.Skip(1).Select((x, i) => x - current[i]).ToArray();

                if (diffs.All(x => x == 0))
                {
                    sums.Add(stack.Select((x, i) => i % 2 == 0 ? x.First() : -x.First()).Sum());
                    break;
                }

                stack.Add(diffs);
            }
        }

        foreach (var sum in sums)
            Console.WriteLine(sum);

        Console.WriteLine(sums.Sum());
    }

    /*
7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ

6

| is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.
. is ground; there is no pipe in this tile.
S is the starting position of the animal;  */
    public static void Day10A()
    {
        var input = Read.StringBatch().ToArray();

        var current1 = input.Select((x, i) => new Day10Point(i, x.IndexOf('S'))).First(p => p.J >= 0);
        var current2 = current1;

        // 8 4 6 2
        Console.WriteLine("Give me hint!");
        Move(ref current1, Read.Int(), ref input);
        Move(ref current2, Read.Int(), ref input);
        var steps = 1;

        while (true)
        {
            var direction = GetDirection(current1, input);
            Move(ref current1, direction, ref input);

            direction = GetDirection(current2, input);
            Move(ref current2, direction, ref input);

            steps++;

            if (current1 == current2) break;
        }

        Console.WriteLine(steps);

        static void Move(ref Day10Point current, int direction, ref string[] input)
        {
            var line = input[current.I]; // 5  2         01   *  34
            input[current.I] = line.Substring(0, current.J) + "*" + line.Substring(current.J + 1, line.Length - current.J - 1);

            current = new Day10Point(
                current.I + direction switch { 8 => -1, 2 => 1, _ => 0, },
                current.J + direction switch { 4 => -1, 6 => 1, _ => 0, });

        }

        static int GetDirection(Day10Point current, string[] input)
        {
            var now = input[current.I][current.J];

            int up() => input[current.I - 1][current.J];
            //// int down() => input[current.I + 1][current.J];
            int left() => input[current.I][current.J - 1];
            int right() => input[current.I][current.J + 1];

            return now switch
            {
                '|' => up() != '*' ? 8 : 2,
                '-' => left() != '*' ? 4 : 6,
                'L' => up() != '*' ? 8 : 6,
                'J' => up() != '*' ? 8 : 4,
                '7' => left() != '*' ? 4 : 2,
                'F' => right() != '*' ? 6 : 2,
                _ => throw new Exception(),
            };
        }
    }

    public static void Day10B()
    {
        var input = Read.StringBatch().ToArray();

        var start = input.Select((x, i) => new Day10Point(i, x.IndexOf('S'))).First(p => p.J >= 0);
        var steps = new List<Day10Point>() { start, };

        var original = input.Select((line, i) => line.Select((c, j) =>
            new Day10BTrack(new Day10Point(i, j), c, c == '.' ? Day10BStatus.Unknown : Day10BStatus.OtherLine, null)).ToArray()).ToArray();
        original[start.I][start.J] = original[start.I][start.J] with { Char = (char)Console.Read(), };

        // 8 4 6 2
        int direction = Read.Int();
        var current = start;
        var prev = current;
        current = Move(current, direction, ref original);
        steps.Add(current);

        while (true)
        {
            direction = GetDirection(current, original, direction);
            current = Move(current, direction, ref original);

            /*Console.SetCursorPosition(0, 0);
            foreach (var item in original)
                Console.WriteLine(item.Select(x => x.Status == Status.Line ? '*' : x.Char).ToArray());
            */
            if (current == start) break;

            steps.Add(current);
        }

        var totals = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (original[i][j].Status == Day10BStatus.Line) continue;

                var borders = original[i].Take(j).Aggregate((Count: 0, Up: (bool?)null), Process);

                if (borders.Up.HasValue) throw new Exception();

                if (borders.Count % 1 != 0) throw new Exception();

                totals += borders.Count % 2;

                static (int, bool?) Process((int Count, bool? Up) state, Day10BTrack current)
                {
                    return current.Status == Day10BStatus.Line
                        ? state.Up switch
                        {
                            null => current.Char switch
                            {
                                'F' => (state.Count, true),
                                'L' => (state.Count, false),
                                '|' => (state.Count + 1, null),
                                _ => throw new Exception(),
                            },
                            true => current.Char switch
                            {
                                'J' => (state.Count + 1, null),
                                '7' => (state.Count, null),
                                '-' => state,
                                _ => throw new Exception(),
                            },
                            false => current.Char switch
                            {
                                'J' => (state.Count, null),
                                '7' => (state.Count + 1, null),
                                '-' => state,
                                _ => throw new Exception(),
                            },
                        }
                        : state;
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(totals);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        foreach (var item in original)
            Console.WriteLine(item.Select(x => x.Status == Day10BStatus.Line ? '*' : x.Char).ToArray());

        static Day10Point Move(Day10Point current, int direction, ref Day10BTrack[][] input)
        {
            var track = input[current.I][current.J];
            if (track.Status != Day10BStatus.Unknown && track.Status != Day10BStatus.OtherLine) throw new Exception();

            var next = new Day10Point(
                current.I + direction switch { 8 => -1, 2 => 1, _ => 0, },
                current.J + direction switch { 4 => -1, 6 => 1, _ => 0, });

            input[current.I][current.J] = track with { Status = Day10BStatus.Line, Next = (current, direction), };
            return next;
        }

        static int GetDirection(Day10Point current, Day10BTrack[][] input, int from)
        {
            var now = input[current.I][current.J].Char;

            bool up() => from == 2;
            bool down() => from == 8;
            bool left() => from == 6;
            bool right() => from == 4;

            return now switch
            {
                '|' => down() ? 8 : 2,
                '-' => left() ? 6 : 4,
                'L' => up() ? 6 : 8,
                'J' => up() ? 4 : 8,
                '7' => left() ? 2 : 4,
                'F' => right() ? 2 : 6,
                _ => throw new Exception(),
            };
        }
    }

    enum Day10BStatus
    {
        OtherLine,
        Line,
        Unknown,
    }
    private readonly record struct Day10BTrack(Day10Point Point, char Char, Day10BStatus Status, (Day10Point Point, int Direction)? Next);

    public readonly record struct Day10Point(int I, int J);

    public static void Day11A()
    {
        var input = Read.StringBatch().ToList();

        for (int i = input.Count - 1; i >= 0; i--)
        {
            if (input[i].All(x => x == '.'))
                input.Insert(i, input[i]);
        }

        var width = input[0].Length;
        for (int j = width - 1; j >= 0; j--)
        {
            if (input.All(line => line[j] == '.'))
            {
                input = input.Select(line => line.Substring(0, j) + "." + line.Substring(j)).ToList();
            }
        }

        var galaxies = input.SelectIndex()
            .SelectMany(row => row.Value.SelectIndex().Select(c => (I: row.Index, J: c.Index, Char: c.Value)))
            .Where(x => x.Char == '#')
            .Select(x => (x.I, x.J))
            .ToArray();

        var total = 0;
        foreach (var start in galaxies)
        {
            foreach (var end in galaxies)
            {
                var distance = Math.Abs(start.I - end.I) + Math.Abs(start.J - end.J);
                total += distance;
            }
        }

        Console.WriteLine(total / 2);
    }

    public static void Day11B()
    {
        var input = Read.StringBatch().ToList();

        var rows = input.SelectIndex().Where(x => x.Value.All(x => x == '.')).Select(x => x.Index).ToArray();
        var cols = Enumerable.Range(0, input[0].Length).Where(j => input.All(line => line[j] == '.')).ToArray();

        var galaxies = input.SelectIndex()
            .SelectMany(row => row.Value.SelectIndex().Select(c => (I: row.Index, J: c.Index, Char: c.Value)))
            .Where(x => x.Char == '#')
            .Select(x => (I: Expand(x.I, rows), J: Expand(x.J, cols)))
            .ToArray();

        var total = 0L;
        foreach (var start in galaxies)
        {
            foreach (var end in galaxies)
            {
                var distance = Math.Abs(start.I - end.I) + Math.Abs(start.J - end.J);
                total += distance;
            }
        }

        Console.WriteLine(total / 2);

        static long Expand(int i, int[] indexes) => i + (1000000 - 1) * indexes.Count(x => x < i);
    }

    public static void Current() // Day12A()
    {
        var text = Read.Line();
        Read.Line();

        var sum1 = Read.LongBatch().Sum();
        Console.WriteLine(sum1);

        var all = new List<long>();
        foreach (var batch in Read.LongBatches())
        {
            var sum = batch.Sum();

            all.Add(sum);
        }
        Console.WriteLine(all.OrderDescending().Take(3).Sum());

        var input = Read.StringBatch().ToArray();

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

    public static void CurrentSample() // Day11A()
    {
        var text = Read.Line();
        Read.Line();

        var sum1 = Read.LongBatch().Sum();
        Console.WriteLine(sum1);

        var all = new List<long>();
        foreach (var batch in Read.LongBatches())
        {
            var sum = batch.Sum();

            all.Add(sum);
        }
        Console.WriteLine(all.OrderDescending().Take(3).Sum());

        var input = Read.StringBatch().ToArray();

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