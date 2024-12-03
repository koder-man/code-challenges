using System.Collections.Specialized;

namespace AoCCore;

public static class Y2020
{
    private static void P(string error)
    {
        Console.WriteLine(error);
    }

    private static int Exit(string msg)
    {
        msg.P();
        return 1;
    }

    public static void Day15a()
    {
        const int cnt = 2020;
        var numbers = Read.Line().Split(',').Select(int.Parse).ToList();
        numbers.Capacity = cnt;

        for (int i = numbers.Count; i < cnt; i++)
        {
            numbers.Add(0);

            for (int j = i - 2; j >= 0; j--)
            {
                if (numbers[j] == numbers[i - 1])
                {
                    numbers[i] = i - 1 - j;
                    break;
                }
            }
        }

        Console.WriteLine("2020: " + numbers.Last());
        // 2020: 1836
    }

    public static void Day15b()
    {
        const int cnt = 30000000;
        var input = Read.Line().Split(',').Select(int.Parse);
        var numbers = input.Select((i, index) => (i, index)).ToDictionary(i => i.i, i => (Last: 0, Index: i.index));
        int lastNumber = input.Last();

        for (int i = numbers.Count; i < cnt; i++)
        {
            lastNumber = numbers[lastNumber].Last;
            numbers[lastNumber] = (numbers.ContainsKey(lastNumber) ? (i - numbers[lastNumber].Index) : 0, i);
        }

        Console.WriteLine(cnt + ": " + lastNumber);
        // 2020: 421
        // 30000000: 436
    }

    class Day16a
    {
        class Field
        {
            public Field(string input)
            {
                var split = input.Split(": ");
                Description = split[0];
                split = split[1].Split(" or ");
                Range1 = new Range(split[0]);
                Range2 = new Range(split[1]);
            }

            public string Description;

            public bool InRange(int val)
            {
                return Range1.InRange(val) || Range2.InRange(val);
            }

            class Range
            {
                public Range(string input)
                {
                    var split = input.Split('-');
                    Start = int.Parse(split[0]);
                    End = int.Parse(split[1]);
                }
                public int Start;
                public int End;

                public bool InRange(int val)
                {
                    return val >= Start && val <= End;
                }
            }

            private Range Range1;
            private Range Range2;
        }

        public static void Current()
        {
            List<Field> fields = new List<Field>();

            string str;
            while ((str = Read.Line()).Length > 0)
            {
                fields.Add(new Field(str));
            }

            Read.Line(); // your ticket
            Read.Line(); // data
            Read.Line(); // empty line
            Read.Line(); // nerbay tickets

            int sum = 0;
            while ((str = Read.Line()).Length > 0)
            {
                sum += str.Split(',')
                  .Select(int.Parse)
                  .Where(x => !fields.Any(field => field.InRange(x)))
                  .Sum();
            }

            Console.WriteLine("sum: " + sum);
            // sum: 19087
        }
    }

    class Day16b
    {
        class Field
        {
            public Field(string input)
            {
                var split = input.Split(": ");
                Description = split[0];
                split = split[1].Split(" or ");
                Range1 = new Range(split[0]);
                Range2 = new Range(split[1]);
            }

            public string Description;

            public bool InRange(int val)
            {
                return Range1.InRange(val) || Range2.InRange(val);
            }

            public void InitIndexes(int count)
            {
                ValidIndexes = Enumerable.Range(0, count).ToHashSet();
            }
            public void ExtendRestriction(int index, int value)
            {
                if (ValidIndexes.Contains(index) && !InRange(value))
                    ValidIndexes.Remove(index);
            }
            public int ValidIndex()
            {
                if (ValidIndexes.Count != 1)
                {
                    return -1;
                }
                return ValidIndexes.Single();
            }

            public bool SetNotValid(int index)
            {
                return ValidIndexes.Remove(index);
            }

            class Range
            {
                public Range(string input)
                {
                    var split = input.Split('-');
                    Start = int.Parse(split[0]);
                    End = int.Parse(split[1]);
                }
                public int Start;
                public int End;

