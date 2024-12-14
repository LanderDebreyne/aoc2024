namespace AdventOfCode
{
    public class Day14 : BaseDayInputInLines<long>
    {
        private readonly int _w = 101;
        private readonly int _h = 103;
        private readonly List<(int px, int py, int vx, int vy)> _robots = [];

        public Day14() : base(nameof(Day14))
        {
        }

        public override long SolvePart1()
        {
            ParseRobots();

            long q1 = 0;
            long q2 = 0;
            long q3 = 0;
            long q4 = 0;
            int w = (_w / 2);
            int h = (_h / 2);
            foreach ((int px, int py, int vx, int vy) in _robots)
            {
                var x = (px + 100 * vx) % _w;
                var y = (py + 100 * vy) % _h;

                while (x < 0)
                {
                    x += _w;
                }
                while (y < 0)
                {
                    y += _h;
                }

                if (x < w && y < h)
                {
                    q1++;
                }
                else if (x < w && y > h)
                {
                    q2++;
                }
                else if (x > w && y < h)
                {
                    q3++;
                }
                else if (x > w && y > h)
                {
                    q4++;
                }
            }
            return q1* q2 * q3 * q4;
        }

        public override long SolvePart2()
        {
            ParseRobots();

            bool[,] grid = new bool[_w, _h];
            int c = 0;

            while (true)
            {
                Array.Clear(grid, 0, grid.Length);
                foreach ((int px, int py, int vx, int vy) in _robots)
                {
                    var x = (px + c * vx) % _w;
                    var y = (py + c * vy) % _h;

                    while (x < 0)
                    {
                        x += _w;
                    }
                    while (y < 0)
                    {
                        y += _h;
                    }

                    grid[x,y] = true;
                }

                for (int y = 0; y < _h; y++)
                {
                    int consecutive = 0;
                    for (int x = 0; x < _w; x++)
                    {
                        if (grid[x, y])
                        {
                            consecutive++;
                            if (consecutive == 25)
                            {
                                return c;
                            }
                        }
                        else
                        {
                            consecutive = 0;
                        }
                    }
                }
                c++;
            }
        }

        private void ParseRobots()
        {
            foreach (string line in Input)
            {
                var parts = line.Split(' ');
                var pc = parts[0].IndexOf(',');
                var px = int.Parse(parts[0][2..pc]);
                var py = int.Parse(parts[0][(pc + 1)..]);

                var vc = parts[1].IndexOf(',');
                var vx = int.Parse(parts[1][2..vc]);
                var vy = int.Parse(parts[1][(vc + 1)..]);

                _robots.Add((px, py, vx, vy));
            }
        }
    }
}
