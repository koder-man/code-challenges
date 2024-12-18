﻿namespace AoCCore;

public static class Y2022
{
    public static void Day01A()
    {
        var max = 0L;

        while (true)
        {
            var sum = Read.LongBatch().Sum();
            if (sum == 0) { break; }

            if (sum > max) { max = sum; }
        }

        Console.WriteLine(max);
    }

    public static void Day01B()
    {
        var all = new List<long>();

        foreach (var batch in Read.Batches<long>())
        {
            var sum = batch.Sum();
            if (sum == 0) { break; }

            all.Add(sum);
        }

        Console.WriteLine(all.OrderDescending().Take(3).Sum());
    }

    public static void Day02A()
    {
        var sum = 0;

        string line;
        while ((line = Read.Line()).Length > 0)
        {
            var a = line[0];
            var b = line[2];

            sum += (b - 'X' + 1);
            sum += (GetScore(a, b));
        }

        Console.WriteLine(sum);

        static int GetScore(char a, char b)
        {
            b = b switch
            {
                'X' => 'A', // r
                'Y' => 'B', // p
                'Z' => 'C', // s
                _ => throw new NotImplementedException(),
            };

            if (a == b) return 3;
            if (a == (b - 1)) return 6;
            if ((a, b) == ('C', 'A')) return 6;

            return 0;
        }
    }

    public static void Day02B()
    {
        var sum = 0;

        foreach (var line in Read.StringBatch())
        {
            var a = line[0];
            var b = line[2];

            var r = GetScore(a, b);
            Console.WriteLine(r);
            sum += r.Item1 + r.Item2;
        }

        Console.WriteLine(sum);

        static (int, int) GetScore(char a, char b)
        {
            // lose draw win
            return (a, b) switch
            {
                ('A', 'X') => (3, 0),
                ('A', 'Y') => (1, 3),
                ('A', 'Z') => (2, 6),
                ('B', 'X') => (1, 0),
                ('B', 'Y') => (2, 3),
                ('B', 'Z') => (3, 6),
                ('C', 'X') => (2, 0),
                ('C', 'Y') => (3, 3),
                ('C', 'Z') => (1, 6),
                _ => throw new NotImplementedException(),
            };
        }
    }

    public static void Day03A()
    {
        var sum = 0;
        foreach (var line in Read.StringBatch())
        {
            var one = line[..(line.Length / 2)];
            var two = line[(line.Length / 2)..];

            var x = one.First(two.Contains);
            var t = x - (char.IsUpper(x) ? ('A' - 27) : ('a' - 1));

            Console.WriteLine(x);
            Console.WriteLine(t);
            sum += t;
        }

        Console.WriteLine(sum);
    }

    public static void Day04A()
    {
        var sum = 0;
        foreach (var line in Read.StringBatch())
        {
            var split = line.Split(',');
            int dash1 = split[0].IndexOf('-');
            int dash2 = split[1].IndexOf('-');
            var s1 = int.Parse(split[0][..dash1]);
            var e1 = int.Parse(split[0][(dash1 + 1)..]);
            var s2 = int.Parse(split[1][..dash2]);
            var e2 = int.Parse(split[1][(dash2 + 1)..]);

            if (s1 <= s2 && e1 >= e2)
            {
                sum++;
                Console.WriteLine("match");
            }
            else if (s1 >= s2 && e1 <= e2)
            {
                sum++;
                Console.WriteLine("match");
            }
        }

        Console.WriteLine(sum);
    }

    public static void Day04B()
    {
        var sum = 0;
        foreach (var line in Read.StringBatch())
        {
            var split = line.Split(',');
            int dash1 = split[0].IndexOf('-');
            int dash2 = split[1].IndexOf('-');
            var s1 = int.Parse(split[0][..dash1]);
            var e1 = int.Parse(split[0][(dash1 + 1)..]);
            var s2 = int.Parse(split[1][..dash2]);
            var e2 = int.Parse(split[1][(dash2 + 1)..]);

            if (s2 >= s1 && s2 <= e1)
            {
                sum++;
            }
            else if (s1 >= s2 && s1 <= e2)
            {
                sum++;
            }
            else
            {
                Console.WriteLine("no match");
            }
        }

        Console.WriteLine(sum);
    }

    public static void Day05A()
    {
        const int cols = 9;
        var state = Enumerable.Repeat(0, cols).Select(_ => new Stack<char>()).ToArray();
        foreach (var line in Read.StringBatch())
        {
            if (char.IsDigit(line[1])) break;

            for (var i = 1; i < cols * 4; i += 4)
            {
                if (line[i] != ' ')
                {
                    state[i / 4].Push(line[i]);
                }
            }
        }

        Read.Line();

        for (int i = 0; i < state.Length; i++)
        {
            state[i] = new Stack<char>(state[i]);
        }

        var rx = new Regex(@"^move (\d+) from (\d+) to (\d+)$");
        foreach (var line in Read.StringBatch())
        {
            var match = rx.Match(line);
            if (!match.Success)
            {
                Console.WriteLine("aaaa " + line);
                break;
            }

            for (var i = 0; i < int.Parse(match.Groups[1].Value); i++)
            {
                state[int.Parse(match.Groups[3].Value) - 1].Push(state[int.Parse(match.Groups[2].Value) - 1].Pop());
            }
        }

        Console.WriteLine(new string(state.Select(x => x.Peek()).ToArray()));
    }

    public static void Day05B()
    {
        var cols = Read.Int();
        var state = Enumerable.Repeat(0, cols).Select(_ => new Stack<char>()).ToArray();
        foreach (var line in Read.StringBatch())
        {
            if (char.IsDigit(line[1])) break;

            for (var i = 1; i < cols * 4; i += 4)
            {
                if (line[i] != ' ')
                {
                    state[i / 4].Push(line[i]);
                }
            }
        }

        Read.Line();

        for (int i = 0; i < state.Length; i++)
        {
            state[i] = new Stack<char>(state[i]);
        }

        var rx = new Regex(@"^move (\d+) from (\d+) to (\d+)$");
        foreach (var line in Read.StringBatch())
        {
            var match = rx.Match(line);
            if (!match.Success)
            {
                Console.WriteLine("aaaa " + line);
                break;
            }

            var tmp = new List<char>();
            for (var i = 0; i < int.Parse(match.Groups[1].Value); i++)
            {
                tmp.Add(state[int.Parse(match.Groups[2].Value) - 1].Pop());
            }
            foreach (var item in tmp.AsEnumerable().Reverse())
            {
                state[int.Parse(match.Groups[3].Value) - 1].Push(item);
            }
        }

        Console.WriteLine(new string(state.Select(x => x.Peek()).ToArray()));
    }