                public bool InRange(int val)
                {
                    return val >= Start && val <= End;
                }
            }

            private Range Range1;
            private Range Range2;
            private HashSet<int> ValidIndexes = null!;
        }

        public static void Current()
        {
            List<Field> fields = new List<Field>();

            string str;
            while ((str = Read.Line()).Length > 0)
            {
                fields.Add(new Field(str));
            }

            foreach (var field in fields)
            {
                field.InitIndexes(fields.Count);
            }

            Read.Line(); // your ticket
            var myData = Read.Line().Split(',').Select(int.Parse).ToArray();
            Read.Line(); // data
            Read.Line(); // empty line
            Read.Line(); // nerbay tickets

            while ((str = Read.Line()).Length > 0)
            {
                var nums = str.Split(',').Select(int.Parse).ToArray();
                if (nums.Any(x => !fields.Any(field => field.InRange(x))))
                    continue;

                foreach (var x in nums.SelectIndex()
                  //.SelectMany(x => fields.Where(f => f.ValidOn(x.Index)).Select(f => (x.Value, x.Index, Field: f)))
                  //.Where(x => x.Field.InRange(x.Value))
                  )
                {
                    fields.ForEach(f => f.ExtendRestriction(x.Index, x.Value));
                }
            }

            for (bool DoLoop = true; DoLoop;)
            {
                DoLoop = false;

                foreach (Field field in fields)
                {
                    var validIndex = field.ValidIndex();
                    if (validIndex != -1)
                    {
                        fields.ForEach(f =>
                        {
                            if (f != field)
                            {
                                DoLoop = f.SetNotValid(validIndex) || DoLoop;
                            }
                        });
                    }
                }
            }

            long prod = fields.Take(6).Aggregate(1L, (x, f) => x *= myData[f.ValidIndex()]);
            Console.WriteLine("prod: " + prod);
            // prod: 1382443095281
        }
    }

