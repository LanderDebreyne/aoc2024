namespace AdventOfCode
{
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
        Day7 = 7,
        Day8 = 8,
        Day9 = 9,
        Day10 = 10,
        Day11 = 11,
        Day12 = 12,
        Day13 = 13,
        Day14 = 14,
        Day15 = 15,
        Day16 = 16,
        All = -1,
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter day number (1-25): ");
            Day? day = (Day)int.Parse(Console.ReadLine() ?? throw new Exception("Please give input."));

            if(day == Day.All)
            {
                RunTestSuite();
                return;
            }

            Console.Write("Enter part (1 or 2): ");
            Part? part = (Part?)int.Parse(Console.ReadLine() ?? throw new Exception("Please give input."));

            try
            {
                string? dayName = day.ToString();

                Type? dayType = Type.GetType("AdventOfCode." + dayName);

                if (dayType == null)
                {
                    Console.WriteLine($"No solutions found for {day}");
                    return;
                }

                var dayInstance = Activator.CreateInstance(dayType);

                if (dayInstance == null)
                {
                    Console.WriteLine($"Instance for {day} could not be activated");
                    return;
                }

                object? result = part == Part.Part1
                    ? dayInstance?.GetType().GetMethod("SolvePart1")?.Invoke(dayInstance, null)
                    : part == Part.Part2 ? dayInstance?.GetType().GetMethod("SolvePart2")?.Invoke(dayInstance, null) : null;

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
                Console.WriteLine($"Error: {ex}");
            }
        }

        private static void RunTestSuite()
        {
            var globalSw = System.Diagnostics.Stopwatch.StartNew();
            foreach (Day day in Enum.GetValues<Day>())
            {
                if (day == Day.Day9 || day == Day.Day12)
                {
                    Console.WriteLine($"Skipping Day {day}, it takes too long to run");
                    continue;
                }
                Console.WriteLine($"Running tests for {day}");
                if (day == Day.All)
                {
                    continue;
                }
                Type? dayType = Type.GetType("AdventOfCode." + day);
                if (dayType == null)
                {
                    Console.WriteLine($"No solutions found for {day}");
                    return;
                }
                var dayInstance = Activator.CreateInstance(dayType);
                if (dayInstance == null)
                {
                    Console.WriteLine($"Instance for {day} could not be activated");
                    return;
                }
                var sw = System.Diagnostics.Stopwatch.StartNew();
                object? result1 = dayInstance?.GetType().GetMethod("SolvePart1")?.Invoke(dayInstance, null);
                if (sw.ElapsedMilliseconds > 1000)
                {
                    Console.WriteLine($"Part1 took too long for Day {day}, took {sw.ElapsedMilliseconds}ms");
                    return;
                }
                Console.WriteLine($"Part1 passed for {day}, took {sw.ElapsedMilliseconds}ms");
                var expectedResult = typeof(Results.Results).GetField($"{day}Part1")?.GetValue(null);
                if (!result1?.Equals(expectedResult) ?? true)
                {
                    Console.WriteLine($"Part1 failed for {day}");
                    return;
                }
                dayInstance = Activator.CreateInstance(dayType);
                if (dayInstance == null)
                {
                    Console.WriteLine($"Instance for {day} could not be activated");
                    return;
                }
                sw.Restart();
                object? result2 = dayInstance?.GetType().GetMethod("SolvePart2")?.Invoke(dayInstance, null);
                if (sw.ElapsedMilliseconds > 1000)
                {
                    Console.WriteLine($"Part2 took too long for {day}, took {sw.ElapsedMilliseconds}ms");
                    return;
                }
                Console.WriteLine($"Part2 passed for {day}, took {sw.ElapsedMilliseconds}ms");
                expectedResult = typeof(Results.Results).GetField($"{day}Part2")?.GetValue(null);
                if (!result2?.Equals(expectedResult) ?? true)
                {
                    Console.WriteLine($"Part2 failed for {day}");
                    return;
                }
            }
            Console.WriteLine($"All tests passed, took {globalSw.ElapsedMilliseconds}ms");
        }
    }
}