    public static void Day06A()
    {
        var line = Read.Line();

        var chars = new Queue<char>();
        for (var i = 0; i < 3; i++)
        {
            chars.Enqueue(line[i]);
        }

        for (int i = 3; i < line.Length; i++)
        {
            chars.Enqueue(line[i]);
            if (chars.Distinct().Count() == 4)
            {
                Console.WriteLine(i + 1);
                break;
            }

            chars.Dequeue();
        }

        Console.WriteLine("what");
    }

    public static void Day06B()
    {
        var line = Read.Line();

        var chars = new Queue<char>();
        for (var i = 0; i < 13; i++)
        {
            chars.Enqueue(line[i]);
        }

        for (int i = 13; i < line.Length; i++)
        {
            chars.Enqueue(line[i]);
            if (chars.Distinct().Count() == 14)
            {
                Console.WriteLine(i + 1);
                break;
            }

            chars.Dequeue();
        }

        Console.WriteLine("what");
    }

    public static void Day07A()
    {
        var all = new Dictionary<string, long>();
        var path = new Stack<string>();
        foreach (var line in Read.StringBatch())
        {
            if (line[0] == '$')
            {
                switch (line.Substring(2, 2))
                {
                    case "cd":
                        if (line[5] == '.')
                            path.Pop();
                        else
                        {
                            path.Push(line.Substring(5));
                            all.Add(string.Join('\\', path.Reverse()), 0);
                        }
                        continue;
                    case "ls":
                        continue;
                    default:
                        throw new Exception("invalid command");
                }
            }

            if (line.StartsWith("dir")) continue;

            long size = long.Parse(line[..line.IndexOf(' ')]);
            for (int i = 0; i < path.Count; i++)
            {
                all[string.Join('\\', path.Reverse().Take(i + 1))] += size;
            }
        }

        Console.WriteLine(all.Values.Where(x => x <= 100000).Sum());
    }

    public static void Day07B()
    {
        var all = new Dictionary<string, long>();
        var path = new Stack<string>();
        foreach (var line in Read.StringBatch())
        {
            if (line[0] == '$')
            {
                switch (line.Substring(2, 2))
                {
                    case "cd":
                        if (line[5] == '.')
                            path.Pop();
                        else
                        {
                            path.Push(line.Substring(5));
                            all.Add(string.Join('\\', path.Reverse()), 0);
                        }
                        continue;
                    case "ls":
                        continue;
                    default:
                        throw new Exception("invalid command");
                }
            }

            if (line.StartsWith("dir")) continue;

            long size = long.Parse(line[..line.IndexOf(' ')]);
            for (int i = 0; i < path.Count; i++)
            {
                all[string.Join('\\', path.Reverse().Take(i + 1))] += size;
            }
        }


        var total = all["/"];
        var freeUp = total - 40_000_000;
        Console.WriteLine(all.Values.Where(x => x >= freeUp).Order().First());
    }

    public static void Day08A()
    {
        var lines = Read.StringBatch().ToArray();

        var sum = 0L;
        for (var i = 0; i < lines.Length; i++)
        {
            int l = lines[i].Length;
            for (var j = 0; j < l; j++)
            {
                if (i == 0 || j == 0 || i == lines.Length - 1 || j == l - 1) { sum++; continue; }

                var h = lines[i][j];

                // row
                var isTallest = true;
                for (var x = 0; x < j; x++)
                {
                    if (lines[i][x] < h) continue;

                    isTallest = false;
                }
                if (isTallest)
                {
                    sum++;
                    continue;
                }
                // row
                isTallest = true;
                for (var x = j + 1; x < l; x++)
                {
                    if (lines[i][x] < h) continue;

                    isTallest = false;
                }
                if (isTallest)
                {
                    sum++;
                    continue;
                }

                // col
                isTallest = true;
                for (var x = 0; x < i; x++)
                {
                    if (lines[x][j] < h) continue;

                    isTallest = false;
                }
                if (isTallest)
                {
                    sum++;
                    continue;
                }
                // col
                isTallest = true;
                for (var x = i + 1; x < lines.Length; x++)
                {
                    if (lines[x][j] < h) continue;

                    isTallest = false;
                }
                if (isTallest)
                {
                    sum++;
                    continue;
                }
            }
        }

        Console.WriteLine(sum);
    }

    public static void Day08B()
    {
        var lines = Read.StringBatch().ToArray();

        var max = 0L;
        for (var i = 1; i < lines.Length - 1; i++)
        {
            int l = lines[i].Length;
            for (var j = 1; j < l - 1; j++)
            {
                var h = lines[i][j];

                var current = 1L;
                // row
                var cnt = 0;
                for (var x = j - 1; x >= 0; x--)
                {
                    cnt++;
                    if (lines[i][x] >= h) break;
                }
                current *= cnt;
                // row
                cnt = 0;
                for (var x = j + 1; x < l; x++)
                {
                    cnt++;
                    if (lines[i][x] >= h) break;
                }
                current *= cnt;

                // col
                cnt = 0;
                for (var x = i - 1; x >= 0; x--)
                {
                    cnt++;
                    if (lines[x][j] >= h) break;
                }
                current *= cnt;
                // col
                cnt = 0;
                for (var x = i + 1; x < lines.Length; x++)
                {
                    cnt++;
                    if (lines[x][j] >= h) break;
                }
                current *= cnt;

                if (current > max)
                {
                    max = current;
                    Console.WriteLine($"{i}-{j} {current}");
                }
            }
        }

        Console.WriteLine(max);
    }

