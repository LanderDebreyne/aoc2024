namespace AdventOfCode
{
    enum State
    {
        Obstacle,
        Unvisited,
        Visited
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Day6() : BaseDayInputAsStringArray(nameof(Day6))
    {
        public override int SolvePart1()
        {
            var (map, guard) = InitMap();
            return FillMap(map, guard).Visited;
        }

        public override int SolvePart2()
        {
            var (map, guard) = InitMap();
            var start = guard;

            map = FillMap(map, guard).Map;

            // brute force
            int r = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = 0; j < Input[0].Length; j++)
                {
                    if (map[(i,j)] == State.Visited && (i,j) != start)
                    {
                        map[(i,j)] = State.Obstacle;
                        if(IsLoop(start, map)) r++;
                        map[(i, j)] = State.Visited;
                    }
                }
            }

            return r;
        }

        private (Dictionary<(int, int), State> Map, int Visited) FillMap(Dictionary<(int, int), State> map, (int, int) guard)
        {
            var dir = Direction.Up;
            int visited = 1;
            while (true)
            {
                var nextPos = Move(guard, dir);
                if (!InMap(nextPos))
                {
                    return (map, visited);
                }
                if (map[nextPos] == State.Obstacle)
                {
                    dir = NextDir(dir);
                }
                else
                {
                    guard = nextPos;
                    if (map[nextPos] == State.Unvisited)
                    {
                        map[nextPos] = State.Visited;
                        visited++;
                    }
                }
            }
        }

        private (Dictionary<(int,int), State> map, (int, int) guard) InitMap()
        {
            Dictionary<(int, int), State> map = [];
            var guard = (0,0);
            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = 0; j < Input[0].Length; j++)
                {
                    if (Input[i][j] == '^')
                    {
                        guard = (i, j);
                        map.Add((i, j), State.Visited);
                    }
                    else if (Input[i][j] == '#') map.Add((i, j), State.Obstacle);
                    else map.Add((i, j), State.Unvisited);
                }
            }

            return (map, guard);
        }

        private bool IsLoop((int, int) guard, Dictionary<(int,int), State> map)
        {
            Direction dir = Direction.Up;
            int max = map.Count;
            while (max > 0)
            {
                var nextPos = Move(guard, dir);
                if (!InMap(nextPos))
                {
                    return false;
                }
                if (map[nextPos] == State.Obstacle)
                {
                    dir = NextDir(dir);
                }
                else
                {
                    guard = nextPos;
                }
                max--;
            }
            return true;
        }

        private bool InMap((int, int) guard)
        {
            return guard.Item1 >= 0 && guard.Item1 < Input.Length && guard.Item2 >= 0 && guard.Item2 < Input.Length;
        }

        private static (int, int) Move((int, int) guard, Direction dir)
        {
            return dir switch
            {
                Direction.Up => (guard.Item1 - 1, guard.Item2),
                Direction.Down => (guard.Item1 + 1, guard.Item2),
                Direction.Left => (guard.Item1, guard.Item2 - 1),
                Direction.Right => (guard.Item1, guard.Item2 + 1),
                _ => throw new NotImplementedException(),
            };
        }

        private static Direction NextDir(Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
