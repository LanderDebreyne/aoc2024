namespace AdventOfCode;

internal class Day1() : BaseDayInputInLines(nameof(Day1))
{
    public override int SolvePart1()
    {
        List<int> firstList = [];
        List<int> secondList = [];
        foreach (var line in Input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            firstList.Add(int.Parse(split[0]));
            secondList.Add(int.Parse(split[1]));
        }

        firstList.Sort();
        secondList.Sort();

        return firstList.Select((i, index) => Math.Abs(secondList[index] - i)).Sum();
    }

    public override int SolvePart2()
    {
        Dictionary<int, (int, int)> map = [];
        foreach (var line in Input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int first = int.Parse(split[0]);
            int second = int.Parse(split[1]);

            if (map.TryGetValue(first, out (int, int) pair))
            {
                pair.Item1 += 1;
                map[first] = pair;
            }
            else
            {
                map.Add(first, (1, 0));
            }

            if (map.TryGetValue(second, out (int, int) p))
            {
                p.Item2 += 1;
                map[second] = p;
            }
            else
            {
                map.Add(second, (0, 1));
            }
        }

        int result = 0;

        foreach (var pair in map)
        {
            result += pair.Key * pair.Value.Item1 * pair.Value.Item2;
        }

        return result;
    }
}
