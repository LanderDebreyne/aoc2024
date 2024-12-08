using System.Text.RegularExpressions;

namespace AdventOfCode;

internal partial class Day3() : BaseDayInputAsString(nameof(Day3))
{
    public override int SolvePart1()
    {
        return MulString(Input);
    }

    public override int SolvePart2()
    {
        return EnabledMulString(Input);
    }

    private int MulString(string input)
    {
        return MulRegex().Matches(input).Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum();
    }

    private int EnabledMulString(string input)
    {
        var disable = input.IndexOf("don't()");
        if (disable != -1)
        {
            var disabled = input[(disable + 1)..];
            var findNext = disabled.IndexOf("do()");
            if (findNext != -1)
            {
                return MulString(input[..disable]) + EnabledMulString(disabled[(findNext + 1)..]);
            }
            return MulString(input[..disable]);
        }
        return MulString(input);
    }

    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    private static partial Regex MulRegex();
}