    public static void Day09A()
    {
        var visited = new HashSet<(int X, int Y)>();
        (int X, int Y) h = (0, 0);
        var t = h;
        foreach (var line in Read.StringBatch())
        {
            var move = int.Parse(line[2..]);
            var x = 0;
            var y = 0;
            switch (line[0])
            {
                case 'R': x = 1; break;
                case 'L': x = -1; break;
                case 'D': y = 1; break;
                case 'U': y = -1; break;
                default: throw new Exception(line);
            }

            for (int i = 0; i < move; i++) Add(x, y);

            void Add(int x, int y)
            {
                h = (h.X + x, h.Y + y);

                if (Math.Abs(h.X - t.X) + Math.Abs(h.Y - t.Y) == 3) { t.X += h.X > t.X ? 1 : -1; t.Y += h.Y > t.Y ? 1 : -1; }
                if (Math.Abs(h.X - t.X) > 1) t.X += h.X > t.X ? 1 : -1;
                if (Math.Abs(h.Y - t.Y) > 1) t.Y += h.Y > t.Y ? 1 : -1;

                visited.Add(t);

                Console.WriteLine($"{line}: {x}, {y}. h: {h}  t: {t} count: {visited.Count}");
            }
        }

        Console.WriteLine(visited.Count);
    }

    public static void Day09B()
    {
        var visited = new HashSet<(int X, int Y)>();
        //var visited = new Dictionary<(int X, int Y), char>();
        var s = new (int X, int Y)[1 + 9];
        var screenX = (-5, 20);
        var screenY = (-30, 10);

        foreach (var line in Read.StringBatch())
        {
            var move = int.Parse(line[2..]);
            var x = 0;
            var y = 0;
            switch (line[0])
            {
                case 'R': x = 1; break;
                case 'L': x = -1; break;
                case 'D': y = 1; break;
                case 'U': y = -1; break;
                default: throw new Exception(line);
            }

            // Console.WriteLine($"{line} {x}, {y}");
            for (int i = 0; i < move; i++)
            {
                // Console.WriteLine($"  {i}");
                Add(x, y, 0);
            }

            void Add(int x, int y, int i)
            {
                s[i] = (s[i].X + x, s[i].Y + y);

                if (i == 9)
                {
                    visited.Add(s[i]);
                    /*visited[s[i]] = (x, y) switch
                    {
                        (0, 1) or (0, -1) => '|',
                        (1, 1) or (-1, -1) => '/',
                        (1, 0) or (-1, 0) => '-',
                        (1, -1) or (-1, 1) => '\\',
                        _ => 'o',
                    };*/

                    Console.SetCursorPosition(0, 0);
                    screenX = (Math.Min(s[0].X, screenX.Item1), Math.Max(s[0].X, screenX.Item2));
                    screenY = (Math.Min(s[0].Y, screenY.Item1), Math.Max(s[0].Y, screenY.Item2));
                    var items = new Dictionary<(int, int), char>();
                    foreach (var item in visited) items[(item)] = '*';
                    // foreach (var item in visited) items[(item.Key)] = item.Value;
                    for (var si = s.Length - 1; si >= 0; si--) items[s[si]] = (char)('0' + si);
                    for (int dy = screenY.Item1; dy <= screenY.Item2; dy++)
                    {
                        Console.WriteLine(new string(
                            Enumerable.Range(screenX.Item1, screenX.Item2 - screenX.Item1)
                                .Select(x => items.TryGetValue((x, dy), out var value) ? value : ' ')
                                .ToArray()));
                    }
                    // System.Threading.Thread.Sleep(10);

                    return;
                }

                var xt = 0;
                var yt = 0;

                if (Math.Abs(s[i].X - s[i + 1].X) + Math.Abs(s[i].Y - s[i + 1].Y) >= 3) { xt = s[i].X > s[i + 1].X ? 1 : -1; yt = s[i].Y > s[i + 1].Y ? 1 : -1; }
                else if (Math.Abs(s[i].X - s[i + 1].X) > 1) xt = s[i].X > s[i + 1].X ? 1 : -1;
                else if (Math.Abs(s[i].Y - s[i + 1].Y) > 1) yt = s[i].Y > s[i + 1].Y ? 1 : -1;

                // Console.WriteLine($"    i:{i}. {xt}, {yt}. count: {visited.Count}. s: {string.Join(',', s)}");

                Add(xt, yt, i + 1);
            }
        }

        Console.WriteLine(visited.Count);
    }

    public static void Day10A()
    {
        var sum = 0L;
        var cycle = 0;
        var x = 1L;
        foreach (var line in Read.StringBatch())
        {
            if (line == "noop")
            {
                Increment();
                continue;
            }

            var val = int.Parse(line.Substring(5));
            Increment();
            Increment();
            x += val;
        }

        Console.WriteLine(sum);

        void Increment()
        {
            cycle++;

            if ((cycle + 20) % 40 == 0 && cycle <= 220)
            {
                sum += cycle * x;
            }
        }
    }

    public static void Day10B()
    {
        var cycle = 0;
        var x = 1L;
        var screen = new bool[240];
        foreach (var line in Read.StringBatch())
        {
            if (line == "noop")
            {
                Draw();
                continue;
            }

            var val = int.Parse(line.Substring(5));
            Draw();
            Draw();
            x += val;
        }

        for (int i = 0; i < 6; i++)
        {
            Console.WriteLine(new string(screen.Skip(i * 40).Take(40).Select(b => b ? '#' : '.').ToArray()));
        }

        void Draw()
        {
            cycle++;

            if (Math.Abs((cycle - 1) % 40 - x) <= 1) screen[cycle - 1] = true;
        }
    }

    public static void Day10Bx()
    {
        var cycle = 0;
        var reg = 1L;
        string[] lines = Read.StringBatch().ToArray();
        Console.WriteLine();
        var offset = Console.CursorTop;

        foreach (var line in lines)
        {
            if (line == "noop")
            {
                Draw();
                continue;
            }

            var val = int.Parse(line.Substring(5));
            Draw();
            Draw();
            reg += val;

            void Draw()
            {
                cycle++;

                Console.WriteLine($"Command:            {line}    ");
                Console.WriteLine($"Cycle:              {cycle}");
                Console.WriteLine();

                Console.WriteLine($"Sprite position:    {new string(Enumerable.Range(0, 40).Select(x => Math.Abs(x - reg) <= 1 ? '#' : '.').ToArray())}");
                Console.WriteLine();

                Console.Write(Math.Abs((cycle - 1) % 40 - reg) <= 1 ? '#' : '.');
                if (cycle % 40 == 0) Console.WriteLine();
                System.Threading.Thread.Sleep(10);
                // if (Math.Abs((cycle - 1) % 40 - x) <= 1) screen[cycle - 1] = true;
            }
        }

        /*for (int i = 0; i < 6; i++)
        {
            Console.WriteLine(new string(screen.Skip(i * 40).Take(40).Select(b => b ? '#' : '.').ToArray()));
        }*/
    }

