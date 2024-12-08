using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AoCCore;

public static class Y2024
{
    /*
3   4
4   3
2   5
1   3
3   9
3   3
    */
    public static void Day01A()
    {
        var ints = Read.HBatches<int>().ToArray();

        var left = ints.Select(x => x[0]).Order().ToArray();
        var right = ints.Select(x => x[1]).Order().ToArray();

        left.Zip(right).Select(x => Math.Abs(x.First - x.Second)).Sum().P("sum");
    }

    public static void Day01B()
    {
        var ints = Read.HBatches<int>().ToArray();

        var right = ints.Select(x => x[1]).GroupBy(x => x, (key, values) => (key, values.Count())).ToDictionary(x => x.key, x => x.Item2);

        var lefts = new Dictionary<int, int>();

        ints.Select(x => right.TryGetValue(x[0], out var score) ? (long)x[0] * score : 0).Sum().P("sum");
    }

    /*
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
    */
    public static void Day02A()
    {
        var ints = Read.HBatches<int>().ToArray();

        ints.Sum(IsSafe).P("Safe");

        static int IsSafe(int[] report)
        {
            bool isIncreasing = report[1] > report[0];

            for (int i = 1; i < report.Length; i++)
            {
                if (report[i] == report[i - 1])
                {
                    return 0;
                }

                if (isIncreasing ^ report[i] > report[i - 1])
                {
                    return 0;
                }

                if (Math.Abs(report[i] - report[i - 1]) > 3)
                {
                    return 0;
                }
            }

            return 1;
        }
    }

    public static void Day02B()
    {
        var ints = Read.HBatches<int>().ToArray();

        int safes = 0;

        foreach (var report in ints)
        {
            if (IsSafe(report))
            {
                safes++;
                continue;
            }

            for (var i = 0; i < report.Length; i++)
            {
                if (IsSafe(report.Take(i).Concat(report.Skip(i + 1)).ToArray()))
                {
                    safes++;
                    break;
                }
            }
        }

        safes.P("Safe");

        static bool IsSafe(int[] report)
        {
            bool isIncreasing = report[1] > report[0];

            for (int i = 1; i < report.Length; i++)
            {
                if (report[i] == report[i - 1])
                {
                    return false;
                }

                if (isIncreasing ^ report[i] > report[i - 1])
                {
                    return false;
                }

                if (Math.Abs(report[i] - report[i - 1]) > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }

    /*
xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))

    */
    public static void Day03A()
    {
        Read.StringBatch()
            .Join()
            .Matches(@"mul\((\d+),(\d+)\)")
            .Sum(m => m.Groups[1].Value.Long() * m.Groups[2].Value.Int())
            .P("Sum");

        // 179834255
    }

    /*
xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))

    */
    public static void Day03B()
    {
        Read.StringBatch()
            .Join()
            .Matches(@"mul\((\d+),(\d+)\)|(do\(\))|(don't\(\))")
            .Aggregate(
                (Sum: 0L, Enabled: true),
                (a, m) => m.Groups[3].Success
                    ? (a.Sum, true)
                    : m.Groups[4].Success
                        ? (a.Sum, false)
                        : a.Enabled
                            ? (a.Sum + m.Groups[1].Value.Long() * m.Groups[2].Value.Int(), true)
                            : (a.Sum, false))
            .Sum
            .P();

        // 80570939
    }

    /*
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX

    */
    public static void Day04A()
    {
        var input = Read.StringBatch().ToArray();

        var h = input.Length; if (h == 0) return;
        var w = input[0].Length;

        var count = 0;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (input[i][j] != 'X')
                    continue;

                if (All(1, 0)) count++;     // D
                if (All(1, 1)) count++;     // DR
                if (All(0, 1)) count++;     //  R
                if (All(-1, 1)) count++;    // UR
                if (All(-1, 0)) count++;    // U
                if (All(-1, -1)) count++;   // UL
                if (All(0, -1)) count++;    //  L
                if (All(1, -1)) count++;    // DL

                bool All(int y, int x)
                {
                    return Check(1 * y, 1 * x, 'M')
                        && Check(2 * y, 2 * x, 'A')
                        && Check(3 * y, 3 * x, 'S');
                }

                bool Check(int y, int x, char c)
                {
                    return i + y >= 0
                        && i + y < h
                        && j + x >= 0
                        && j + x < w
                        && input[i + y][j + x] == c;
                }
            }
        }

        count.P();
    }

    public static void Day04B()
    {
        var input = Read.StringBatch().ToArray();

        var h = input.Length; if (h == 0) return;
        var w = input[0].Length;

        var count = 0;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (input[i][j] != 'M')
                    continue;

                // if (All(1, 0)) count++;     // D
                if (All(1, 1)) count++;     // DR
                // if (All(0, 1)) count++;     //  R
                if (All(-1, 1)) count++;    // UR
                // if (All(-1, 0)) count++;    // U
                if (All(-1, -1)) count++;   // UL
                // if (All(0, -1)) count++;    //  L
                if (All(1, -1)) count++;    // DL

                bool All(int y, int x)
                {
                    return Check(1 * y, 1 * x, 'A')
                        && Check(2 * y, 2 * x, 'S')
                        &&
                        (
                            Check(2 * y, 0 * x, 'M')
                            && Check(0 * y, 2 * x, 'S')
                        ||
                            Check(2 * y, 0 * x, 'S')
                            && Check(0 * y, 2 * x, 'M')
                        );
                }

                bool Check(int y, int x, char c)
                {
                    return i + y >= 0
                        && i + y < h
                        && j + x >= 0
                        && j + x < w
                        && input[i + y][j + x] == c;
                }
            }
        }

