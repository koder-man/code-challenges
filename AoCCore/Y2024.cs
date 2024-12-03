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
        new Regex(@"mul\((\d+),(\d+)\)")
            .Matches(Read.StringBatch().Join())
            .Sum(m => m.Groups[1].Value.Long() * m.Groups[2].Value.Int())
            .P("Sum");

        // 179834255
    }

    /*
xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))

    */
    public static void Day03B()
    {
        new Regex(@"mul\((\d+),(\d+)\)|(do\(\))|(don't\(\))")
            .Matches(string.Join(",", Read.StringBatch()))
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

    public static void Current() // Day05A()
    {
    }
}