    public static void Day11A()
    {
        var monkeys = new List<Monkey11A>();

        while (true)
        {
            string[] def = Read.StringBatch().ToArray();
            if (def.Length == 0) break;

            var items = def[1][18..].Split(", ").Select(int.Parse);
            string byVal = def[2][25..];
            Func<int, int> operation = def[2][23] == '*'
                ? val => val * (byVal == "old" ? val : int.Parse(byVal))
                : val => val + (byVal == "old" ? val : int.Parse(byVal));

            monkeys.Add(new Monkey11A(items, operation, int.Parse(def[3][21..]), int.Parse(def[4][29..]), int.Parse(def[5][30..])));
        }

        for (int i = 0; i < 20; i++)
        {
            foreach (var m in monkeys) Console.WriteLine(m);

            foreach (var monkey in monkeys)
            {
                while (monkey.CanProcess())
                    monkeys[monkey.Test(out var newLevel)].Enqueue(newLevel);
            }
        }

        foreach (var m in monkeys) Console.WriteLine(m);

        var maxes = monkeys.Select(m => m.Inspections).OrderDescending().Take(2).ToArray();
        Console.WriteLine(maxes[0] * maxes[1]);
    }

    private class Monkey11A
    {
        private readonly Queue<int> items = new Queue<int>();
        private readonly Func<int, int> operation;
        private readonly int divisibleBy;
        private readonly int toIfTrue;
        private readonly int toIfFalse;

        public Monkey11A(IEnumerable<int> items, Func<int, int> operation, int divisibleBy, int toIfTrue, int toIfFalse)
        {
            this.items = new Queue<int>(items);
            this.operation = operation;
            this.divisibleBy = divisibleBy;
            this.toIfTrue = toIfTrue;
            this.toIfFalse = toIfFalse;
        }

        public long Inspections { get; private set; }

        public bool CanProcess() => items.Any();

        public int Test(out int newLevel)
        {
            Inspections++;
            newLevel = items.Dequeue();
            newLevel = operation(newLevel);
            newLevel /= 3;
            return newLevel % divisibleBy == 0 ? toIfTrue : toIfFalse;
        }

        public void Enqueue(int level) => items.Enqueue(level);

        public override string ToString()
        {
            return $"{Inspections}, {items.Count}: ({string.Join(',', items)}), {divisibleBy}, {toIfTrue}, {toIfFalse}";
        }
    }

    public static void Day11B()
    {
        var input = new List<(IEnumerable<int> Items, bool IsMultiplication, int? ByVal, int DivisableBy, int IfTrue, int IfFalse)>();
        foreach (var def in Read.StringBatches())
        {
            var items = def[1][18..].Split(", ").Select(int.Parse);
            string byVal = def[2][25..];

            input.Add((items, def[2][23] == '*', byVal == "old" ? null : int.Parse(byVal), int.Parse(def[3][21..]), int.Parse(def[4][29..]), int.Parse(def[5][30..])));
        }

        var divisors = input.Select(x => x.DivisableBy).ToArray();
        var monkeys = input.Select(x => new Monkey11B(divisors, x.Items, x.IsMultiplication, x.ByVal, x.DivisableBy, x.IfTrue, x.IfFalse)).ToArray();

        for (int i = 0; i < 10000; i++)
        {
            if (i % 100 == 0) Console.WriteLine(i);
            if (i == 1 || i == 20 || i == 1000)
            {
                Console.WriteLine(i);
                foreach (var m in monkeys) Console.WriteLine(m);
            }

            foreach (var monkey in monkeys)
            {
                while (monkey.CanProcess())
                    monkeys[monkey.Test(out var newLevel)].Enqueue(newLevel);
            }
        }

        foreach (var m in monkeys) Console.WriteLine(m);

        var maxes = monkeys.Select(m => m.Inspections).OrderDescending().Take(2).ToArray();
        Console.WriteLine(maxes[0] * maxes[1]);
    }

    private class Monkey11B
    {
        private readonly Queue<Level> items = new();
        private readonly bool isMultiplication;
        private readonly int? byValue;
        private readonly int divisibleBy;
        private readonly int toIfTrue;
        private readonly int toIfFalse;

        public Monkey11B(IEnumerable<int> divisors, IEnumerable<int> items, bool isMultiplication, int? byValue, int divisibleBy, int toIfTrue, int toIfFalse)
        {
            this.items = new Queue<Level>(items.Select(x => new Level(divisors, x)));
            this.isMultiplication = isMultiplication;
            this.byValue = byValue;
            this.divisibleBy = divisibleBy;
            this.toIfTrue = toIfTrue;
            this.toIfFalse = toIfFalse;
        }

        public long Inspections { get; private set; }

        public bool CanProcess() => items.Any();

        public int Test(out Level newLevel)
        {
            Inspections++;
            newLevel = items.Dequeue();
            if (isMultiplication)
            {
                if (byValue.HasValue)
                    newLevel.Multiply(byValue.Value);
                else
                    newLevel.MultiplyBySelf();
            }
            else
            {
                if (byValue.HasValue)
                    newLevel.Add(byValue.Value);
                else
                    newLevel.AddSelf();
            }

            return newLevel.IsDivisibleBy(divisibleBy) ? toIfTrue : toIfFalse;
        }

        public void Enqueue(Level level) => items.Enqueue(level);

        public override string ToString()
        {
            return $"{Inspections}, {items.Count}: ({string.Join(", ", items)}), {divisibleBy}, {toIfTrue}, {toIfFalse}";
        }

        public class Level
        {
            public Dictionary<int, int> Reminders { get; } = new();

            public Level(IEnumerable<int> divisors, int value)
            {
                foreach (var item in divisors)
                {
                    Set(item, value);
                }
            }

            public void Multiply(int value)
            {
                foreach (var item in Reminders)
                {
                    Set(item.Key, item.Value * value);
                }
            }

            public void MultiplyBySelf()
            {
                foreach (var item in Reminders)
                {
                    Set(item.Key, item.Value * item.Value);
                }
            }

