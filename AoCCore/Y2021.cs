using System.Diagnostics.CodeAnalysis;

namespace AoCCore;

public static class Y2021
{
    public static void Day01A()
    {
        var previous = int.MaxValue;
        var count = 0;
        // Read.TryInt(out var next)
        while (int.TryParse(Read.Line(), out var next))
        {
            if (next > previous)
            {
                count++;
            }

            previous = next;
        }

        Console.WriteLine("Total: " + count);
    }

    public static void Day01B()
    {
        var count = 0;
        var previous = int.MaxValue;
        var queue = new Queue<int>(4);
        for (var i = 0; i < 2; i++)
        {
            queue.Enqueue(int.Parse(Read.Line()));
        }

        while (int.TryParse(Read.Line(), out var next))
        {
            queue.Enqueue(next);

            var sum = queue.Sum();
            if (sum > previous)
            {
                count++;
            }

            previous = sum;
            queue.Dequeue();
        }

        Console.WriteLine("Total: " + count);
    }

    public static void Day02A()
    {
        /*
forward 5
down 5
forward 8
up 3
down 8
forward 2*/
        var forward = 0;
        var depth = 0;

        while (true)
        {
            var input = Read.Line().Split(' ');
            if (input.Length != 2)
            {
                break;
            }

            var value = int.Parse(input[1]);

            switch (input[0])
            {
                case "forward":
                    forward += value;
                    break;
                case "down":
                    depth += value;
                    break;
                case "up":
                    depth -= value;
                    break;
                default:
                    true.Assert();
                    return;
            }
        }

        Console.WriteLine("Total: " + forward * depth);
    }

    public static void Day02B()
    {
        /*
forward 5
down 5
forward 8
up 3
down 8
forward 2*/
        var forward = 0;
        var aim = 0;
        var depth = 0;

        while (true)
        {
            var input = Read.Line().Split(' ');
            if (input.Length != 2)
            {
                break;
            }

            var value = int.Parse(input[1]);

            switch (input[0])
            {
                case "forward":
                    forward += value;
                    depth += aim * value;
                    break;
                case "down":
                    aim += value;
                    break;
                case "up":
                    aim -= value;
                    break;
                default:
                    true.Assert();
                    return;
            }
        }

        Console.WriteLine("Total: " + forward * depth);
    }

    public static void Day03A()
    {
        const int Length = 12;
        var counts = new int[Length];

        string line;
        while ((line = Read.Line()).Length > 0)
        {
            for (var i = 0; i < Length; i++)
            {
                counts[i] += line[i] == '1' ? 1 : -1;
            }
        }

        uint total = 0;
        for (var i = 0; i < Length; i++)
        {
            if (counts[i] > 0)
            {
                total += (uint)(1 << (Length - i - 1));
            }
        }

        Console.WriteLine("Total: " + total);
        Console.WriteLine("Total: " + ((1 << Length) - total - 1));
        Console.WriteLine("Total: " + (long)total * ((1 << Length) - total - 1));
    }

    public static void Day03B()
    {
        var items = new List<string>();
        string line;
        while ((line = Read.Line()).Length > 0)
        {
            items.Add(line);
        }

        const int Length = 12;
        string oxygen = "";
        string co2 = "";

        for (var i = 0; i < Length; i++)
        {
            var ones = 0;
            var all = items.Where(x => x.StartsWith(oxygen)).Select(x => { if (x[i] == '1') ones += 2; return x; }).Count();
            oxygen += ones >= all ? '1' : '0';

            var zeros = 0;
            var all2 = items.Where(x => x.StartsWith(co2)).Select(x => { if (x[i] == '0') zeros += 2; return x; }).Count();
            co2 += (zeros > 0 && zeros <= all2) || zeros == all2 * 2 ? '0' : '1';
        }

        var oxygenValue = 0;
        var co2Value = 0;
        for (var i = 0; i < Length; i++)
        {
            oxygenValue += oxygen[Length - i - 1] == '1' ? (1 << i) : 0;
            co2Value += co2[Length - i - 1] == '1' ? (1 << i) : 0;
        }

        Console.WriteLine("Total: " + oxygenValue);
        Console.WriteLine("Total: " + co2Value);
        Console.WriteLine("Total: " + oxygenValue * co2Value);
    }

