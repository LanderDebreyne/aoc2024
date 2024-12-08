using System;

namespace AdventOfCode
{
    public class Day6: BaseDayInputAsStringArray
    {
        private readonly bool[,] _obstacleMap;
        private readonly bool[,] _visitedMap;
        private (int, int) _guard;

        public Day6() : base(nameof(Day6)) 
        {
            _obstacleMap = new bool[Input.Length, Input[0].Length];
            _visitedMap = new bool[Input.Length, Input[0].Length];
        }


        public override int SolvePart1()
        {
            InitMap();
            return FillMap();
        }

        public override int SolvePart2()
        {
            InitMap();
            var start = (_guard.Item1, _guard.Item2);

            FillMap();

            int r = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = 0; j < Input[0].Length; j++)
                {
                    if (_visitedMap[i,j] && (i,j) != start)
                    {
                        _obstacleMap[i,j] = true;
                        if(IsLoop(start)) r++;
                        _obstacleMap[i, j] = false;
                    }
                }
            }

            return r;
        }

        private int FillMap()
        {
            int dirX = 0;
            int dirY = -1;
            int visited = 1;
            int height = Input.Length;
            int width = Input[0].Length;
            while (true)
            {
                var nextPos = (_guard.Item1 + dirY, _guard.Item2 + dirX);
                if (!(nextPos.Item1 >= 0 && nextPos.Item1 < height && nextPos.Item2 >= 0 && nextPos.Item2 < width))
                {
                    return visited;
                }
                if (_obstacleMap[nextPos.Item1, nextPos.Item2])
                {
                    (dirX, dirY) = NextDir(dirX, dirY);
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
            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = 0; j < Input[0].Length; j++)
                {
                    switch (Input[i][j])
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

        private bool IsLoop((int, int) guard)
        {
            int dirX = 0;
            int dirY = -1;
            var visitedStates = new HashSet<(int, int, int, int)>();
            int height = Input.Length;
            int width = Input[0].Length;
            int max = height*width;
            while (max > 0)
            {
                var nextPos = (guard.Item1+dirY, guard.Item2+dirX);
                if (!(nextPos.Item1 >= 0 && nextPos.Item1 < height && nextPos.Item2 >= 0 && nextPos.Item2 < width))
                {
                    return false;
                }
                if (_obstacleMap[nextPos.Item1, nextPos.Item2])
                {
                    (dirX, dirY) = NextDir(dirX,dirY);
                }
                else
                {
                    guard = nextPos;
                    var state = (guard.Item1, guard.Item2, dirX, dirY);
                    if (!visitedStates.Add(state))
                    {
                        return true;
                    }
                }
                max--;
            }
            return true;
        }

        private static (int, int) NextDir(int dirX, int dirY)
        {
            return (dirX, dirY) switch
            {
                (0,-1) => (1,0),
                (0,1) => (-1,0),
                (-1,0) => (0,-1),
                (1,0) => (0,1),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
