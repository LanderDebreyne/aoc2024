namespace AdventOfCode
{
    public abstract class BaseDay<T>(string day)
    {
        internal string InputFilePath => $"./input/{day}.txt";

        public abstract T SolvePart1();
        public abstract T SolvePart2();
    }

    public abstract class BaseDayInputInLines<T>(string day) : BaseDay<T>(day)
    {
        internal IEnumerable<string> Input => File.ReadLines(InputFilePath);
    }

    public abstract class BaseDayInputAsString<T>(string day) : BaseDay<T>(day)
    {
        internal string Input => File.ReadAllText(InputFilePath);
    }
    public abstract class BaseDayInputAsStringArray<T>(string day) : BaseDay<T>(day)
    {
        internal string[] Input => File.ReadAllLines(InputFilePath);
    }
}
