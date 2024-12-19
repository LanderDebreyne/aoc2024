namespace AdventOfCode
{
    public class Day18() : BaseDayInputInLines<string>(nameof(Day18))
    {
        private readonly int _w = 71;
        private readonly int _h = 71;
        private (int X, int Y) _me = (0, 0);
        private (int X, int Y) _end = (70, 70);
        private readonly bool[,] _original = new bool[71, 71];
        private bool[,] _map = new bool[71,71];
        private readonly PriorityQueue<(int, int, int), int> _pq = new();

        public override string SolvePart1()
        {
            foreach (var line in Input.Take(1024))
            {
                var ints = line.Split(',').Select(int.Parse).ToList();
                var (x, y) = (ints[0], ints[1]);
                _map[x, y] = true;
            }
            
            _pq.Enqueue((0, 0, 0), 0);

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
                        return (cost + 1).ToString();
                    }
                    else
                    {
                        _pq.Enqueue((cost + 1, x, y), cost + 1);
                    }
                }
            }
            return "ERROR";
        }
        public override string SolvePart2()
        {
            int skip = 1025;

            foreach (var line in Input.Take(skip))
            {
                var ints = line.Split(',').Select(int.Parse).ToList();
                var (x, y) = (ints[0], ints[1]);
                _original[x, y] = true;
            }
            bool found = true;
            while (found)
            {
                _pq.Clear();
                _pq.Enqueue((0, 0, 0), 0);
                found = false;

                var ints = Input.Skip(skip).First().Split(',').Select(int.Parse).ToList();
                var (nx, ny) = (ints[0], ints[1]);
                _original[nx, ny] = true;

                _map = (bool[,])_original.Clone();

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
                            found = true;
                            break;
                        }
                        else
                        {
                            _pq.Enqueue((cost + 1, x, y), cost + 1);
                        }
                    }
                    if (found) break;
                }
                skip++;
            }
            return Input.Skip(skip - 1).First();
        }
    }
}
