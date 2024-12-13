namespace AdventOfCode
{
    public class Day7() : BaseDayInputInLines<long>(nameof(Day7))
    {
        public override long SolvePart1() => Solve(false);
        public override long SolvePart2() => Solve(true);

        private long Solve(bool con)
        {
            long result = 0;
            foreach (var line in Input)
            {
                var parts = line.Split(":");
                var goal = long.Parse(parts[0]);
                var terms = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                if (CombineTerms(terms, goal, con))
                {
                    result += goal;
                }
            }
            return result;
        }

        private static bool CombineTerms(List<long> terms, long goal, bool con)
        {
            if (terms.Count == 1)
            {
                return terms.First() == goal;
            }
            else
            {
                var plusList = terms.Skip(1).ToList();
                var mulList = terms.Skip(1).ToList();
                var conList = terms.Skip(1).ToList();
                plusList[0] = plusList[0] + terms.First();
                mulList[0] = mulList[0] * terms.First();
                conList[0] = long.Parse(terms.First().ToString() + conList[0].ToString());
                return CombineTerms(plusList, goal, con) || CombineTerms(mulList, goal, con) || (con && CombineTerms(conList, goal, con));
            }
        }
    }
}
