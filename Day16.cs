using System.Runtime.InteropServices;

namespace AdventOfCode
{
    public class Day16 : BaseDayInputAsStringArray<long>
    {
        private readonly long _w;
        private readonly long _h;
        private readonly bool[,] _map;
        private readonly PriorityQueue<(int, int, int, int), int> _pq = new();
        private (int, int) _start;
        private (int, int) _end;
        private readonly (int, int)[] _dirs = [ (0, 1), (1, 0), (0, -1), (-1, 0)];
        private long _best = long.MaxValue;
        public Day16() : base(nameof(Day16))
        {
            _w = Input[0].Length;
            _h = Input.Length;
            _map = new bool[_w, _h];
        }
        public override long SolvePart1()
        {
            HashSet<(int, int, int)> visited = [];

            for (int j = 0; j < _h; j++)
            {
                string line = Input[j];
                for(int i = 0; i < _h; i++)
                {
                    if (line[i] == '.')
                    {
                        _map[i, j] = false;
                    }
                    else if (line[i] == '#')
                    {
                        _map[i, j] = true;
                    }
                    else if (line[i] == 'S')
                    {
                        _map[i, j] = false;
                        _start = (i, j);
                    }
                    else
                    {
                        _map[i, j] = false;
                        _end = (i, j);
                    }
                }
            }

            visited.Add((_start.Item1, _start.Item2, 1));
            _pq.Enqueue((0, _start.Item1, _start.Item2, 1), 0);
            while(_pq.Count > 0)
            {
                var(cost, posX, posY, dir) = _pq.Dequeue();
                if ((posX, posY) == _end)
                {
                    return cost;
                }
                visited.Add((posX, posY, dir));
                List<(int, int, int, int)> nextPos = [(1 + cost, posX + _dirs[dir].Item1, posY + _dirs[dir].Item2, dir), (1000 + cost, posX, posY, (dir + 1) % 4), (1000 + cost, posX, posY, (dir + 3) % 4)];
                foreach ((int c,int x,int y,int d) in nextPos)
                {
                    if(_map[x, y] || x < 0 || x >= _w || y < 0 || y >= _h)
                    {
                        continue;
                    }
                    if (visited.Contains((x, y, d)))
                    {
                        continue;
                    }
                    _pq.Enqueue((c, x, y, d), c);
                }
            }
            return 0;
        }

        public override long SolvePart2()
        {
            Dictionary<(int, int, int), long> lowest = [];
            for (int j = 0; j < _h; j++)
            {
                string line = Input[j];
                for (int i = 0; i < _h; i++)
                {
                    if (line[i] == '.')
                    {
                        _map[i, j] = false;
                    }
                    else if (line[i] == '#')
                    {
                        _map[i, j] = true;
                    }
                    else if (line[i] == 'S')
                    {
                        _map[i, j] = false;
                        _start = (i, j);
                    }
                    else
                    {
                        _map[i, j] = false;
                        _end = (i, j);
                    }
                }
            }

            lowest[(_start.Item1, _start.Item2, 1)] = 0;
            Dictionary<(int, int, int), List<(int?, int?, int?)>> bt = [];
            _pq.Enqueue((0, _start.Item1, _start.Item2, 1), 0);

            List<(int,int,int)> ends = [];
            while (_pq.Count > 0)
            {
                var (cost, posX, posY, dir) = _pq.Dequeue();
                if (cost > lowest.GetValueOrDefault((posX, posY, dir), long.MaxValue))
                {
                    continue;
                }
                if ((posX, posY) == _end)
                {
                    if (cost <= _best)
                    {
                        _best = cost;
                        ends.Add((posX, posY, dir));
                    }
                    else
                    {
                        break;
                    }
                }
                List<(int, int, int, int)> nextPos = [(1 + cost, posX + _dirs[dir].Item1, posY + _dirs[dir].Item2, dir), (1000 + cost, posX, posY, (dir + 1) % 4), (1000 + cost, posX, posY, (dir + 3) % 4)];
                foreach ((int c, int x, int y, int d) in nextPos)
                {
                    if (_map[x, y] || x < 0 || x >= _w || y < 0 || y >= _h)
                    {
                        continue;
                    }
                    long lowestCost = lowest.GetValueOrDefault((x, y, d), long.MaxValue);
                    if (c > lowestCost)
                    {
                        continue;
                    }
                    if (c < lowestCost)
                    {
                        bt[(x, y, d)] = [];
                        lowest[(x, y, d)] = c;
                    }
                    bt[(x, y, d)].Add((posX, posY, dir));
                    _pq.Enqueue((c, x, y, d), c);
                }
            }

            LinkedList<(int, int, int)> path = new();
            HashSet<(int?, int?, int?)> visited = [];
            path.AddFirst((ends[0].Item1, ends[0].Item2, ends[0].Item3));
            while (path.Count > 0)
            {
                var (x, y, d) = path.First!.Value;
                path.RemoveFirst();
                if (bt.ContainsKey((x, y, d)))
                {
                    foreach ((int?, int?, int?) b in bt[(x, y, d)])
                    {
                        var (lx, ly, ld) = b;
                        if (visited.Contains((lx, ly, ld)))
                        {
                            continue;
                        }
                        visited.Add((lx, ly, ld));
                        if (lx == null)
                        {
                            continue;
                        }
                        path.AddFirst((lx.Value, ly.Value, ld.Value));
                    }
                }
            } 
            var paths = visited.Select(x => (x.Item1, x.Item2)).ToHashSet();

            // Print map
            //int count = 0;
            //for (int j = 0; j < _h; j++)
            //{
            //    for (int i = 0; i < _w; i++)
            //    {
            //        if (_map[i, j])
            //        {
            //            Console.Write('#');
            //        }
            //        else if (paths.Contains((i, j)))
            //        {
            //            Console.Write("|");
            //            count++;
            //        }
            //        else
            //        {
            //            Console.Write('.');
            //        }
            //    }
            //    Console.WriteLine();
            //}

            //return count+1;

            return paths.Count + 1;
        }
    }
}