    private class Board04A
    {
        private readonly Line[] lines;
        private readonly Line[] cols;

        private Board04A()
        {
            lines = new Line[5];
            cols = new Line[5];
        }

        public static bool TryParse([NotNullWhen(true)] out Board04A? board)
        {
            var line = Read.Line();
            if (line.Length == 0)
            {
                board = null;
                return false;
            }

            board = new Board04A();

            for (var i = 0; i < 5; i++)
            {
                board.lines[i] = new Line(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
                line = Read.Line();
            }

            for (var i = 0; i < 5; i++)
            {
                board.cols[i] = new Line(board.lines.Select(l => l.numbers[i]));
            }
            return true;
        }

        public bool Mark(int number)
        {
            var result = false;

            for (var i = 0; i < 5; i++)
            {
                result |= lines[i].Mark(number);
                result |= cols[i].Mark(number);
            }

            return result;
        }

        public int Sum()
        {
            return lines.Sum(x => x.numbers.Sum());
        }

        private class Line
        {
            public readonly List<int> numbers;

            public Line(IEnumerable<int> input)
            {
                numbers = input.ToList();
            }

            public bool Mark(int number)
            {
                numbers.Remove(number);
                return numbers.Count == 0;
            }
        }
    }
    public static void Day04A()
    {
        var numbers = Read.Line().Split(',').Select(int.Parse);
        Read.Line();

        var boards = new List<Board04A>();
        while (Board04A.TryParse(out var board)) boards.Add(board);

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                if (board.Mark(number))
                {
                    Console.WriteLine("Total: " + board.Sum());
                    Console.WriteLine("Total: " + board.Sum() * number);
                    return;
                }
            }
        }

