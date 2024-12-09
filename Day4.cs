using System.Text.RegularExpressions;

namespace AdventOfCode;

internal partial class Day4() : BaseDayInputAsStringArray<long>(nameof(Day4))
{
    public override long SolvePart1()
    {
        int r = 0;

        for (int i = 0; i < Input.Length; i++)
        {
            string line = "";
            string l = "";
            for (int j = 0;  j < Input[i].Length; j++)
            {
                if (Input[i][j] == 'X')
                {
                    r += CheckDiag(Input, i, j);
                }
                l += Input[i][j];
                line += Input[j][i];
            }
            r += Forward().Matches(line).Count;
            r += Backward().Matches(line).Count;

            r += Backward().Matches(l).Count;
            r += Forward().Matches(l).Count;
        }

        return r;
    }

    public override long SolvePart2()
    {
        int r = 0;

        for (int i = 0; i < Input.Length; i++)
        {
            for (int j = 0; j < Input[i].Length; j++)
            {
                if (Input[i][j] == 'A')
                {
                    r += CheckXMAS(Input, i, j);
                }
            }
        }

        return r;
    }

    private int CheckDiag(string[] input, int i, int j)
    {
        return CheckDiagDir(input, i, j, -1, -1) +
            CheckDiagDir(input, i, j, 1, 1) +
            CheckDiagDir(input, i, j, -1, 1) +
            CheckDiagDir(input, i, j, 1, -1);       
    }

    private int CheckDiagDir(string[] input, int i, int j, int v1, int v2)
    {
        int cI = i + (v1 * 3);
        int cJ = j + (v2 * 3);
        if(cI >=  0 && cJ >= 0 
            && cI < Input.Length
            && cJ < Input[0].Length 
            && input[i + (v1 * 1)][j + (v2 * 1)] == 'M'
            && input[i + (v1 * 2)][j + (v2 * 2)] == 'A'
            && input[i + (v1 * 3)][j + (v2 * 3)] == 'S')
            return 1;
        return 0;
    }



    private int CheckXMAS(string[] input, int i, int j)
    {
        if (i - 1 >= 0 && j - 1 >= 0
            && i + 1 < Input.Length
            && j + 1 < Input[0].Length
            && ((input[i - 1][j - 1] == 'M' && input[i + 1][j + 1] == 'S') || (input[i - 1][j - 1] == 'S' && input[i + 1][j + 1] == 'M'))
            && ((input[i - 1][j + 1] == 'M' && input[i + 1][j - 1] == 'S') || (input[i - 1][j + 1] == 'S' && input[i + 1][j - 1] == 'M')))
                return 1;
        return 0;
    }

    [GeneratedRegex("XMAS")]
    private static partial Regex Forward();

    [GeneratedRegex("SAMX")]
    private static partial Regex Backward();
}
