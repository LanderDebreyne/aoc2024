using System;

namespace AdventOfCode
{
    public class Day6: BaseDayInputAsStringArray<long>
    {
        private readonly int _n;
        private readonly bool[,] _obstacleMap;
        private readonly bool[,] _visitedMap;
        private (int, int) _guard;
        private (int, int) _start;
        private bool _loop = false;
        private readonly (int, int)[] _dirs = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        public Day6() : base(nameof(Day6)) 
        {
            _n = Input.Length;
            _obstacleMap = new bool[Input.Length, Input[0].Length];
            _visitedMap = new bool[Input.Length, Input[0].Length];
        }

        public override long SolvePart1()
        {
            InitMap();
            return FillMap();
        }

        public override long SolvePart2()
        {
            InitMap();
            _start = (_guard.Item1, _guard.Item2);

            FillMap();

            int r = 0;
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    if (_visitedMap[i,j] && !_start.Equals((i,j)))
                    {
                        _obstacleMap[i,j] = true;
                        CheckLoop();
                        if (_loop) r++;
                        _loop = false;
                        _obstacleMap[i, j] = false;
                    }
                }
            }

            return r;
        }

        private int FillMap()
        {
            int dir = 0;
            int visited = 1;
            while (true)
            {
                var nextPos = (_guard.Item1 + _dirs[dir].Item2, _guard.Item2 + _dirs[dir].Item1);
                if (!(nextPos.Item1 >= 0 && nextPos.Item1 < _n && nextPos.Item2 >= 0 && nextPos.Item2 < _n))
                {
                    return visited;
                }
                if (_obstacleMap[nextPos.Item1, nextPos.Item2])
                {
                    dir++;
                    dir %= 4;
                }
                else
                {
                    _guard = nextPos;
                    if (!_visitedMap[nextPos.Item1, nextPos.Item2])
                    {
                        _visitedMap[nextPos.Item1, nextPos.Item2] = true;
                        visited++;
                    }
                }
            }
        }

        private void InitMap()
        {
            Array.Clear(_obstacleMap, 0, _obstacleMap.Length);
            Array.Clear(_visitedMap, 0, _visitedMap.Length);
            for (int i = 0; i < _n; i++)
            {
                string line = Input[i];
                for (int j = 0; j < _n; j++)
                {
                    switch (line[j])
                    {
                        case '^':
                            _guard = (i, j);
                            _visitedMap[i, j] = true;
                            break;
                        case '#':
                            _obstacleMap[i, j] = true;
                            break;
                    }
                }
            }
            return;
        }

        private void CheckLoop()
        {
            (int, int) guard = _start;
            int dir = 0;
            var visitedStates = new HashSet<(int, int, int)>();
            while (true)
            {
                var nextPos = (guard.Item1 + _dirs[dir].Item2, guard.Item2 + _dirs[dir].Item1);
                if (!(nextPos.Item1 >= 0 && nextPos.Item1 < _n && nextPos.Item2 >= 0 && nextPos.Item2 < _n))
                {
                    return;
                }
                if (_obstacleMap[nextPos.Item1, nextPos.Item2])
                {
                    dir++;
                    dir %= 4;
                }
                else
                {
                    guard = nextPos;
                    var state = (guard.Item1, guard.Item2, dir);
                    if (!visitedStates.Add(state))
                    {
                        _loop = true;
                        return;
                    }
                }
            }
        }
    }
}
