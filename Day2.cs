namespace AdventOfCode;

internal class Day2() : BaseDayInputInLines<long>(nameof(Day2))
{
    public override long SolvePart1()
    {
        int result = 0;
        foreach (var line in Input)
        {
            var ints = line.Split(' ').Select(int.Parse).ToList();
            int sign = (ints.First() > ints.Skip(1).First()) ? -1 : (ints.First() < ints.Skip(1).First()) ? 1 : 0;
            if (sign == 0)
                continue;

            bool inc = true;
            bool desc = true;

            for (int i = 0; i < ints.Count - 1; i++)
            {
                int diff = ints[i + 1] - ints[i];
                if (diff > 0)
                {
                    desc = false;
                }
                else if (diff < 0)
                {
                    inc = false;
                }

                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                {
                    desc = false;
                    inc = false;
                    break;
                }
            }

            if (desc || inc)
                result++;
        }

        return result;
    }

    public override long SolvePart2()
    {
        int result = 0;
        foreach (var line in Input)
        {
            var ints = line.Split(' ').Select(int.Parse).ToList();
            if (Main(ints))
                result++;
        }

        return result;

        static bool Main(List<int> ints)
        {
            bool safe = IsRestOfLineSafe(ints);
            safe |= Consider(ints, 0);
            for (int i = 0; i < ints.Count-1; i++)
            {
                int diff = ints[i + 1] - ints[i];
                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                {
                    safe |= Consider(ints, i) || Consider(ints, i + 1);
                }

                if (i + 2 < ints.Count)
                {
                    int diff2 = ints[i + 2] - ints[i + 1];

                    if ((diff > 0) != (diff2 > 0))
                    {
                        safe |= Consider(ints, i + 1) || Consider(ints, i + 2) || Consider(ints, i);
                    }
                }
            }
            return safe;
        }

        static bool IsRestOfLineSafe(List<int> ints)
        {
            bool inc = true;
            bool desc = true;
            for (int i = 0; i < ints.Count - 1; i++)
            {
                int diff = ints[i + 1] - ints[i];
                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                {
                    return false;
                }
                if (diff > 0)
                {
                    desc = false;
                }
                else if (diff < 0)
                {
                    inc = false;
                }
            }
            return inc || desc;
        }

        static bool Consider(List<int> ints, int remove)
        {
            List<int> copy = [.. ints];
            copy.RemoveAt(remove);
            return IsRestOfLineSafe(copy);
        }
    }
}
