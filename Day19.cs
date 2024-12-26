namespace AdventOfCode
{
    public class Day19() : BaseDayInputInLines<long>(nameof(Day19))
    {
        private List<string> _patterns = [];
        private readonly Dictionary<string, long> _memo = [];
        public override long SolvePart1()
        {
            _patterns = Input.First().Split(", ").ToList();
            long result = 0;
            foreach (var line in Input.Skip(2))
            {
                result += MatchesPattern(line) ? 1 : 0;
            }
            return result;
        }

        public override long SolvePart2()
        {
            _patterns = Input.First().Split(", ").ToList();
            long result = 0;
            foreach (var line in Input.Skip(2))
            {
                result += PatternMatchCount(line);
            }
            return result;
        }

        private bool MatchesPattern(string line)
        {
            if (line.Length == 0)
                return true;

            foreach (var pattern in _patterns)
            {
                if (line.StartsWith(pattern))
                {
                    if (MatchesPattern(line[pattern.Length..]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private long PatternMatchCount(string line)
        {
            if (line.Length == 0)
                return 1;

            if (_memo.TryGetValue(line, out long value))
                return value;

            long result = 0;
            foreach (var pattern in _patterns)
            {
                if (line.StartsWith(pattern))
                {
                    result += PatternMatchCount(line[pattern.Length..]);
                }
            }
            _memo[line] = result;
            return result;
        }
    }
}
