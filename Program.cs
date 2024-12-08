using AdventOfCode;

public enum Part
{
    Part1 = 1,
    Part2 = 2,
}

public enum Day
{
    Day1 = 1,
    Day2 = 2,
    Day3 = 3,
    Day4 = 4,
    Day5 = 5,
    Day6 = 6,
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter day number (1-25): ");
        Day? day = (Day)int.Parse(Console.ReadLine() ?? throw new Exception("Please give input."));

        Console.Write("Enter part (1 or 2): ");
        Part? part = (Part?)int.Parse(Console.ReadLine() ?? throw new Exception("Please give input."));

        try
        {
            string? dayName = day.ToString();

            Type? dayType = Type.GetType("AdventOfCode."+dayName);

            if (dayType == null)
            {
                Console.WriteLine($"No solutions found for {day}");
                return;
            }

            BaseDay? dayInstance = (BaseDay?)Activator.CreateInstance(dayType);

            if (dayInstance == null)
            {
                Console.WriteLine($"Instance for {day} could not be activated");
                return;
            }

            int? result = part == Part.Part1
                ? dayInstance?.SolvePart1()
                : part == Part.Part2 ? dayInstance?.SolvePart2() : null;

            if (result == null)
            {
                Console.WriteLine($"No solutions found for {day} {part}");
            }
            else
            {
                Console.WriteLine($"{day} {part} Result: {result}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

