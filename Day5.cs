
namespace AdventOfCode
{
    public class Day5() : BaseDayInputInLines(nameof(Day5))
    {
        public override long SolvePart1()
        {
            Dictionary<(int, int), bool> ordering = [];
            int skip = 0;
            foreach(var line in Input)
            {
                skip++;
                if (string.IsNullOrWhiteSpace(line))
                    break;

                var order = line.Split('|').Select(int.Parse).ToList();
                ordering[(order[0], order[1])] = true;
                ordering[(order[1], order[0])] = false;
            }

            int r = 0;
            foreach(var line in Input.Skip(skip))
            {
                var ints = line.Split(",").Select(int.Parse).ToList();
                bool goodManual = true;
                for (int i = 0; i < ints.Count; i++)
                {
                    for(int j = i+1;  j < ints.Count; j++)
                    {
                        if (ordering.ContainsKey((ints[i], ints[j])) && !ordering[(ints[i], ints[j])])
                        {
                            goodManual = false;
                        }
                    }
                }
                if (goodManual) r += ints[ints.Count / 2];
            }
            return r;
        }

        public override long SolvePart2()
        {
            Dictionary<(int, int), bool> ordering = [];
            int skip = 0;
            foreach (var line in Input)
            {
                skip++;
                if (string.IsNullOrWhiteSpace(line))
                    break;

                var order = line.Split('|').Select(int.Parse).ToList();
                ordering[(order[0], order[1])] = true;
                ordering[(order[1], order[0])] = false;
            }

            int r = 0;
            foreach (var line in Input.Skip(skip))
            {
                var ints = line.Split(",").Select(int.Parse).ToList();
                bool goodManual = true;
                for (int i = 0; i < ints.Count; i++)
                {
                    if (goodManual)
                    {
                        for (int j = i + 1; j < ints.Count; j++)
                        {
                            if (ordering.ContainsKey((ints[i], ints[j])) && !ordering[(ints[i], ints[j])])
                            {
                                goodManual = false;
                                break;
                            }
                        }
                    }
                    else break;
                }
                if (!goodManual)
                {
                    r += MiddleOfSortedLine(ints, ordering);
                };
            }
            return r;
        }

        private int MiddleOfSortedLine(List<int> ints, Dictionary<(int, int), bool> ordering)
        {
            for (int i =0; i<ints.Count; i++)
            {
                for(int j = i+1; j<ints.Count; j++)
                {
                    if (ordering.ContainsKey((ints[i], ints[j])) && !ordering[(ints[i], ints[j])])
                    {
                        int s1 = ints[i];
                        int s2 = ints[j];
                        ints[i] = s2;
                        ints[j] = s1;
                        return MiddleOfSortedLine(ints, ordering);
                    }
                }
            }
            return ints[ints.Count / 2];
        }
    }
}
