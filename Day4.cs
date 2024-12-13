using System.Text.RegularExpressions;

namespace AdventOfCode;

internal partial class Day4 : BaseDayInputAsStringArray<long>
{
    private readonly int _n;
    private readonly bool[,] _X;
    private readonly bool[,] _M;
    private readonly bool[,] _A;
    private readonly bool[,] _S;

    public Day4() : base(nameof(Day4))
    {
        _n = Input.Length;
        _X = new bool[_n, _n];
        _M = new bool[_n, _n];
        _A = new bool[_n, _n];
        _S = new bool[_n, _n];
    }

    public override long SolvePart1()
    {
        int r = 0;
        string[] verticals = new string[_n];
        for (int i = 0; i < _n; i++)
        {
            string line = Input[i];
            for (int j = 0;  j < _n; j++)
            {
                if (line[j] == 'X')
                {
                    _X[i, j] = true;
                    r += CheckXDiag(i, j, -1) +
                        CheckXDiag(i, j, 1);
                }
                else if (line[j] == 'M')
                {
                    _M[i, j] = true;
                }
                else if (line[j] == 'A')
                {
                    _A[i, j] = true;
                }
                else if (line[j] == 'S')
                {
                    _S[i, j] = true;
                    r += CheckSDiag(i, j, -1) +
                    CheckSDiag(i, j, 1);
                }
                verticals[i] += line[j];
            }
            r += Forward().Matches(line).Count;
            r += Backward().Matches(line).Count;
        }
        foreach (string vertical in verticals)
        {
            r += Forward().Matches(vertical).Count;
            r += Backward().Matches(vertical).Count;
        }
        return r-1;
    }

    public override long SolvePart2()
    {
        int r = 0;
        Stack<(int, int)> _XMAS = new();
        for (int i = 0; i < _n; i++)
        {
            string line = Input[i];
            for (int j = 0; j < _n; j++)
            {
                if (line[j] == 'M')
                {
                    _M[i, j] = true;
                }
                else if (line[j] == 'A')
                {
                    _A[i, j] = true;
                    _XMAS.Push((i, j));
                }
                else if (line[j] == 'S')
                {
                    _S[i, j] = true;
                }
            }
        }
        while(_XMAS.Count > 0)
        {
            (int i, int j) = _XMAS.Pop();
            r += CheckXMAS(i, j);
        }

        return r;
    }

    private int CheckXDiag(int i, int j, int v)
    {
        int cJ = j + (v * 3);
        if (cJ >= 0 && i - 3 >= 0
            && cJ < _n
            && _M[i - 1, j + (v * 1)]
            && _A[i - 2, j + (v * 2)]
            && _S[i - 3, j + (v * 3)])
            return 1;
        return 0;
    }

    private int CheckSDiag(int i, int j, int v)
    {
        int cJ = j + (v * 3);
        if (cJ >= 0
            && cJ < _n
            && i-3 >= 0
            && _A[i - 1, j + (v * 1)]
            && _M[i - 2, j + (v * 2)]
            && _X[i - 3, j + (v * 3)])
            return 1;
        return 0;
    }

    private int CheckXMAS(int i, int j)
    {
        if (i - 1 >= 0 && j - 1 >= 0
            && i + 1 < _n
            && j + 1 < _n
            && ((_M[i - 1, j - 1] && _S[i + 1, j + 1]) || (_S[i - 1, j - 1] && _M[i + 1, j + 1]))
            && ((_M[i - 1, j + 1] && _S[i + 1, j - 1]) || (_S[i - 1, j + 1] && _M[i + 1, j - 1])))
                return 1;
        return 0;
    }

    [GeneratedRegex("XMAS")]
    private static partial Regex Forward();

    [GeneratedRegex("SAMX")]
    private static partial Regex Backward();
}
