using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoCCore;

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
        while ((line = Console.ReadLine()).Length > 0)
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

        Console.ReadLine();

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

        Console.ReadLine();

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
        var line = Console.ReadLine();

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
        var line = Console.ReadLine();

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

    public static void Current() // Day10A()
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
        Console.WriteLine(new string(lines.Select(Enumerable.First).ToArray()));
    }

    public static void CurrentSample()
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
        Console.WriteLine(new string(lines.Select(Enumerable.First).ToArray()));
    }
}