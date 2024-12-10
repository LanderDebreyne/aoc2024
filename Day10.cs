namespace AdventOfCode
{
    public class Day10 : BaseDayInputAsStringArray<int>
    {
        private readonly List<(int x, int y)> _possibleStarts = [];
        private readonly Dictionary<(int sx, int sy), List<(int x, int y)>> _trailheads = [];
        private readonly int[,] _map;
        private readonly int _n;

        public Day10() : base(nameof(Day10))
        {
            _n = Input.Length;
            _map = new int[_n, _n];
        }
        public override int SolvePart1()
        {
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    int height = int.Parse($"{Input[i][j]}");
                    if (height == 0)
                    {
                        _possibleStarts.Add((i, j));
                    }
                    _map[i, j] = height;
                }
            }
            int trailheads = 0;
            foreach (var (x, y) in _possibleStarts)
            {
                trailheads += Trailheads(x, y, 0, (x, y));
            }
            return trailheads;
        }

        public override int SolvePart2()
        {
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    int height = int.Parse($"{Input[i][j]}");
                    if (height == 0)
                    {
                        _possibleStarts.Add((i, j));
                    }
                    _map[i, j] = height;
                }
            }
            int trailheads = 0;
            foreach (var (x, y) in _possibleStarts)
            {
                trailheads += TrailheadRating(x, y, 0);
            }
            return trailheads;
        }

        private int TrailheadRating(int x, int y, int height)
        {
            if (x < 0 || x >= _n || y < 0 || y >= _n)
            {
                return 0;
            }
            if (_map[x, y] == height)
            {
                if (height == 9)
                {
                    return 1;
                }
                return
                TrailheadRating(x + 1, y, height + 1) +
                TrailheadRating(x - 1, y, height + 1) +
                TrailheadRating(x, y + 1, height + 1) +
                TrailheadRating(x, y - 1, height + 1);
            }
            return 0;
        }

        private int Trailheads(int x, int y, int height, (int, int) start)
        {
            if (x < 0 || x >= _n || y < 0 || y >= _n)
            {
                return 0;
            }
            if (_map[x, y] == height)
            {
                if (height == 9)
                {
                    if(_trailheads.TryGetValue(start, out var trailHeads))
                    {
                        if (!trailHeads.Contains((x, y)))
                        {
                            trailHeads.Add((x, y));
                            return 1;
                        }
                        return 0;
                    }
                    else
                    {
                        _trailheads.Add(start, [(x, y)]);
                        return 1;
                    }
                } 
                return
                Trailheads(x + 1, y, height+1, start) +
                Trailheads(x - 1, y, height+1, start) +
                Trailheads(x, y + 1, height+1, start) +
                Trailheads(x, y - 1, height+1, start);
            }
            return 0;
        }
    }
}
