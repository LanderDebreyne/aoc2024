namespace AdventOfCode
{
    public class Day12 : BaseDayInputAsStringArray<int>
    {
        private readonly int _n;
        private readonly bool[,] _visited;
        private readonly List<(char c, List<(int, int)> r)> _regions;
        private List<(int, int)> _current;

        public Day12() : base(nameof(Day12))
        {
            _n = Input.Length;
            _visited = new bool[_n, _n];
            _regions = [];
            _current = [];
        }

        public override int SolvePart1()
        {
            for (int i = 0; i < _n; i++)
            {
                string line = Input[i];
                for (int j = 0; j < _n; j++)
                {
                    if (!_visited[i, j])
                    {
                        _current = [(i, j)];
                        _visited[i, j] = true;
                        FillRegions(i-1, j, line[j]);
                        FillRegions(i+1, j, line[j]);
                        FillRegions(i, j-1, line[j]);
                        FillRegions(i, j+1, line[j]);

                        _regions.Add((line[j], _current));
                    }
                }
            }

            int result = 0;
            foreach (var (c, r) in _regions)
            {
                int area = r.Count;
                int perimeter = PerimeterOfPlot(r, c);
                result += area * perimeter;

            }
            return result;
        }

        public override int SolvePart2()
        {
            for (int i = 0; i < _n; i++)
            {
                string line = Input[i];
                for (int j = 0; j < _n; j++)
                {
                    if (!_visited[i, j])
                    {
                        _current = [(i, j)];
                        _visited[i, j] = true;
                        FillRegions(i - 1, j, line[j]);
                        FillRegions(i + 1, j, line[j]);
                        FillRegions(i, j - 1, line[j]);
                        FillRegions(i, j + 1, line[j]);

                        _regions.Add((line[j], _current));
                    }
                }
            }

            int result = 0;
            foreach (var (c, r) in _regions)
            {
                int area = r.Count;
                int sides = SidesOfPlot(r, c);
                result += area * sides;

            }
            return result;
        }

        private void FillRegions(int i, int j, char c)
        {
            if (i < 0 || i >= _n || j < 0 || j >= _n || _visited[i, j] || Input[i][j] != c)
            {
                return;
            }
            _visited[i, j] = true;
            _current.Add((i, j));
            FillRegions(i - 1, j, c);
            FillRegions(i + 1, j, c);
            FillRegions(i, j - 1, c);
            FillRegions(i, j + 1, c);
        }

        private int PerimeterOfPlot(List<(int, int)> region, char c)
        {
            int perimeter = 0;
            foreach (var plot in region)
            {
                if (plot.Item1 == 0 || Input[plot.Item1 - 1][plot.Item2] != c)
                {
                    perimeter++;
                }
                if (plot.Item1 == _n - 1 || Input[plot.Item1 + 1][plot.Item2] != c)
                {
                    perimeter++;
                }
                if (plot.Item2 == 0 || Input[plot.Item1][plot.Item2 - 1] != c)
                {
                    perimeter++;
                }
                if (plot.Item2 == _n - 1 || Input[plot.Item1][plot.Item2 + 1] != c)
                {
                    perimeter++;
                }
            }
            return perimeter;
        }

        private int SidesOfPlot(List<(int, int)> region, char c)
        {
            int corners = 0;
            foreach ((int, int) plot in region)
            {
                corners+= CountCorners(plot,c);
            }
            return corners;
        }

        private int CountCorners((int, int) plot, char c)
        {
            List<(int,int)> directions = [(1, 1), (1, -1), (-1, 1), (-1,-1)];
            int corners = 0;
            foreach ((int, int) dir in directions)
            {
                (int, int) check1 = (plot.Item1 + dir.Item1, plot.Item2);
                (int, int) check2 = (plot.Item1, plot.Item2 + dir.Item2);
                (int, int) check3 = (plot.Item1 + dir.Item1, plot.Item2 + dir.Item2);
                if ((check1.Item1 < 0 || check1.Item1 == _n || Input[check1.Item1][check1.Item2] != c) &&
                    (check2.Item2 < 0 || check2.Item2 == _n || Input[check2.Item1][check2.Item2] != c))
                {
                    corners++;
                }

                if (check3.Item1 < 0 || check3.Item1 == _n || check3.Item2 < 0 || check3.Item2 == _n || Input[check3.Item1][check3.Item2] != c)
                {
                    if (check1.Item1 >= 0 && check1.Item1 < _n && Input[check1.Item1][check1.Item2] == c 
                        && check2.Item2 >= 0 && check2.Item2 < _n && Input[check2.Item1][check2.Item2] == c)
                    {
                        corners++;
                    }
                }
            }

            return corners;
        }
    }
}