        "damn".P();
    }

    public static void Day04B()
    {
        var numbers = Read.Line().Split(',').Select(int.Parse);
        Read.Line();

        var boards = new List<Board04A>();
        while (Board04A.TryParse(out var board)) boards.Add(board);

        foreach (var number in numbers)
        {
            for (var i = 0; i < boards.Count; i++)
            {
                if (boards[i].Mark(number))
                {
                    if (boards.Count == 1)
                    {
                        Console.WriteLine("Total: " + boards.Single().Sum());
                        Console.WriteLine("Total: " + boards.Single().Sum() * number);
                        return;
                    }

                    boards.RemoveAt(i);
                    i--;
                }
            }
        }

        "damn".P();
    }

    public static void Day05A()
    {
        /*
0,9-> 5,9
8,0-> 0,8
9,4-> 3,4
2,2-> 2,1
7,0-> 7,4
6,4-> 2,0
0,9-> 2,9
3,4-> 1,4
0,0-> 8,8
5,5-> 8,2
       */
        const int Size = 1000;
        // var board = new int[Size, Size];
        var board = Enumerable.Repeat(1, Size).Select(_ => new int[Size]).ToArray();

        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var pair = line.Split("-> ");
            var p0 = Parse(pair[0]);
            var p1 = Parse(pair[1]);

            if (p0.X == p1.X)
            {
                for (var i = Math.Min(p0.Y, p1.Y); i <= Math.Max(p0.Y, p1.Y); i++)
                {
                    board[i][p0.X]++;
                }
            }
            else if (p0.Y == p1.Y)
            {
                for (var i = Math.Min(p0.X, p1.X); i <= Math.Max(p0.X, p1.X); i++)
                {
                    board[p0.Y][i]++;
                }
            }
            else
            {

            }

            // foreach (var boardLine in board)
            //     Console.WriteLine(string.Join("", boardLine));
            // 
            // Console.WriteLine();
        }

        Console.WriteLine("Total: " + board.Sum(x => x.Count(y => y > 1)));

        static (int X, int Y) Parse(string input)
        {
            var split = input.Split(',').Select(int.Parse).ToArray();
            return (split[0], split[1]);
        }
    }

    public static void Day05B()
    {
        /*
0,9-> 5,9
8,0-> 0,8
9,4-> 3,4
2,2-> 2,1
7,0-> 7,4
6,4-> 2,0
0,9-> 2,9
3,4-> 1,4
0,0-> 8,8
5,5-> 8,2
       */
        const int Size = 1000;
        // var board = new int[Size, Size];
        var board = Enumerable.Repeat(1, Size).Select(_ => new int[Size]).ToArray();

        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var pair = line.Split("-> ");
            var p0 = Parse(pair[0]);
            var p1 = Parse(pair[1]);

            var startX = Math.Min(p0.X, p1.X);
            var startY = Math.Min(p0.Y, p1.Y);
            var endX = Math.Max(p0.X, p1.X);
            var endY = Math.Max(p0.Y, p1.Y);

            var stepX = p1.X.CompareTo(p0.X);
            var stepY = p1.Y.CompareTo(p0.Y);

            var steps = Math.Max(Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y));

            for (var i = 0; i <= steps; i++)
            {
                board[p0.Y + i * stepY][p0.X + i * stepX]++;
            }

            // foreach (var boardLine in board)
            //     Console.WriteLine(string.Join("", boardLine));
            // 
            // Console.WriteLine();
        }

        Console.WriteLine("Total: " + board.Sum(x => x.Count(y => y > 1)));

        static (int X, int Y) Parse(string input)
        {
            var split = input.Split(',').Select(int.Parse).ToArray();
            return (split[0], split[1]);
        }
    }

    public static void Day06A()
    {
        var fish = Read.Line().Split(',').Select(long.Parse).ToList();

        for (var i = 0; i < 80; i++)
        {
            var count = fish.Count;
            for (var j = 0; j < count; j++)
            {
                if (fish[j] == 0)
                {
                    fish[j] = 6;
                    fish.Add(8);
                }
                else
                {
                    fish[j]--;
                }
            }

            // Console.WriteLine($"After {i:D2} days: {string.Join(',', fish)}");
        }

        Console.WriteLine($"Total: {fish.Count}");
    }

    public static void Day06B()
    {
        var fish = Enumerable.Range(0, 9).ToDictionary(x => x, _ => 0L);

        foreach (var i in Read.Line().Split(',').Select(int.Parse))
            fish[i]++;

        for (var i = 0; i < 256; i++)
        {
            var zeros = fish[0];

            foreach (var j in Enumerable.Range(1, 8))
            {
                fish[j - 1] = fish[j];
            }

            fish[6] += zeros;
            fish[8] = zeros;

            // Console.WriteLine($"After {i:D2} days: {string.Join(' ', fish.Select(x => $"{x.Key}:{x.Value:D5}"))}");
        }

        Console.WriteLine($"Total: {fish.Sum(x => x.Value)}");
    }

    public static void Day07A()
    {
        var items = Read.Line().Split(',').Select(int.Parse).ToArray();

        var x1 = items.Min();
        var x2 = items.Max();

        while (true)
        {
            var next = (x1 + x2) / 2;
            var cost1 = Cost(next);
            var cost2 = Cost(next + 1);

            if (x1 == next)
            {
                Console.WriteLine($"Position: {x2}   Cost: {cost2}");
                break;
            }

            if (cost1 > cost2)
            {
                x1 = next;
            }
            else
            {
                x2 = next;
            }
        }

        int Cost(int position)
        {
            return items.Sum(i => Math.Abs(position - i));
        }
    }

    public static void Day07B()
    {
        var items = Read.Line().Split(',').Select(int.Parse).ToArray();

        var x1 = items.Min();
        var x2 = items.Max();

        while (true)
        {
            var next = (x1 + x2) / 2;
            var cost1 = Cost(next);
            var cost2 = Cost(next + 1);

            if (x1 == next)
            {
                Console.WriteLine($"Position: {x2}   Cost: {cost2}");
                break;
            }

            if (cost1 > cost2)
            {
                x1 = next;
            }
            else
            {
                x2 = next;
            }
        }

        int Cost(int position) => items.Sum(i => Cost1(Math.Abs(position - i)));
        int Cost1(int move) => move * (move + 1) / 2;
    }

    public static void Day08A()
    {
        var count = 0;
        string line;
        while ((line = Read.Line()).Length > 0)
        {
            count += line.Substring(61).Split(' ').Count(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7);
        }

        Console.WriteLine($"Total: {count}");
    }

    public static void Day08B()
    {
        var sum = 0;
        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var dict = Decode(line.Substring(0, 58).Split(' '));

            sum += line.Substring(61).Split(' ').Select(x => dict[new string(x.OrderBy(c => c).ToArray())]).Select((x, i) => (int)Math.Pow(10, 3 - i) * x).Sum();
        }

        Console.WriteLine($"Total: {sum}");

        Dictionary<string, int> Decode(string[] inputs)
        {
            var lookup = inputs.ToLookup(x => x.Length);

            var d1 = lookup[2].Single();
            var d7 = lookup[3].Single();
            var d4 = lookup[4].Single();
            var d8 = lookup[7].Single();

            var a = d7.Except(d1).Single();
            var cf = d1.ToArray();
            var bd = d4.Except(d1).ToArray();
            var be = lookup[5].SelectMany(x => x).GroupBy(x => x).GroupBy(x => x.Count(), x => x.Key).Single(x => x.Key == 1).ToArray();
            var b = be.Intersect(bd).Single();
            var e = be.Except(bd).Single();
            var d = bd.Except(be).Single();
            var c = lookup[6].SelectMany(x => x).GroupBy(x => x).GroupBy(x => x.Count(), x => x.Key).Single(x => x.Key == 2).Except(new[] { d, e, }).Single();
            var f = cf.Except(new[] { c, }).Single();
            var g = d8.Except(new[] { a, b, c, d, e, f, }).Single();

            return new[]
            {
                CreatePair(0, a, b, c, e, f, g),
                CreatePair(1, c, f),
                CreatePair(2, a, c, d, e, g),
                CreatePair(3, a, c, d, f, g),
                CreatePair(4, b, c, d, f),
                CreatePair(5, a, b, d, f, g),
                CreatePair(6, a, b, d, e, f, g),
                CreatePair(7, a, c, f),
                CreatePair(8, d8.ToArray()),
                CreatePair(9, a, b, c, d, f, g),
            }.ToDictionary(x => x.Segments, x => x.Digit);
        }

        (string Segments, int Digit) CreatePair(int digit, params char[] segments) => (new string(segments.OrderBy(x => x).ToArray()), digit);
    }

    public static void Day09A()
    {
        var lines = new List<int[]>();

        string line1;
        while ((line1 = Read.Line()).Length > 0)
        {
            lines.Add(line1.Select(c => c - '0').Prepend(int.MaxValue).Append(int.MaxValue).ToArray()); ; ; ;
        }

        lines.Insert(0, Enumerable.Repeat(int.MaxValue, lines[0].Length).ToArray());
        lines.Add(Enumerable.Repeat(int.MaxValue, lines[0].Length).ToArray());

        var rows = lines.Count;
        var cols = lines[0].Length;

        var sum = 0;
        for (var y = 1; y < rows - 1; y++)
        {
            var line = lines[y];

            for (var x = 1; x < cols - 1; x++)
            {
                var min = Min(lines[y - 1][x], lines[y + 1][x], lines[y][x - 1], lines[y][x + 1]);

                if (lines[y][x] < min)
                    sum += lines[y][x] + 1;
            }
        }

        Console.WriteLine($"Sum: {sum}");

        static int Min(int i1, int i2, int i3, int i4)
        {
            return Math.Min(Math.Min(i1, i2), Math.Min(i3, i4));
        }
    }

    class Group09B
    {
        public List<Item09B> Items { get; } = [];
    }

    class Item09B
    {
        public int Z { get; set; }
        public Group09B? Group { get; set; }
    }

    public static void Day09B()
    {
        var border = new Item09B() { Z = 9, };
        var lines = new List<Item09B[]>();
        var groups = new HashSet<Group09B>();

        string line1;
        while ((line1 = Read.Line()).Length > 0)
        {
            lines.Add(line1.Select(c => c - '0').Select(x => new Item09B() { Z = x, }).Prepend(border).Append(border).ToArray());
        }

        lines.Insert(0, Enumerable.Repeat(border, lines[0].Length).ToArray());
        lines.Add(Enumerable.Repeat(border, lines[0].Length).ToArray());

        var rows = lines.Count;
        var cols = lines[0].Length;

        for (var y = 1; y < rows - 1; y++)
        {
            for (var x = 1; x < cols - 1; x++)
            {
                var current = lines[y][x];

                if (current.Z == 9) continue;

                var left = lines[y][x - 1].Group;
                var up = lines[y - 1][x].Group;

                Group09B? group;
                if (left != null && up != null)
                {
                    if (left != up)
                    {
                        left.Items.AddRange(up.Items);
                        up.Items.ForEach(x => x.Group = left);
                        groups.Remove(up);
                    }

                    group = left;
                }
                else
                {
                    group = left ?? up;
                    if (group == null)
                    {
                        group = new Group09B();
                        groups.Add(group);
                    }
                }

                current.Group = group;
                group.Items.Add(current);
            }
        }

        var largest = groups.Select(x => x.Items.Count).OrderByDescending(x => x).Take(3);
        var prod = largest.Aggregate(1, (x, y) => x * y);

        Console.WriteLine($"Prod: {prod}");
    }

    public static void Day10A()
    {
        var total = 0;
        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var commands = new Stack<char>();
            foreach (var c in line)
            {
                var score = GetOpen(c);
                if (score != 0)
                {
                    commands.Push(c);
                    continue;
                }

                score = GetClose(c);
                if (score != GetOpen(commands.Pop()))
                {
                    total += score;
                    break;
                }
            }
        }

        Console.WriteLine($"Total: {total}");

        static int GetOpen(char c)
        {
            return c switch
            {
                '(' => 3,
                '[' => 57,
                '{' => 1197,
                '<' => 25137,
                _ => 0,
            };
        }

        static int GetClose(char c)
        {
            return c switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => throw new Exception(),
            };
        }
    }

    public static void Day10B()
    {
        var totals = new List<long>();
        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var commands = new Stack<char>();
            var corrupted = false;
            foreach (var c in line)
            {
                var score = GetOpen(c);
                if (score != 0)
                {
                    commands.Push(c);
                    continue;
                }

                score = GetClose(c);
                if (score != GetOpen(commands.Pop()))
                {
                    corrupted = true;
                    break;
                }
            }

            if (!corrupted)
            {
                var v = commands.Aggregate(0L, (x, c) => x * 5 + GetOpen(c));
                totals.Add(v);
            }
        }

        totals.Sort();
        totals.ForEach(x => Console.WriteLine(x));
        Console.WriteLine($"Total: {totals[totals.Count / 2]}");
        // 165561627
        static int GetOpen(char c)
        {
            return c switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4,
                _ => 0,
            };
        }

        static int GetClose(char c)
        {
            return c switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => throw new Exception(),
            };
        }
    }

    class Flash11A
    {
        public int Energy { get; set; }
        public bool Flash { get; set; }
    }

    public static void Day11A()
    {
        var lines = new List<List<Flash11A>>();

        string line1;
        while ((line1 = Read.Line()).Length > 0)
        {
            lines.Add(line1.Select(c => new Flash11A() { Energy = c - '0', }).ToList());
        }

        var flashes = 0L;
        for (var i = 0; i < 100; i++)
        {
            lines.ForEach(x => x.ForEach(f => f.Energy++));

            int flashes1;
            do
            {
                flashes1 = 0;
                for (var y = 0; y < lines.Count; y++)
                {
                    for (var x = 0; x < lines.Count; x++)
                    {
                        if (lines[y][x].Energy > 9)
                        {
                            var flash = lines[y][x];
                            flash.Flash = true;
                            flash.Energy = 0;
                            flashes1++;

                            Increase(y - 1, x - 1);
                            Increase(y - 1, x);
                            Increase(y - 1, x + 1);

                            Increase(y, x - 1);
                            Increase(y, x + 1);

                            Increase(y + 1, x - 1);
                            Increase(y + 1, x);
                            Increase(y + 1, x + 1);
                        }

                        void Increase(int y1, int x1)
                        {
                            if (y1 >= 0 && y1 < lines.Count && x1 >= 0 && x1 < lines.Count)
                            {
                                var flash = lines[y1][x1];
                                if (!flash.Flash) flash.Energy++;
                            }
                        }
                    }
                }

                flashes += flashes1;
            }
            while (flashes1 > 0);

            lines.ForEach(x => x.ForEach(f => f.Flash = false));

            if (i < 10 || i % 10 == 0)
            {
                Console.WriteLine($"Step {i + 1}: {flashes}");
                lines.ForEach(x => Console.WriteLine(string.Join("", x.Select(y => y.Energy))));
                Console.WriteLine();
            }
        }

        Console.WriteLine($"Flashes: {flashes}");
    }

    public static void Day11B()
    {
        var lines = new List<List<Flash11A>>();

        string line1;
        while ((line1 = Read.Line()).Length > 0)
        {
            lines.Add(line1.Select(c => new Flash11A() { Energy = c - '0', }).ToList());
        }

        for (var i = 0; i < 1000; i++)
        {
            lines.ForEach(x => x.ForEach(f => f.Energy++));

            var stepFlashes = 0;
            int flashes1;
            do
            {
                flashes1 = 0;
                for (var y = 0; y < lines.Count; y++)
                {
                    for (var x = 0; x < lines.Count; x++)
                    {
                        if (lines[y][x].Energy > 9)
                        {
                            var flash = lines[y][x];
                            flash.Flash = true;
                            flash.Energy = 0;
                            flashes1++;

                            Increase(y - 1, x - 1);
                            Increase(y - 1, x);
                            Increase(y - 1, x + 1);

                            Increase(y, x - 1);
                            Increase(y, x + 1);

                            Increase(y + 1, x - 1);
                            Increase(y + 1, x);
                            Increase(y + 1, x + 1);
                        }

                        void Increase(int y1, int x1)
                        {
                            if (y1 >= 0 && y1 < lines.Count && x1 >= 0 && x1 < lines.Count)
                            {
                                var flash = lines[y1][x1];
                                if (!flash.Flash) flash.Energy++;
                            }
                        }
                    }
                }

                stepFlashes += flashes1;
            }
            while (flashes1 > 0);

            lines.ForEach(x => x.ForEach(f => f.Flash = false));

            if (i < 10 || i % 10 == 0)
            {
                Console.WriteLine($"Step {i + 1}: {stepFlashes}");
                // lines.ForEach(x => Console.WriteLine(string.Join("", x.Select(y => y.Energy))));
                // Console.WriteLine();
            }

            if (stepFlashes == lines.Count * lines.Count)
            {
                Console.WriteLine($"Step {i + 1}: {stepFlashes}");
                return;
            }
        }

        Console.WriteLine($"Flashes out");
    }

    public static void Current()
    {
        var lines = new List<List<Flash11A>>();

        string line1;
        while ((line1 = Read.Line()).Length > 0)
        {
            lines.Add(line1.Select(c => new Flash11A() { Energy = c - '0', }).ToList());
        }

        for (var i = 0; i < 1000; i++)
        {
            lines.ForEach(x => x.ForEach(f => f.Energy++));

            var stepFlashes = 0;
            int flashes1;
            do
            {
                flashes1 = 0;
                for (var y = 0; y < lines.Count; y++)
                {
                    for (var x = 0; x < lines.Count; x++)
                    {
                        if (lines[y][x].Energy > 9)
                        {
                            var flash = lines[y][x];
                            flash.Flash = true;
                            flash.Energy = 0;
                            flashes1++;

                            Increase(y - 1, x - 1);
                            Increase(y - 1, x);
                            Increase(y - 1, x + 1);

                            Increase(y, x - 1);
                            Increase(y, x + 1);

                            Increase(y + 1, x - 1);
                            Increase(y + 1, x);
                            Increase(y + 1, x + 1);
                        }

                        void Increase(int y1, int x1)
                        {
                            if (y1 >= 0 && y1 < lines.Count && x1 >= 0 && x1 < lines.Count)
                            {
                                var flash = lines[y1][x1];
                                if (!flash.Flash) flash.Energy++;
                            }
                        }
                    }
                }

                stepFlashes += flashes1;
            }
            while (flashes1 > 0);

            lines.ForEach(x => x.ForEach(f => f.Flash = false));

            if (i < 10 || i % 10 == 0)
            {
                Console.WriteLine($"Step {i + 1}: {stepFlashes}");
                // lines.ForEach(x => Console.WriteLine(string.Join("", x.Select(y => y.Energy))));
                // Console.WriteLine();
            }

            if (stepFlashes == lines.Count * lines.Count)
            {
                Console.WriteLine($"Step {i + 1}: {stepFlashes}");
                return;
            }
        }

        Console.WriteLine($"Flashes out");
    }
}