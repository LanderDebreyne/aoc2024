namespace AdventOfCode
{
    public class Day11() : BaseDayInputAsString<long>(nameof(Day11))
    {
        private readonly Dictionary<(int, long), long> _memory = [];

        public override long SolvePart1() => Solve(25);

        public override long SolvePart2() => Solve(75);

        private long Solve(int blinks) => Input.Split(' ').Select(long.Parse).Select(stone => Stones(0, stone, blinks)).Sum();

        private long Stones(int blinks, long currentValue, int max)
        {
            if (blinks == max) return 1;
            
            if (_memory.TryGetValue((blinks, currentValue), out long stones))
            {
                return stones;
            }

            long mem;

            if (currentValue == 0)
            {
                mem = Stones(blinks + 1, 1, max);
                _memory[(blinks, currentValue)] = mem;
                return mem;
            };

            string currentValueString = currentValue.ToString();
            if (currentValueString.Length % 2 == 0)
            {
                mem = Stones(blinks + 1, long.Parse(currentValueString[..(currentValueString.Length / 2)]), max) 
                    + Stones(blinks + 1, long.Parse(currentValueString[(currentValueString.Length / 2)..]), max);
                _memory[(blinks, currentValue)] = mem;
                return mem;
            }
            
            mem = Stones(blinks + 1, currentValue * 2024, max);
            _memory[(blinks, currentValue)] = mem;
            return mem;
        }
    }
}