    public static void Day17a()
    {
        const int start = 8;
        const int iter = 6;
        const int dim = iter + start + iter;

        bool[,,] pocket = new bool[dim, dim, dim];

        for (int i = 0; i < start; i++)
        {
            foreach (var x in Read.Line().SelectIndex())
                pocket[iter + x.Index, iter + i, iter] = x.Value == '#';
        }

        Func<int, int, int, int> countNeibours = (i1, j1, k1) =>
        {
            int cnt = pocket[i1, j1, k1] ? -1 : 0;

            for (int i = Math.Max(0, i1 - 1); i < Math.Min(dim, i1 + 2); i++)
                for (int j = Math.Max(0, j1 - 1); j < Math.Min(dim, j1 + 2); j++)
                    for (int k = Math.Max(0, k1 - 1); k < Math.Min(dim, k1 + 2); k++)
                        if (pocket[i, j, k]) cnt++;

            return cnt;
        };

        for (int it = 0; it < iter; it++)
        {
            bool[,,] next = (bool[,,])pocket.Clone();

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        var cnt = countNeibours(i, j, k);
                        next[i, j, k] = (pocket[i, j, k] && cnt == 2) || cnt == 3;
                    }
                }
            }

            pocket = next;
        }

        var all = from i in Enumerable.Range(0, dim)
                  from j in Enumerable.Range(0, dim)
                  from k in Enumerable.Range(0, dim)
                  select pocket[i, j, k];
        int count = all.Count(x => x);

        Console.WriteLine("count: " + count);
        // count: 215
    }

    public static void Day17b()
    {
        const int start = 8;
        const int iter = 6;
        const int dim = iter + start + iter;

        bool[,,,] pocket = new bool[dim, dim, dim, dim];

        for (int i = 0; i < start; i++)
        {
            foreach (var x in Read.Line().SelectIndex())
                pocket[iter + x.Index, iter + i, iter, iter] = x.Value == '#';
        }

        Func<int, int, int, int, int> countNeibours = (i1, j1, k1, l1) =>
        {
            int cnt = pocket[i1, j1, k1, l1] ? -1 : 0;

            for (int i = Math.Max(0, i1 - 1); i < Math.Min(dim, i1 + 2); i++)
                for (int j = Math.Max(0, j1 - 1); j < Math.Min(dim, j1 + 2); j++)
                    for (int k = Math.Max(0, k1 - 1); k < Math.Min(dim, k1 + 2); k++)
                        for (int l = Math.Max(0, l1 - 1); l < Math.Min(dim, l1 + 2); l++)
                            if (pocket[i, j, k, l]) cnt++;

            return cnt;
        };

        for (int it = 0; it < iter; it++)
        {
            bool[,,,] next = (bool[,,,])pocket.Clone();

            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    for (int k = 0; k < dim; k++)
                        for (int l = 0; l < dim; l++)
                        {
                            var cnt = countNeibours(i, j, k, l);
                            next[i, j, k, l] = (pocket[i, j, k, l] && cnt == 2) || cnt == 3;
                        }

            pocket = next;
        }

        var all = from i in Enumerable.Range(0, dim)
                  from j in Enumerable.Range(0, dim)
                  from k in Enumerable.Range(0, dim)
                  from l in Enumerable.Range(0, dim)
                  select pocket[i, j, k, l];
        int count = all.Count(x => x);

        Console.WriteLine("count2: " + count);
        // count2: 1728
    }

    class Day18a
    {
        enum Operator
        {
            Undefined,
            Plus,
            Mult,
            // ParO,
            // ParC,
        }
        private static void Calculate(Stack<(long Value, Operator Operator)> Values)
        {
            var tmp1 = Values.Pop();
            var tmp0 = Values.Pop();
            switch (tmp0.Operator)
            {
                case Operator.Plus:
                    Values.Push((tmp0.Value + tmp1.Value, tmp1.Operator));
                    return;
                case Operator.Mult:
                    Values.Push((tmp0.Value * tmp1.Value, tmp1.Operator));
                    return;
                default:
                    Exit("aaaaa");
                    return;
            }
        }
        public static int Current()
        {
            long sum = 0;

            Stack<(long Value, Operator Operator)> Values = new();
            for (string str; (str = Read.Line()).Length > 0;)
            {
                Values.Push((0, Operator.Plus));

                foreach (char c in str)
                {
                    switch (c)
                    {
                        case ' ':
                            continue;
                        case '+':
                            Values.Push((Values.Pop().Value, Operator.Plus));
                            Calculate(Values);
                            continue;
                        case '*':
                            Values.Push((Values.Pop().Value, Operator.Mult));
                            Calculate(Values);
                            continue;
                        case '(':
                            Values.Push((0, Operator.Plus));
                            continue;
                        case ')':
                            if (Values.Peek().Operator != Operator.Undefined) return Exit("should be undefined");
                            Calculate(Values);
                            continue;
                        case char when char.IsDigit(c):
                            switch (Values.Peek().Operator)
                            {
                                case Operator.Undefined:
                                    Values.Push((Values.Pop().Value * 10 + (c - '0'), Operator.Undefined));
                                    continue;
                                case Operator.Plus:
                                case Operator.Mult:
                                    Values.Push((c - '0', Operator.Undefined));
                                    continue;
                                default:
                                    return Exit("opala");
                            }
                        default:
                            return Exit($"error: '{c}' ({str})");
                    }
                }

                Calculate(Values);

                if (Values.Count != 1)
                {
                    return Exit($"Too many elements: {Values.Count}");
                }

                P("tmpsum: " + Values.Peek().Value);
                sum += Values.Pop().Value;
            }

            P("sum: " + sum);
            // sum: 4491283311856

            return 1;
        }
    }

    static class Day18b
    {
        enum Operator
        {
            Undefined,
            Plus,
            Mult,
        }
        private static string ToStr(Operator op)
        {
            switch (op)
            {
                case Operator.Undefined:
                    return "";
                case Operator.Plus:
                    return "+ ";
                case Operator.Mult:
                    return "* ";
                default:
                    return Exit("asaa").ToString();
            }
        }

        interface IExpr
        {
            long Eval();
        }

        class ConstExp : IExpr
        {
            private long Value;
            public ConstExp(long value = 0)
            {
                Value = value;
            }
            public void Expand(long value)
            {
                Value = Value * 10 + value;
            }
            public long Eval()
            {
                return Value;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
        class Expr : IExpr
        {
            private readonly List<(Operator Oper, IExpr Expr)> Parts = new();

            public void Append(Operator @operator, IExpr expression)
            {
                Parts.Add((@operator, expression));
            }

            public long Eval()
            {
                for (int i = 1; i < Parts.Count;)
                {
                    if (Parts[i].Oper == Operator.Plus)
                    {
                        Parts[i - 1] = (Parts[i - 1].Oper, new ConstExp(Parts[i - 1].Expr.Eval() + Parts[i].Expr.Eval()));
                        Parts.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                for (int i = 1; i < Parts.Count;)
                {
                    if (Parts[i].Oper != Operator.Mult)
                    {
                        return Exit("invalid operator");
                    }

                    Parts[i - 1] = (Parts[i - 1].Oper, new ConstExp(Parts[i - 1].Expr.Eval() * Parts[i].Expr.Eval()));
                    Parts.RemoveAt(i);
                }

                return Parts.Single().Expr.Eval();
            }

            public override string ToString()
            {
                return $"({string.Join(' ', Parts.Select(part => $"{ToStr(part.Oper)}{part.Expr}"))})";
            }
        }
        public static int Current()
        {
            long sum = 0;
            Stack<Expr> exprs = new();

            for (string str; (str = Read.Line()).Length > 0;)
            {
                exprs.Push(new Expr());
                Operator oper = Operator.Undefined;
                ConstExp? constExpr = null;

                foreach (char c in str)
                {
                    if (char.IsDigit(c))
                    {
                        if (constExpr == null)
                        {
                            constExpr = new ConstExp();
                            exprs.Peek().Append(oper, constExpr);
                            oper = Operator.Undefined;
                        }

                        constExpr.Expand(c - '0');
                        continue;
                    }

                    constExpr = null;

                    switch (c)
                    {
                        case ' ':
                            continue;
                        case '+':
                        case '*':
                            if (oper != Operator.Undefined) return Exit("oper " + c);
                            oper = c == '+' ? Operator.Plus : Operator.Mult;
                            continue;
                        case '(':
                            var newExpr = new Expr();
                            exprs.Peek().Append(oper, newExpr);
                            oper = Operator.Undefined;
                            exprs.Push(newExpr);
                            continue;
                        case ')':
                            exprs.Pop();
                            continue;
                        default:
                            return Exit($"error: '{c}' ({str})");
                    }
                }

                if (exprs.Count > 1)
                {
                    return Exit("count");
                }

                P("tmpsum: " + exprs.Peek().Eval());
                sum += exprs.Pop().Eval();
            }

            P("sum: " + sum);
            // sum: 68852578641904

            return 1;
        }
    }

    class Day19a
    {
        interface IRule
        {
            string ToRegex(Dictionary<int, IRule> rules);
        }
        class CharRule : IRule
        {
            char Char;
            public CharRule(char c)
            {
                Char = c;
            }
            public string ToRegex(Dictionary<int, IRule> rules)
            {
                return Char.ToString();
            }
        }
        class RefRule : IRule
        {
            int RefId;
            public RefRule(int refId)
            {
                RefId = refId;
            }
            public string ToRegex(Dictionary<int, IRule> rules)
            {
                return rules[RefId].ToRegex(rules);
            }
        }
        class OptionRule : IRule
        {
            List<IRule> Rules = new();
            public void AppendRule(IRule rule)
            {
                Rules.Add(rule);
            }
            public string ToRegex(Dictionary<int, IRule> rules)
            {
                return $"(?:{string.Join('|', Rules.Select(r => r.ToRegex(rules)))})";
            }
        }
        class SeqRule : IRule
        {
            List<IRule> Rules = new();
            public void AppendRule(IRule rule)
            {
                Rules.Add(rule);
            }
            public string ToRegex(Dictionary<int, IRule> rules)
            {
                return string.Join("", Rules.Select(r => r.ToRegex(rules)));
            }
        }
        public static int Current()
        {
            Dictionary<int, IRule> rules = new();

            for (string str; (str = Read.Line()).Length > 0;)
            {
                int ind = str.IndexOf(':');
                int id = int.Parse(str.AsSpan(0, ind));
                IRule rule;

                if (str[ind + 2] == '"')
                {
                    rule = new CharRule(str[ind + 3]);
                }
                else
                {
                    var split = str.Substring(ind + 2).Split(" | ");
                    if (split.Length > 1)
                    {
                        OptionRule rule1 = new();
                        foreach (var seq in split)
                        {
                            rule1.AppendRule(ProcessSeq(seq));
                        }
                        rule = rule1;
                    }
                    else
                    {
                        rule = ProcessSeq(split[0]);
                    }

                    SeqRule ProcessSeq(string str)
                    {
                        SeqRule rule = new();
                        foreach (var id in str.Split(' '))
                        {
                            rule.AppendRule(new RefRule(int.Parse(id)));
                        }
                        return rule;
                    }
                }

                rules.Add(id, rule);
            }

            Regex regex = new Regex($"^{rules[0].ToRegex(rules)}$", RegexOptions.Compiled);

            int count = 0;
            for (string str; (str = Read.Line()).Length > 0;)
            {
                if (regex.IsMatch(str))
                    count++;
            }


            P("count: " + count);
            // count: 235

            return 1;
        }
    }

    class Day19b
    {
        interface IRule
        {
            string ToRegex(int myId, Dictionary<int, IRule> rules);
        }
        class CharRule : IRule
        {
            char Char;
            public CharRule(char c)
            {
                Char = c;
            }
            public string ToRegex(int myId, Dictionary<int, IRule> rules)
            {
                return Char.ToString();
            }
        }
        class RefRule : IRule
        {
            int RefId;
            public RefRule(int refId)
            {
                RefId = refId;
            }
            public string ToRegex(int myId, Dictionary<int, IRule> rules)
            {
                return rules[RefId].ToRegex(RefId, rules);
            }
        }
        class OptionRule : IRule
        {
            List<IRule> Rules = new();
            public void AppendRule(IRule rule)
            {
                Rules.Add(rule);
            }
            public string ToRegex(int myId, Dictionary<int, IRule> rules)
            {
                return $"(?:{string.Join('|', Rules.Select(r => r.ToRegex(myId, rules)))})";
            }
        }
        class SeqRule : IRule
        {
            List<IRule> Rules = new();
            public void AppendRule(IRule rule)
            {
                Rules.Add(rule);
            }
            public string ToRegex(int myId, Dictionary<int, IRule> rules)
            {
                if (myId == 8 || myId == 11)
                {
                    return $"{myId}";
                }

                return string.Join("", Rules.Select(r => r.ToRegex(myId, rules)));
            }
        }
        static string RepStr(string str, int count)
        {
            return string.Join("", Enumerable.Repeat(str, count));
        }
        public static int Current()
        {
            Dictionary<int, IRule> rules = new();

            for (string str; (str = Read.Line()).Length > 0;)
            {
                int ind = str.IndexOf(':');
                int id = int.Parse(str.AsSpan(0, ind));
                IRule rule;

                if (str[ind + 2] == '"')
                {
                    rule = new CharRule(str[ind + 3]);
                }
                else
                {
                    var split = str.Substring(ind + 2).Split(" | ");
                    if (split.Length > 1)
                    {
                        OptionRule rule1 = new();
                        foreach (var seq in split)
                        {
                            rule1.AppendRule(ProcessSeq(seq));
                        }
                        rule = rule1;
                    }
                    else
                    {
                        rule = ProcessSeq(split[0]);
                    }

                    SeqRule ProcessSeq(string str)
                    {
                        SeqRule rule = new();
                        foreach (var id in str.Split(' '))
                        {
                            rule.AppendRule(new RefRule(int.Parse(id)));
                        }
                        return rule;
                    }
                }

                rules.Add(id, rule);
            }

            string r42 = rules[42].ToRegex(42, rules);
            string r31 = rules[31].ToRegex(31, rules);

            const int Repeat = 10;
            var regexes = new Regex[Repeat * Repeat];
            for (int i = 0; i < Repeat; i++)
            {
                for (int j = 0; j < Repeat; j++)
                {
                    regexes[i * Repeat + j] = new Regex($"^{RepStr(r42, i + 1)}{RepStr(r42, j + 1)}{RepStr(r31, j + 1)}$", RegexOptions.Compiled);
                }
            }

            int count = 0;
            for (string str; (str = Read.Line()).Length > 0;)
            {
                if (regexes.Any(r => r.IsMatch(str)))
                    count++;
            }

            P("count: " + count);
            // count: 379

            return 1;
        }
    }

    const int Dim = 10;
    const int Top = 0;
    const int Left = 1;
    const int Bottom = 2;
    const int Right = 3;

    class TileEdge
    {
        public BitVector32 Bits = new();
        public List<Tile> Candidates = new();
    }
    class Tile
    {
        public Tile(int id)
        {
            Id = id;
            Edges = Enumerable.Repeat(0, 4).Select(_ => new TileEdge()).ToArray();

            for (int i = 0; i < Dim; i++)
            {
                var line = Read.Line();

                if (i == 0 || i == Dim - 1)
                {
                    for (int j = 0; j < Dim; j++)
                    {
                        Edges[i == 0 ? Top : Bottom].Bits[1 << j] = line[j] == '#';
                    }
                }

                Edges[Left].Bits[1 << i] = line.First() == '#';
                Edges[Right].Bits[1 << i] = line.Last() == '#';
            }
        }

        public readonly int Id;
        public readonly TileEdge[] Edges;
    }

    public static int Current()
    {
        List<Tile> tiles = new();

        for (string str; (str = Read.Line()).Length > 0;)
        {
            tiles.Add(new Tile(int.Parse(str.AsSpan(5, str.Length - 6))));

            Read.Line();
        }

        foreach (var tile in tiles)
        {
            foreach (var neibour in tiles)
            {
                for (int i = 0; i < 2; i++)
                {
                    CompareTo(i);
                }

                void CompareTo(int src)
                {
                    if (tile.Edges[src].Bits.Data == neibour.Edges[3 - src].Bits.Data)
                    {
                        tile.Edges[src].Candidates.Add(neibour);
                        neibour.Edges[3 - src].Candidates.Add(tile);
                    }
                }
            }
        }

        List<Tile> edges = new();

        bool exit = false;
        foreach (var tile in tiles)
        {
            var cnts = tile.Edges.Select(edge => (edge.Candidates.Count, Ids: string.Join(", ", edge.Candidates.Select(c => c.Id)))).ToArray();

            if (cnts.Any(edge => edge.Count > 1))
            {
                Exit($"too much for {tile.Id}: {string.Join(", ", cnts.Select(c => $"{c.Count} ({c.Ids})"))}");
                exit = true;
            }

            if (tile.Edges.Count(edge => edge.Candidates.Count == 0) == 2)
            {
                edges.Add(tile);
            }
        }

        if (exit)
        {
            return 1;
        }

        if (edges.Count != 4)
        {
            return Exit($"Cnt: {edges.Count}");
        }

        P("edges: " + string.Join(", ", edges.Select(e => e.Id)));
        P("prod: " + edges.Aggregate(1L, (i, t) => i *= t.Id));
        // prod: 19945942578259

        return 1;
    }
}