        (count / 2).P();
    }

    /*
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47

    */
    public static void Day05A()
    {
        Dictionary<int, HashSet<int>> orderPagesAfter = [];
        Dictionary<int, HashSet<int>> orderPagesBefore = [];

        foreach (var rule in Read.HBatches<int>('|'))
        {
            if (!orderPagesAfter.TryAdd(rule[0], [rule[1]]))
                orderPagesAfter[rule[0]].Add(rule[1]);

            if (!orderPagesBefore.TryAdd(rule[1], [rule[0]]))
                orderPagesBefore[rule[1]].Add(rule[0]);
        }

        var sum = 0;
        foreach (var update in Read.HBatches<int>(','))
        {
            if (IsValid(update, orderPagesAfter, orderPagesBefore)) sum += update[update.Length / 2];
        }

        sum.P();

        static bool IsValid(int[] update, Dictionary<int, HashSet<int>> orderPagesAfter, Dictionary<int, HashSet<int>> orderPagesBefore)
        {
            for (int i = 0; i < update.Length; i++)
            {
                int page = update[i];

                if (orderPagesAfter.TryGetValue(page, out var pagesAfter))
                {
                    for (int j = i + 1; j < update.Length; j++)
                    {
                        if (!pagesAfter.Contains(update[j]))
                            return false;
                    }
                }

                if (orderPagesBefore.TryGetValue(page, out var pagesBefore))
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!pagesBefore.Contains(update[j]))
                            return false;
                    }
                }
            }

            return true;
        }
    }

    public static void Day05B()
    {
        Dictionary<int, HashSet<int>> orderPagesAfter = new();
        Dictionary<int, HashSet<int>> orderPagesBefore = new();

        foreach (var rule in Read.HBatches<int>('|'))
        {
            if (!orderPagesAfter.TryAdd(rule[0], [rule[1]]))
                orderPagesAfter[rule[0]].Add(rule[1]);

            if (!orderPagesBefore.TryAdd(rule[1], [rule[0]]))
                orderPagesBefore[rule[1]].Add(rule[0]);
        }

        var sum = 0;
        foreach (var update in Read.HBatches<int>(','))
        {
            if (!IsValid(update, orderPagesAfter, orderPagesBefore, out var tempFix))
            {
                var newUpdate = tempFix;
                while (!IsValid(newUpdate, orderPagesAfter, orderPagesBefore, out tempFix))
                {
                    newUpdate = tempFix;
                }

                sum += newUpdate[newUpdate.Length / 2];
            }
        }

        sum.P();

        static bool IsValid(int[] update, Dictionary<int, HashSet<int>> orderPagesAfter, Dictionary<int, HashSet<int>> orderPagesBefore, [NotNullWhen(false)] out int[]? updated)
        {
            for (int i = 0; i < update.Length; i++)
            {
                int page = update[i];

                if (orderPagesAfter.TryGetValue(page, out var pagesAfter))
                {
                    foreach (var pageAfter in pagesAfter)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (update[j] == pageAfter)
                            {
                                updated = update.ToArray();
                                updated[j] = update[i];
                                updated[i] = update[j];
                                return false;
                            }
                        }
                    }
                }

                if (orderPagesBefore.TryGetValue(page, out var pagesBefore))
                {
                    foreach (var pageBefore in pagesBefore)
                    {
                        for (int j = i + 1; j < update.Length; j++)
                        {
                            if (update[j] == pageBefore)
                            {
                                updated = update.ToArray();
                                updated[j] = update[i];
                                updated[i] = update[j];
                                return false;
                            }
                        }
                    }
                }
            }

            updated = null;
            return true;
        }
    }

    /*
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...

    */
    private struct Guard
    {
        public Point Point { get; set; }
        public Direction Direction { get; set; }
    }
    enum Direction { Up, Down, Left, Right }
    public static void Day06A()
    {
        var map1 = Read.StringBatch().ToArray();
        if (map1.Length == 0) return;
        var h = map1.Length;
        var w = map1[0].Length;

        var guard = map1.SelectIndex().Select(x => (x.Index, x.Value.IndexOf('^'))).Where(x => x.Item2 >= 0).Select(x => new Guard() { Point = new(x.Index, x.Item2), }).First();
        var map = map1.Select(x => x.ToArray()).ToArray();

        while (true)
        {
            map[guard.Point.X][guard.Point.Y] = 'X';

            var offset = Next(guard.Direction);
            var next = new Point(guard.Point.X + offset.X, guard.Point.Y + offset.Y);

            if (next.X < 0
                || next.Y < 0
                || next.X >= h
                || next.Y >= w)
                break;

            if (map[next.X][next.Y] == '#')
            {
                guard.Direction = New(guard.Direction);
                continue;
            }

            guard.Point = next;
        }

        map.Sum(x => x.Count(p => p == 'X')).P();

        static Point Next(Direction direction) => direction switch
        {
            Direction.Up => new(-1, 0),
            Direction.Down => new(1, 0),
            Direction.Left => new(0, -1),
            Direction.Right => new(0, 1),
            _ => new(0, 0),
        };

        static Direction New(Direction direction) => direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            Direction.Right => Direction.Down,
            _ => Direction.Up,
        };
    }

    public static void Day06B()
    {
        var map1 = Read.StringBatch().ToArray();
        if (map1.Length == 0) return;
        var h = map1.Length;
        var w = map1[0].Length;

        var guard = map1.SelectIndex().Select(x => (x.Index, x.Value.IndexOf('^'))).Where(x => x.Item2 >= 0).Select(x => new Guard() { Point = new(x.Index, x.Item2), }).First();

        var cnt = 0;
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if ((h, i) == (guard.Point.X, guard.Point.Y)) continue;

                if (IsInLoop(map1, guard, i, j))
                    cnt++;
            }
        }

        cnt.P();

        static bool IsInLoop(string[] map1, Guard guard, int x, int y)
        {
            var h = map1.Length;
            var w = map1[0].Length;

            var map = map1.Select(x => x.ToArray()).ToArray();
            map[x][y] = '#';

            while (true)
            {
                if (map[guard.Point.X][guard.Point.Y] <= 16
                    && (map[guard.Point.X][guard.Point.Y] & (1 << (int)guard.Direction)) != 0) return true;

                if (map[guard.Point.X][guard.Point.Y] >= 16)
                    map[guard.Point.X][guard.Point.Y] = (char)0;
                map[guard.Point.X][guard.Point.Y] |= (char)(1 << (int)guard.Direction);

                var offset = Next(guard.Direction);
                var next = new Point(guard.Point.X + offset.X, guard.Point.Y + offset.Y);

                if (next.X < 0
                    || next.Y < 0
                    || next.X >= h
                    || next.Y >= w)
                    return false;

                if (map[next.X][next.Y] == '#')
                {
                    guard.Direction = New(guard.Direction);
                    continue;
                }

                guard.Point = next;
            }

            static Point Next(Direction direction) => direction switch
            {
                Direction.Up => new(-1, 0),
                Direction.Down => new(1, 0),
                Direction.Left => new(0, -1),
                Direction.Right => new(0, 1),
                _ => new(0, 0),
            };

            static Direction New(Direction direction) => direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => Direction.Up,
            };
        }
    }

    /*
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20

    */
    private readonly record struct Equation(long Result, long[] Values);
    public static void Day07A()
    {
        Read.StringBatch()
            .Select(x => x.Split(' '))
            .Select(x => new Equation(x[0][..^1].Long(), x.Skip(1).Select(v => v.Long()).ToArray()))
            .Where(IsValid)
            .Sum(x => x.Result)
            .P();

        // 1611660863222
        static bool IsValid(Equation equation)
        {
            return Execute(equation, 1, equation.Values[0]);

            static bool Execute(Equation equation, int position, long result)
            {
                if (position == equation.Values.Length)
                {
                    return result == equation.Result;
                }

                if (result > equation.Result)
                {
                    return false;
                }

                return Execute(equation, position + 1, Mul(result, equation.Values[position]))
                    || Execute(equation, position + 1, Add(result, equation.Values[position]));
            }

            static long Mul(long left, long right) => left * right;
            static long Add(long left, long right) => left + right;
        }
    }

    public static void Day07B()
    {
        Read.StringBatch()
            .Select(x => x.Split(' '))
            .Select(x => new Equation(x[0][..^1].Long(), x.Skip(1).Select(v => v.Long()).ToArray()))
            .Where(IsValid)
            .Sum(x => x.Result)
            .P();

        // 945341732469724
        static bool IsValid(Equation equation)
        {
            return Execute(equation, 1, equation.Values[0]);

            static bool Execute(Equation equation, int position, long result)
            {
                if (position == equation.Values.Length)
                {
                    return result == equation.Result;
                }

                if (result > equation.Result)
                {
                    return false;
                }

                return Execute(equation, position + 1, Con(result, equation.Values[position]))
                    || Execute(equation, position + 1, Mul(result, equation.Values[position]))
                    || Execute(equation, position + 1, Add(result, equation.Values[position]));
            }

            static long Con(long left, long right) => $"{left}{right}".Long();
            static long Mul(long left, long right) => left * right;
            static long Add(long left, long right) => left + right;
        }
    }

    public static void Current() // Day09A()
    {

    }
}