            public void Add(int value)
            {
                foreach (var item in Reminders)
                {
                    Set(item.Key, item.Value + value);
                }
            }

            public void AddSelf()
            {
                Multiply(2);
            }

            public bool IsDivisibleBy(int value) => Reminders[value] == 0;

            private void Set(int divisor, int value) => Reminders[divisor] = value % divisor;

            public override string ToString()
            {
                return string.Join('*', Reminders.Select(x => $"{x.Key}:{x.Value}"));
            }
        }
    }

    public static void Day12A()
    {
        const char Right = '>';
        const char Left = '<';
        const char Down = 'ˇ';
        const char Up = '^';
        const char Star = 'O';
        var invalid = new[] { Right, Left, Down, Up, Star, }.ToHashSet();

        var original = Read.StringBatch().ToArray();
        var map = original.ToArray();

        var S = map.Select((h, y) => new Point(h.IndexOf('S'), y)).First(x => x.X != -1);
        var E = map.Select((h, y) => new Point(h.IndexOf('E'), y)).First(x => x.X != -1);

        var h = map.Length;
        var w = map[0].Length;

        var queues = new Queue<Queue<(Point P, char E)>>();
        queues.Enqueue(new Queue<(Point P, char E)>(new[] { (S, 'a'), }));

        while (queues.TryDequeue(out var queue))
        {
            var found = false;
            var newQueue = new Queue<(Point P, char E)>();

            while (queue.TryDequeue(out var current))
            {
                if (current.P.X > 0) Try(-1, 0, Right);
                if (current.P.X < w - 1) Try(1, 0, Left);
                if (current.P.Y > 0) Try(0, -1, Down);
                if (current.P.Y < h - 1) Try(0, 1, Up);

                if (found) break;

                void Try(int dx, int dy, char dir)
                {
                    var dest = current.P.Move(dx, dy);
                    var destE = Elevation(dest);

                    if (invalid.Contains(destE)) return;
                    if (destE < 'a' || destE > 'z') throw new Exception("invalid elevation");

                    if (destE <= current.E + 1)
                    {
                        Set(dest, dir);

                        if (dest == E)
                        {
                            found = true;
                        }
                        else
                        {
                            newQueue.Enqueue((dest, destE));
                        }
                    }
                }
            }

            Print(map, 1);

            if (found) break;
            if (newQueue.Count > 0) queues.Enqueue(newQueue);
        }

        var back = E;
        var steps = 0;
        var mapWithTrack = map.ToArray();
        var originalWithTrack = original.ToArray();
        while (back != S)
        {
            char e = Elevation(back);
            Set2(mapWithTrack, back, '*');
            Set2(originalWithTrack, back, '*');

            int dx = 0;
            int dy = 0;
            switch (e)
            {
                case Right: dx = 1; break;
                case Left: dx = -1; break;
                case Down: dy = 1; break;
                case Up: dy = -1; break;
                default: throw new Exception("not known");
            }

            back = back.Move(dx, dy);
            steps++;

            Print(mapWithTrack, 2);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Steps: " + steps);

        while (true)
        {
            Print(originalWithTrack, 1000);

            Print(original, 1000);

            // Print(map, 1000);
            // Print(mapWithTrack, 1000);
        }

        // char ElevationOld(Point p) => p == S ? 'a' : p == E ? 'z' : input[p.Y][p.X];
        char Elevation(Point p) => map[p.Y][p.X] switch { 'S' => 'a', 'E' => 'z', var x => x, };
        void Set(Point p, char c) => Set2(map, p, c);
        static void Set2(string[] map, Point p, char c) => map[p.Y] = map[p.Y][..p.X] + c + map[p.Y][(p.X + 1)..];
        static void Print(string[] what, int delay)
        {
            Thread.Sleep(delay);
            Console.SetCursorPosition(0, 0);
            foreach (var p in what) Console.WriteLine(p);
        }
    }

    public static void Day13A()
    {
        var indexes = new List<int>();
        var i = 0;
        foreach (var pair in Read.StringBatches())
        {
            i++;

            var first = Parse(pair[0]);
            var second = Parse(pair[1]);

            var result = Value.CompareList(first, second);
            Console.WriteLine(result);
            if (result == -1) indexes.Add(i);
        }
        Console.WriteLine(indexes.Sum());

        List<Value> Parse(string line)
        {
            var root = Value.Create();

            var stack = new Stack<Value>();
            stack.Push(root);

            var ints = new List<char>();
            bool inInts = false;
            foreach (var c in line[1..])
            {
                if (ProcessInts(c)) continue;

                switch (c)
                {
                    case '[':
                        Value @new = Value.Create();
                        stack.Peek().Values.Add(@new);
                        stack.Push(@new);
                        break;
                    case ']':
                        stack.Pop();
                        break;
                    case ',':
                        break;
                    default:
                        inInts = true;
                        ints.Add(c);
                        break;
                }
            }

            return root.Values;

            bool ProcessInts(char c)
            {
                if (!inInts) return false;

                if (char.IsDigit(c))
                {
                    ints.Add(c);
                    return true;
                }

                if (c == ',' || c == ']')
                {
                    stack.Peek().Values.Add(Value.Create(int.Parse(new string(ints.ToArray()))));
                    ints.Clear();
                    inInts = false;
                }

                return false;
            }
        }
    }

    public static void Day13B()
    {
        var all = new List<Value>();
        var d1 = Parse("[[2]]");
        var d2 = Parse("[[6]]");
        all.Add(d1);
        all.Add(d2);

        foreach (var pair in Read.StringBatches())
        {
            all.Add(Parse(pair[0]));
            all.Add(Parse(pair[1]));
        }

        all.Sort();

        var index1 = all.IndexOf(d1) + 1;
        var index2 = all.IndexOf(d2) + 1;
        Console.WriteLine($"{index1}, {index2}, {index1 * index2}");

        Value Parse(string line)
        {
            var root = Value.Create();

            var stack = new Stack<Value>();
            stack.Push(root);

            var ints = new List<char>();
            bool inInts = false;
            foreach (var c in line[1..])
            {
                if (ProcessInts(c)) continue;

                switch (c)
                {
                    case '[':
                        Value @new = Value.Create();
                        stack.Peek().Values.Add(@new);
                        stack.Push(@new);
                        break;
                    case ']':
                        stack.Pop();
                        break;
                    case ',':
                        break;
                    default:
                        inInts = true;
                        ints.Add(c);
                        break;
                }
            }

            return root;

            bool ProcessInts(char c)
            {
                if (!inInts) return false;

                if (char.IsDigit(c))
                {
                    ints.Add(c);
                    return true;
                }

                if (c == ',' || c == ']')
                {
                    stack.Peek().Values.Add(Value.Create(int.Parse(new string(ints.ToArray()))));
                    ints.Clear();
                    inInts = false;
                }

                return false;
            }
        }
    }
    private class Value : IComparable<Value>
    {
        public int? Int { get; init; }

        public List<Value> Values { get; init; } = new();

        public static Value Create(int value) => new() { Int = value, };
        public static Value Create() => new();

        public int CompareTo(Value? r)
        {
            if (r == null) return 1;

            if (Int.HasValue && r.Int.HasValue == true) return Int.Value.CompareTo(r.Int.Value);

            return CompareList(Int.HasValue ? new[] { this, } : Values, r.Int.HasValue ? new[] { r, } : r.Values);
        }

        public override string? ToString()
        {
            return Int.HasValue ? Int.ToString() : $"[{string.Join(',', Values)}]";
        }

        public static int CompareList(IReadOnlyList<Value> l, IReadOnlyList<Value> r)
        {
            var min = Math.Min(l.Count, r.Count);

            for (int i = 0; i < min; i++)
            {
                switch (l[i].CompareTo(r[i]))
                {
                    case -1: return -1;
                    case 0: continue;
                    case 1: return 1;
                    default: throw new Exception();
                }
            }

            return l.Count == r.Count ? 0 : l.Count.CompareTo(r.Count);
        }
    }

    public static void Day14A()
    {
        const char Rock = '#';
        const char Sand = 'o';

        var paths = Read.StringBatch().Select(line => line.Split(" -> ").Select(x => Parse(x)).ToArray()).ToArray();

        Dictionary<Point, char> map = new();
        foreach (var path in paths)
        {
            var previous = path.First();

            foreach (var point in path.Skip(1))
            {
                if (previous.X == point.X)
                {
                    for (int i = Math.Min(previous.Y, point.Y); i <= Math.Max(previous.Y, point.Y); i++)
                    {
                        map[new(point.X, i)] = Rock;
                    }
                    previous = point;
                    continue;
                }
                if (previous.Y == point.Y)
                {
                    for (int i = Math.Min(previous.X, point.X); i <= Math.Max(previous.X, point.X); i++)
                    {
                        map[new(i, point.Y)] = Rock;
                    }
                    previous = point;
                    continue;
                }
                throw new Exception("not a line");
            }
        }

        var bottom = paths.SelectMany(x => x).Max(x => x.Y);
        var count = Process(bottom, map);
        Console.WriteLine(count);

        static int Process(int bottom, Dictionary<Point, char> map)
        {
            Point start = new(500, 0);
            var count = 0;
            while (true)
            {
                var current = start;

                while (true)
                {
                    // down
                    while (true)
                    {
                        var down = current.Move(0, 1);
                        if (down.Y > bottom)
                        {
                            return count;
                        }

                        if (map.ContainsKey(down))
                            break;

                        current = down;
                    }

                    // down left
                    var left = current.Move(-1, 1);
                    if (!map.ContainsKey(left))
                    {
                        current = left;
                        continue;
                    }

                    // down right
                    var right = current.Move(1, 1);
                    if (!map.ContainsKey(right))
                    {
                        current = right;
                        continue;
                    }

                    break;
                }

                map.Add(current, Sand);
                count++;
            }
        }

        static Point Parse(string value)
        {
            var splitAt = value.IndexOf(',');
            return new(int.Parse(value[..splitAt]), int.Parse(value.Substring(splitAt + 1)));
        }
    }

    public static void Day14B()
    {
        const char Rock = '#';
        const char Sand = 'o';

        var paths = Read.StringBatch().Select(line => line.Split(" -> ").Select(x => Parse(x)).ToArray()).ToArray();

        Dictionary<Point, char> map = new();
        foreach (var path in paths)
        {
            var previous = path.First();

            foreach (var point in path.Skip(1))
            {
                if (previous.X == point.X)
                {
                    for (int i = Math.Min(previous.Y, point.Y); i <= Math.Max(previous.Y, point.Y); i++)
                    {
                        map[new(point.X, i)] = Rock;
                    }
                    previous = point;
                    continue;
                }
                if (previous.Y == point.Y)
                {
                    for (int i = Math.Min(previous.X, point.X); i <= Math.Max(previous.X, point.X); i++)
                    {
                        map[new(i, point.Y)] = Rock;
                    }
                    previous = point;
                    continue;
                }
                throw new Exception("not a line");
            }
        }

        var bottom = paths.SelectMany(x => x).Max(x => x.Y) + 2;
        var left = 500 - bottom - 1;
        var right = 500 + bottom + 1;
        for (int i = left; i <= right; i++)
        {
            map[new(i, bottom)] = Rock;
        }

        var line = Console.CursorTop;

        var count = Process(bottom, map);
        Console.WriteLine(count);

        int Process(int bottom, Dictionary<Point, char> map)
        {
            Point start = new(500, 0);
            var count = 0;
            while (true)
            {
                var current = start;

                while (true)
                {
                    // down
                    while (true)
                    {
                        Print();

                        var down = current.Move(0, 1);
                        if (down.Y > bottom)
                        {
                            throw new Exception("error");
                        }

                        if (map.ContainsKey(down))
                            break;

                        current = down;
                    }

                    // down left
                    var left = current.Move(-1, 1);
                    if (!map.ContainsKey(left))
                    {
                        current = left;
                        continue;
                    }

                    // down right
                    var right = current.Move(1, 1);
                    if (!map.ContainsKey(right))
                    {
                        current = right;
                        continue;
                    }

                    break;
                }

                map.Add(current, Sand);
                count++;

                if (current == start) { Print(); return count; }

                void Print()
                {
                    Console.SetCursorPosition(0, line);
                    for (int i = 0; i <= bottom; i++)
                    {
                        Console.WriteLine(new string(Enumerable.Range(left, right - left + 1).Select(x =>
                            map.TryGetValue(new(x, i), out var value) ? value : new Point(x, i) == current ? '*' : '.').ToArray()));
                    }
                    Thread.Sleep(1);
                }
            }
        }

        static Point Parse(string value)
        {
            var splitAt = value.IndexOf(',');
            return new(int.Parse(value[..splitAt]), int.Parse(value.Substring(splitAt + 1)));
        }
    }

    public static void Day15A()
    {
        var regex = new Regex(@"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$");
        var sensors = Read.StringBatch().Select(x =>
        {
            var match = regex.Match(x);
            if (!match.Success) throw new Exception("not a match");
            return new Sensor15A(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }).ToArray();

        var line = Read.Int();

        HashSet<int> readings = new();
        HashSet<Point> bs = new();
        foreach (var s in sensors)
        {
            var dist = Math.Abs(s.L.X - s.B.X) + Math.Abs(s.L.Y - s.B.Y);

            var offset = dist - Math.Abs(s.L.Y - line);
            var start = s.L.X - offset;
            var end = s.L.X + offset;
            for (int i = start; i <= end; i++)
            {
                readings.Add(i);
            }

            if (s.B.Y == line) bs.Add(s.B);
        }

        Console.WriteLine(readings.Count - bs.Count);
    }
    public static void Day15BLong()
    {
        var regex = new Regex(@"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$");
        var sensors = Read.StringBatch().Select(x =>
        {
            var match = regex.Match(x);
            if (!match.Success) throw new Exception("not a match");
            return new Sensor15A(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }).ToArray();

        var limit = Read.Int() * 2;

        var matches = new List<Point>();
        for (int line = 0; line < limit; line++)
        {
            HashSet<int> readings = new();
            // HashSet<Point> bs = new();
            foreach (var s in sensors)
            {
                var dist = Math.Abs(s.L.X - s.B.X) + Math.Abs(s.L.Y - s.B.Y);

                var offset = dist - Math.Abs(s.L.Y - line);
                var start = s.L.X - offset;
                var end = s.L.X + offset;
                for (int i = start; i <= end; i++)
                {
                    readings.Add(i);
                }

                // if (s.B.Y == line) bs.Add(s.B);
            }

            for (var j = 0; j <= limit; j++)
                if (!readings.Contains(j)) matches.Add(new(j, line));
        }

        var match = matches.Single();
        var tune = 4000000;

        Console.WriteLine(match);
        Console.WriteLine(match.X * tune + match.Y);
    }
    private record struct Sensor15A(Point L, Point B);

    public static void Day15B()
    {
        var regex = new Regex(@"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$");
        var sensors = Read.StringBatch().Select(x =>
        {
            var match = regex.Match(x);
            if (!match.Success) throw new Exception("not a match");
            return new Sensor15B(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }).ToArray();

        var limit = Read.Int() * 2;

        var matches = new List<Point>();
        for (int line = 0; line < limit; line++)
        {
            List<Range> ranges = new List<Range>();
            foreach (var s in sensors)
            {
                var offset = s.Dist - Math.Abs(s.L.Y - line);
                if (offset <= 0) continue;

                var start = s.L.X - offset;
                var end = s.L.X + offset + 1;

                var @new = new Range(start, end);

                List<Range> newRanges = new List<Range>();
                foreach (var range in ranges)
                {
                    if (!range.Intersection(@new, out var merge))
                    {
                        newRanges.Add(range);
                        continue;
                    }

                    @new = merge;
                }

                newRanges.Add(@new);
                ranges = newRanges;
            }

            switch (ranges.Count)
            {
                case 1: if (ranges[0].Start > 0 || ranges[0].EndEx <= limit) throw new Exception("range error"); else continue;
                case 2: matches.Add(new(ranges[1].EndEx, line)); continue;
                default: throw new Exception("count is " + ranges.Count);
            }
        }

        var match = matches.Single();
        var tune = 4000000L;

        Console.WriteLine(match);
        Console.WriteLine(match.X * tune + match.Y);
    }
    private struct Sensor15B
    {
        public Sensor15B(Point l, Point b)
        {
            L = l;
            B = b;
            Dist = Math.Abs(L.X - B.X) + Math.Abs(L.Y - B.Y);
        }
        public Point L { get; }
        public Point B { get; }
        public int Dist { get; }
    }

    public static void XDay16A() // not working
    {
        // Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
        var regex = new Regex(@"^Valve (\w+) has flow rate=(\d+); tunnel(s)? lead(s)? to valve(s)? (\w+(, \w+)*)$");
        var valves = Read.StringBatch().Select(Parse).ToDictionary(x => x.Key);

        foreach (var valve in valves) Console.WriteLine(valve);

        var max = 0;
        Dfs(valves["AA"], 30, 0, string.Empty);

        Console.WriteLine(max);

        void Dfs(Valve v, int remaining, int total, string path)
        {
            path += " " + v.Key;

            // if (!v.ShouldVisit) return;
            for (int i = path.Length / 3 / 2; i > 0; i--)
            {
                if (path.Substring(path.Length - i * 3, i * 3) == path.Substring(path.Length - (i * 2) * 3, i * 3))
                    return;
            }

            if (remaining < 3)
            {
                if (total > max) max = total;
                return;
            }

            //v.ShouldVisit = false;

            foreach (var w in v.GetLinks(valves))
            {
                // open
                if (remaining > 2 && w.Rate > 0 && !path.Contains(w.Key))
                {
                    // w.IsOpen = true;
                    Dfs(w, remaining - 2, total += (remaining - 2) * v.Rate, path);
                }

                // do not open skip
                Dfs(w, remaining - 1, total, path);
            }

            //v.ShouldVisit = true;
            // v.IsOpen = false;
        }

        Valve Parse(string input)
        {
            var match = regex.Match(input);
            if (!match.Success) throw new Exception("no match");

            return new Valve(match.Groups[1].Value, int.Parse(match.Groups[2].Value), match.Groups[6].Value.Split(", "));
        }
    }
    private record class Valve(string Key, int Rate, string[] Links)
    {
        //public bool ShouldVisit { get; set; } = true;
        // public bool IsOpen { get; set; }

        public IEnumerable<Valve> GetLinks(Dictionary<string, Valve> valves) => Links.Select(x => valves[x])/*.Where(x => x.ShouldVisit)*/;
    }

    public static void Current() // XDay17A() // not working
    {
        var rocks = new Shape[]
        {
            new Shape(new Point[] { new(0, 0), new(1, 0), new(2, 0), new(3, 0),            }, 1, '-'), // --
            new Shape(new Point[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1, 2), }, 3, '+'), // +
            new Shape(new Point[] { new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2), }, 3, 'L'), // _|
            new Shape(new Point[] { new(0, 0), new(0, 1), new(0, 2), new(0, 3),            }, 4, '|'), // |
            new Shape(new Point[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1),            }, 2, 'O'), // [] □
        };

        var moves = Read.Line();
        // var chamber = new List<bool[]>();
        const int drawH = 50;
        const int width = 7;
        // var chamber = Enumerable.Repeat(0, drawH).Select(_ => new bool[width]).ToList(); Console.Clear();
        var chamber = Enumerable.Repeat(0, drawH).Select(_ => new char[width]).ToList(); Console.Clear();
        var iteration = 0;
        var maxH = 0;

        const int Steps = 2022;
        for (int i = 0; i < Steps; i++)
        {
            var next = rocks[i % 5];
            var reqiredHeight = maxH + 3 + next.Height;

            for (int h = chamber.Count; h < reqiredHeight; h++)
            {
                chamber.Add(new char[width]);
            }

            var position = Move(next.Points, 2, maxH + 3);
            Print();

            while (true)
            {
                var direction = moves[iteration++ % moves.Length];

                var newPosition = Move(position, direction == '<' ? -1 : 1, 0);

                if (!WillHit()) position = newPosition;
                Print();

                newPosition = Move(position, 0, -1);

                if (WillHit()) break;
                position = newPosition;
                Print();

                bool WillHit()
                {
                    return newPosition.Any(p => p.X < 0 || p.X >= width || p.Y < 0 || chamber[p.Y][p.X] != 0);
                }
            }

            // store
            foreach (var p in position) chamber[p.Y][p.X] = chamber[p.Y][p.X] == 0 ? next.C : throw new Exception("what");
            position = Array.Empty<Point>(); Print();

            // max rock
            for (int h = chamber.Count - 1; h >= 0; h--)
            {
                if (chamber[h].Any(x => x != 0))
                {
                    maxH = h + 1;
                    break;
                }
            }

            void Print()
            {
                if (i > 100 && i % 100 != 0 && i < 2010)
                    return;

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < drawH; i++)
                {
                    var y = chamber.Count - 1 - i;
                    Console.WriteLine($"|{new string(chamber[y].Select((b, x) => position.Contains(new Point(x, y)) ? '@' : b != 0 ? b : '.').ToArray())}|");
                }

                Console.WriteLine("+-------+");
                Console.WriteLine();
                Console.WriteLine($"{i + 1}: {maxH}");
                Thread.Sleep(10);
            }
        }

        // 3174 3173 3172 3171  3141 ?? 3109
        Console.WriteLine($"{Steps}: {maxH}");

        static Point[] Move(Point[] points, int dx, int dy)
        {
            return points.Select(p => p.Move(dx, dy)).ToArray();
        }
    }
    private record class Shape(Point[] Points, int Height, char C);

