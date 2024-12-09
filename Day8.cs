namespace AdventOfCode
{
    public class Day8 : BaseDayInputAsStringArray
    {
        private readonly int _n;
        private readonly bool[,] _antiNodes;
        private Dictionary<char, HashSet<Point>> _antennas;

        private struct Point
        {
            public int X;
            public int Y;
        }


        public Day8(): base(nameof(Day8))
        {
            _n = Input.Length;
            _antiNodes = new bool[_n, _n];
            _antennas = [];
        }

        public override long SolvePart1() => SolvePart(true);

        public override long SolvePart2() => SolvePart(false);

        private long SolvePart(bool part)
        {
            long r = 0;
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < Input.Length; j++)
                {
                    switch (Input[i][j])
                    {
                        case '.':
                            break;
                        default:
                            r += HandleAntenna(Input[i][j], i, j, part);
                            break;
                    }
                }
            }
            return r;
        }

        private long HandleAntenna(char c, int x, int y, bool part)
        {
            int antiNodes = 0;
            if (!_antennas.TryGetValue(c, out HashSet<Point>? positions))
            {
                _antennas[c] = [new Point() { X = x, Y = y }];
            }
            else
            {
                foreach (var position in positions)
                {
                    int dx = position.X - x;
                    int dy = position.Y - y;
                    Point antiNode1 = new() { X = x + (part ? -dx : 0), Y = y + (part ? -dy : 0) };
                    Point antiNode2 = new() { X = position.X + (part ? dx : 0), Y = position.Y + (part ? dy : 0) };
                    while (InMap(antiNode1.X, antiNode1.Y))
                    {
                        if(!_antiNodes[antiNode1.X, antiNode1.Y])
                        {
                            _antiNodes[antiNode1.X, antiNode1.Y] = true;
                            antiNodes++;
                        }
                        if (part) break;
                        antiNode1.X += -dx;
                        antiNode1.Y += -dy;
                    }
                    while (InMap(antiNode2.X, antiNode2.Y))
                    {
                        if(!_antiNodes[antiNode2.X, antiNode2.Y])
                        {
                            _antiNodes[antiNode2.X, antiNode2.Y] = true;
                            antiNodes++;
                        }
                        if (part) break;
                        antiNode2.X += dx;
                        antiNode2.Y += dy;
                    }
                }
                positions.Add(new() { X = x, Y = y });
            }
            return antiNodes;
        }

        private bool InMap(int x, int y)
        {
            return x >= 0 && x < Input.Length && y >= 0 && y < Input.Length;
        }
    }
}
