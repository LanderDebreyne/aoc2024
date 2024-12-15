namespace AdventOfCode
{
    public class Day15 : BaseDayInputInLines<long>
    {
        private List<List<char>> _map = [];
        private (int, int) _me = (0, 0);
        private int _w;
        private int _h = 0;

        public Day15() : base(nameof(Day15)) 
        {
            _w = Input.First().Length;
            ParseMap();
        }
        public override long SolvePart1()
        {
            foreach (var line in Input.Skip(_h + 1))
            {
                foreach (char movement in line)
                {
                    switch (movement)
                    {
                        case '<':
                            Move((-1, 0));
                            break;
                        case '^':
                            Move((0, -1));
                            break;
                        case '>':
                            Move((1, 0));
                            break;
                        case 'v':
                            Move((0, 1));
                            break;
                    }
                }
            }

            return CalculateTotal();
        }

        public override long SolvePart2()
        {
            List<List<char>> wideMap = [];
            for (int i = 0; i < _h; i++)
            {
                wideMap.Add([]);
                for (int j = 0; j < _w; j++)
                {
                    if (_map[i][j] == '@')
                    {
                        _me = (j * 2, i);
                        wideMap[i].Add('@');
                        wideMap[i].Add('.');
                    }
                    else if (_map[i][j] == 'O')
                    {
                        wideMap[i].Add('[');
                        wideMap[i].Add(']');
                    }
                    else
                    {
                        wideMap[i].Add(_map[i][j]);
                        wideMap[i].Add(_map[i][j]);
                    }
                }
            }

            _map = wideMap;
            _w *= 2;


            foreach (var line in Input.Skip(_h + 1))
            {
                foreach (char movement in line)
                {
                    switch (movement)
                    {
                        case '<':
                            Move2((-1, 0));
                            break;
                        case '^':
                            Move2((0, -1));
                            break;
                        case '>':
                            Move2((1, 0));
                            break;
                        case 'v':
                            Move2((0, 1));
                            break;
                    }
                }
            }

            return CalculateTotal();
        }

        private void Move((int, int) dir)
        {
            (int, int) to = (_me.Item1 + dir.Item1, _me.Item2 + dir.Item2);
            if (_map[to.Item2][to.Item1] == '#') return;
            if (_map[to.Item2][to.Item1] == '.')
            {
                _map[to.Item2][to.Item1] = '@';
                _map[_me.Item2][_me.Item1] = '.';
                _me = to;
            }
            else if (_map[to.Item2][to.Item1] == 'O')
            {
                (int, int) next = to;
                while (_map[next.Item2][next.Item1] == 'O')
                {
                    next = (next.Item1 + dir.Item1, next.Item2 + dir.Item2);
                }
                if (_map[next.Item2][next.Item1] == '#') return;

                _map[to.Item2][to.Item1] = '@';
                _map[_me.Item2][_me.Item1] = '.';
                _me = to;
                while (!to.Equals(next))
                {
                    to = (to.Item1 + dir.Item1, to.Item2 + dir.Item2);
                    _map[to.Item2][to.Item1] = 'O';
                }
            }
        }

        private long CalculateTotal()
        {
            long total = 0;
            for (int i = 0; i < _h; i++)
            {
                for (int j = 0; j < _w; j++)
                {
                    if (_map[i][j] == 'O' || _map[i][j] == '[')
                    {
                        total += i * 100 + j;
                    }
                }
            }
            return total;
        }

        private void ParseMap()
        {
            foreach (var line in Input)
            {
                if (string.IsNullOrEmpty(line)) break;
                if (line.Contains('@'))
                {
                    _me = (line.IndexOf('@'), _h);
                }
                _map.Add([.. line]);
                _h++;
            }
        }

        //private void PrintMap()
        //{
        //    foreach (var line in _map)
        //    {
        //        Console.WriteLine(string.Join("", line));
        //    }
        //    Console.WriteLine();
        //}


        private bool Blocked(bool up, (int, int) pos)
        {
            (int, int) next = (pos.Item1, pos.Item2 + (up ? -1 : 1));
            if (_map[next.Item2][next.Item1] == '#') return true;
            else if (_map[next.Item2][next.Item1] == '.') return false;
            else if (_map[next.Item2][next.Item1] == '[')
            {
                return Blocked(up, next) || Blocked(up, (next.Item1 + 1, next.Item2));
            }
            else
            {
                return Blocked(up, next) || Blocked(up, (next.Item1 - 1, next.Item2));
            }
        }

        private void ResolveVertical(char prev, char c, bool up, (int, int) pos)
        {
            (int, int) next = (pos.Item1, pos.Item2 + (up ? -1 : 1));
            if (_map[next.Item2][next.Item1] == '[')
            {
                ResolveVertical(c, '[', up, next);
                ResolveVertical('.', ']', up, (next.Item1 + 1, next.Item2));
            }
            else if (_map[next.Item2][next.Item1] == ']')
            {
                ResolveVertical(c, ']', up, next);
                ResolveVertical('.', '[', up, (next.Item1 - 1, next.Item2));
            }
            _map[next.Item2][next.Item1] = c;
            _map[pos.Item2][pos.Item1] = prev;
        }

        private void Move2((int, int) dir)
        {
            (int, int) to = (_me.Item1 + dir.Item1, _me.Item2 + dir.Item2);
            if (_map[to.Item2][to.Item1] == '#') return;
            else if (_map[to.Item2][to.Item1] == '.')
            {
                _map[to.Item2][to.Item1] = '@';
                _map[_me.Item2][_me.Item1] = '.';
                _me = to;
                return;
            }
            else if (dir.Item2 == 0 && (_map[to.Item2][to.Item1] == '[' || _map[to.Item2][to.Item1] == ']'))
            {
                (int, int) next = to;
                while (_map[next.Item2][next.Item1] == '[' || _map[next.Item2][next.Item1] == ']')
                {
                    next = (next.Item1 + dir.Item1, next.Item2 + dir.Item2);
                }
                if (_map[next.Item2][next.Item1] == '#') return;

                _map[to.Item2][to.Item1] = '@';
                _map[_me.Item2][_me.Item1] = '.';
                _me = to;
                while (!to.Equals(next))
                {
                    to = (to.Item1 + dir.Item1, to.Item2 + dir.Item2);
                    _map[to.Item2][to.Item1] = dir.Item1 == -1 ? ']' : '[';
                    to = (to.Item1 + dir.Item1, to.Item2 + dir.Item2);
                    _map[to.Item2][to.Item1] = dir.Item1 == -1 ? '[' : ']';
                }
                return;
            }
            else
            {
                if (Blocked(dir.Item2 == -1, _me)) return;
                ResolveVertical('.', '@', dir.Item2 == -1, _me);
                _me = (_me.Item1, _me.Item2 + dir.Item2);
                return;
            }
        }
    }
}