    public static void Day18A()
    {
        int totalSurface = 0;

        var dots = new HashSet<Point3>();
        foreach (var line in Read.StringBatch())
        {
            var input = line.Split(',').Select(int.Parse).ToArray();
            var p = new Point3(input[0], input[1], input[2]);
            dots.Add(p);

            Merge(dx: 1);
            Merge(dx: -1);
            Merge(dy: 1);
            Merge(dy: -1);
            Merge(dz: 1);
            Merge(dz: -1);

            void Merge(int dx = 0, int dy = 0, int dz = 0)
            {
                totalSurface += dots.Contains(p.Move(dx, dy, dz)) ? -1 : 1;
            }
        }

        Console.WriteLine(totalSurface);
    }
    public static void Current18() // Day18A()
    {
        int totalSurface = 0;

        var dots = new HashSet<Point3>();
        var lavas = new Dictionary<Point3, Lava>();
        foreach (var line in Read.StringBatch())
        {
            var input = line.Split(',').Select(int.Parse).ToArray();
            var p = new Point3(input[0], input[1], input[2]);
            var lava = new Lava(new() { p, });

            dots.Add(p);
            lavas.Add(p, lava);

            Merge(dx: 1);
            Merge(dx: -1);
            Merge(dy: 1);
            Merge(dy: -1);
            Merge(dz: 1);
            Merge(dz: -1);

            void Merge(int dx = 0, int dy = 0, int dz = 0)
            {
                totalSurface += dots.Contains(p.Move(dx, dy, dz)) ? -1 : 1;

                if (lavas.TryGetValue(p.Move(dx, dy, dz), out var neighbour))
                {
                    neighbour.Dots.AddRange(lava.Dots);
                    lava = neighbour;

                    foreach (var nDots in lava.Dots) lavas[nDots] = lava;
                }
            }
        }

        var distincs = lavas.Values.Distinct().ToArray();



        foreach (var dist in distincs)
        {

        }

        Console.WriteLine(totalSurface);
    }
    private record class Lava(List<Point3> Dots);

