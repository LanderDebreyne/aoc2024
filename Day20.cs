namespace AdventOfCode
{
    public class Day20: BaseDayInputInLines<long>
    {
        private readonly bool[,] _original;
        private readonly long[,] _costsStart;
        private readonly long[,] _costsEnd;
        private bool[,] _map;
        private readonly int _w;
        private readonly int _h;
        private (int, int) _start;
        private (int, int) _end;
        private readonly (int, int) _originalStart;
        private readonly (int, int) _originalEnd;
        private readonly HashSet<(int, int, int, int)> _cheated = [];
        private readonly PriorityQueue<(int, int, int), int> _pq = new();

        public Day20() : base(nameof(Day20))
        {
            _w = Input.First().Length;
            _h = Input.Count();
            _original = new bool[_w, _h];
            int y = 0;
            foreach (var line in Input)
            {
                for (int x = 0; x < _w; x++)
                {
                    _original[x, y] = line[x] == '#';
                    if (line[x] == 'S')
                    {
                        _originalStart = (x, y);
                    }
                    else if (line[x] == 'E')
                    {
                        _originalEnd = (x, y);
                    }
                }
                y++;
            }

            _map = _original.Clone() as bool[,] ?? throw new Exception("Sanity");
            _costsStart = new long[_w, _h];
            _costsEnd = new long[_w, _h];
            _start = _originalStart;
            _end = _originalEnd;
        }
        public override long SolvePart1()
        {
            long originalPath = FindShortestPath() ?? throw new Exception("Sanity");
            List<long> cheats = [];
            for (int x = 1; x < _w-1; x++)
            {
                for (int y = 1; y < _h-1; y++)
                {
                    if (!_original[x, y])
                    {
                        continue;
                    }
                    _map = _original.Clone() as bool[,] ?? throw new Exception("Sanity");
                    _map[x, y] = false;
                    long path = FindShortestPath() ?? throw new Exception("Sanity");
                    if (path > 0 && path < originalPath)
                    {
                        cheats.Add(path);
                    }
                }
            }
            cheats = cheats.Select(x => originalPath - x).ToList();
            cheats.Sort();
            var cheatCounts = cheats.GroupBy(x => x);
            long result = 0;
            foreach (var pair in cheatCounts)
            {
                if (pair.Key >= 100) result += pair.Count();
            }
            return result;
        }

        public override long SolvePart2()
        {
            long originalPath = FindShortestPath() ?? throw new Exception("Sanity");

            for (int x = 0; x < _w; x++)
            {
                for (int y = 0; y < _h; y++)
                {
                    if (_original[x, y])
                    {
                        continue;
                    }
                    _start = _originalStart;
                    _end = (x, y);
                    _map = _original.Clone() as bool[,] ?? throw new Exception("Sanity");
                    _costsStart[x, y] = FindShortestPath() ?? 1000000000000000;
                    _start = (x, y);
                    _end = _originalEnd;
                    _map = _original.Clone() as bool[,] ?? throw new Exception("Sanity");
                    _costsEnd[x, y] = FindShortestPath() ?? 1000000000000000;
                    if(_originalEnd.Equals((x,y))) _costsEnd[x, y] = 0;
                    if(_originalStart.Equals((x, y))) _costsStart[x, y] = 0;
                }
            }
            _map = _original.Clone() as bool[,] ?? throw new Exception("Sanity");

            List<long> cheats = GetCheats(originalPath);
            cheats.Sort();
            var cheatCounts = cheats.GroupBy(x => x);
            long result = 0;
            foreach (var pair in cheatCounts)
            {
                if (pair.Key >= 100) result += pair.Count();
            }
            return result;
        }

        private List<long> GetCheats(long originalCost)
        {
            List<long> cheats = [];
            for (int x = 0; x < _w; x++)
            {
                for (int y = 0; y < _h; y++)
                {
                    if (_original[x, y] || _costsStart[x,y] > originalCost)
                    {
                        continue;
                    }
                    foreach (int r in Enumerable.Range(2, 19))
                    {
                        foreach (int dr in Enumerable.Range(0, r+1))
                        {
                            int dc = r - dr;
                            List<(int, int)> next = [(x + dr, y + dc), (x - dr, y + dc), (x + dr, y - dc), (x - dr, y - dc)];
                            foreach ((int nX, int nY) in next)
                            {
                                if (nX < 0 || nX >= _w || nY < 0 || nY >= _h || _original[nX, nY])
                                {
                                    continue;
                                }
                                long newCost = _costsStart[x, y] + r + _costsEnd[nX, nY];
                                if (!_cheated.Contains((x,y,nX,nY)) && newCost < originalCost)
                                {
                                    cheats.Add(originalCost - newCost);
                                    _cheated.Add((x, y, nX, nY));
                                }
                            }
                        }
                    }
                }
            }
            return cheats;
        }

        private long? FindShortestPath()
        {
            _pq.Clear();
            _pq.Enqueue((0, _start.Item1, _start.Item2), 0);

            while (_pq.Count > 0)
            {
                var (cost, posX, posY) = _pq.Dequeue();
                if (_map[posX, posY]) continue;
                _map[posX, posY] = true;

                List<(int, int)> nextPos = [(posX + 1, posY),
                    (posX - 1, posY),
                    (posX, posY + 1),
                    (posX, posY - 1)];
                foreach ((int x, int y) in nextPos)
                {
                    if (x < 0 || x >= _w || y < 0 || y >= _h)
                    {
                        continue;
                    }
                    else if (_map[x, y])
                    {
                        continue;
                    }
                    else if ((x, y) == _end)
                    {
                        return (cost + 1);
                    }
                    else
                    {
                        _pq.Enqueue((cost + 1, x, y), cost + 1);
                    }
                }
            }
            return null;
        }
    }
}
