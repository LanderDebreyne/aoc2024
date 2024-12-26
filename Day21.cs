namespace AdventOfCode
{ 
    public class Day21 : BaseDayInputInLines<long>
    {
        private readonly char[][] _numKeypad =
        [
            ['7', '8', '9'],
            ['4', '5', '6'],
            ['1', '2', '3'],
            [' ', '0', 'A']
        ];

        private readonly char[][] _directionKeypad =
        [
            [' ', '^', 'A'],
            ['<', 'v', '>']
        ];


        private readonly Dictionary<char, (int, int)> _keypad = new ()
        {
            { '7', (0, 0) },
            { '8', (0, 1) },
            { '9', (0, 2) },
            { '4', (1, 0) },
            { '5', (1, 1) },
            { '6', (1 , 2) },
            { '1', (2 , 0) },
            { '2', (2 , 1) },
            { '3', (2, 2) },
            { '0', (3 , 1)},
            { 'A', (3 , 2)}
        };

        private readonly Dictionary<char, (int, int)> _directions = new()
        {
            { '^', (0 , 1) },
            { 'A', (0 , 2)},
            { '<', (1 , 0) },
            { 'v', (1, 1) },
            { '>', (1, 2) }
        };

        private readonly Dictionary<(char, char), List<string>> _numpadSequences = [];
        private readonly Dictionary<(char, char), List<string>> _directionSequences = [];

        private readonly Dictionary<(string, int), long> _memo = [];

        public Day21() : base(nameof(Day21))
        {
            _numpadSequences = CalculateSequences(_keypad, _numKeypad);
            _directionSequences = CalculateSequences(_directions, _directionKeypad);
        }

        private static Dictionary<(char, char), List<string>> CalculateSequences(Dictionary<char, (int, int)> keypad, char[][] keys)
        {
            Dictionary<(char, char), List<string>> sequences = [];
            for (int i = 0; i < keypad.Count; i++)
            {
                for (int j = 0; j < keypad.Count; j++)
                {
                    if (i == j)
                    {
                        sequences.Add((keypad.ElementAt(i).Key, keypad.ElementAt(j).Key), ["A"]);
                        continue;
                    }
                    List<string> possibilities = [];

                    Queue<(int, int, string)> q = new();
                    q.Enqueue((keypad.ElementAt(i).Value.Item1, keypad.ElementAt(i).Value.Item2, ""));
                    int optimal = int.MaxValue;
                    while (q.Count > 0)
                    {
                        (int r, int c, string moves) = q.Dequeue();
                        bool b = false;
                        foreach ((int nr, int nc, string nm) in new (int, int, string)[] { (r - 1, c, "^"), (r + 1, c, "v"), (r, c - 1, "<"), (r, c + 1, ">") })
                        {
                            if (nr < 0 || nc < 0 || nr >= keys.Length || nc >= keys[0].Length) continue;
                            if (keys[nr][nc] == ' ') continue;
                            if (keys[nr][nc] == keypad.ElementAt(j).Key)
                            {
                                if (optimal < moves.Length + 1)
                                {
                                    b = true;
                                    break;
                                }
                                optimal = moves.Length + 1;
                                possibilities.Add(moves + nm + "A");
                            }
                            else
                            {
                                q.Enqueue((nr, nc, moves + nm));
                            }
                        }
                        if (b) break;
                    }
                    sequences.Add((keypad.ElementAt(i).Key, keypad.ElementAt(j).Key), possibilities);
                }
            }
            return sequences;
        }
        public override long SolvePart1() => Solve(2);

        public override long SolvePart2() => Solve(25);

        private long CalculateLength(string x, int depth)
        {
            if (_memo.ContainsKey((x, depth)))
            {
                return _memo[(x, depth)];
            }
            if (depth == 0)
            {
                return x.Length;
            }
            long length = 0;
            foreach (var (c, d) in ("A" + x).Zip(x))
            {
                length += _directionSequences[(c,d)].Select(y => CalculateLength(y, depth - 1)).Min();
            }
            _memo[(x, depth)] = length;
            return length;
        }

        private long Solve(int depth)
        {
            long result = 0;
            foreach (var line in Input)
            {
                var f = NumericKeypadInputs(line);
                long length = f.Select(s => CalculateLength(s, depth)).Min();
                result += length * long.Parse(line[..^1]);
            }
            return result;
        }

        private List<string> NumericKeypadInputs(string line)
        {
            var options = new List<List<string>>();
            var t = ("A" + line).Zip(line, (f, s) => (f, s));
            foreach (var (f, s) in t)
            {
                options.Add(_numpadSequences[(f, s)]);
            }

            var product = CartesianProduct(options).Select(x => x.ToList()).ToList();
            return product.Select(arr => string.Join("", arr)).ToList();

            IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
            {
                IEnumerable<IEnumerable<T>> emptyProduct = [[]];
                return sequences.Aggregate(emptyProduct, (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat([item]));
            }
        }

        
    }
}