    public static void CurrentSample() // Day1xA()
    {
        var max = 0L;
        while (true)
        {
            var sum = Read.LongBatch().Sum();
            if (sum == 0) { break; }

            if (sum > max) { max = sum; }
        }
        Console.WriteLine(max);

        var all = new List<long>();
        foreach (var batch in Read.LongBatches())
        {
            var sum = batch.Sum();
            if (sum == 0) { break; }

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

    private record struct Point(int X, int Y)
    {
        public Point Move(int dx, int dy) => new(X + dx, Y + dy);
    }

    private record struct Point3(int X, int Y, int Z)
    {
        public Point3 Move(int dx, int dy, int dz) => new(X + dx, Y + dy, Z + dz);
    }

    private record struct Range(int Start, int EndEx)
    {
        public bool Intersection(Range other, out Range result)
        {
            if (
                (other.Start >= Start && other.Start <= EndEx)
                ||
                (other.EndEx >= Start && other.EndEx <= EndEx)
                ||
                (Start >= other.Start && Start <= other.EndEx)
                ||
                (EndEx >= other.Start && EndEx <= other.EndEx))
            {
                result = new(Math.Min(Start, other.Start), Math.Max(EndEx, other.EndEx));
                return true;
            }

            result = default;
            return false;
        }
    }
}