namespace AdventOfCode
{
    public abstract class BaseDay(string day)
    {
        internal string InputFilePath => $"./input/{day}.txt";

        public abstract long SolvePart1();
        public abstract long SolvePart2();
    }

    public abstract class BaseDayInputInLines(string day) : BaseDay(day)
    {
        internal IEnumerable<string> Input => File.ReadLines(InputFilePath);
    }

    public abstract class BaseDayInputAsString(string day) : BaseDay(day)
    {
        internal string Input => File.ReadAllText(InputFilePath);
    }
    public abstract class BaseDayInputAsStringArray(string day) : BaseDay(day)
    {
        internal string[] Input => File.ReadAllLines(InputFilePath);
    }
}
