namespace AdventOfCode
{
    public class Day13() : BaseDayInputInLines<long>(nameof(Day13))
    {
        public override long SolvePart1() => Solve(true);
        public override long SolvePart2() => Solve(false);
        private long Solve(bool part)
        {
            List<Machine> machines = [];

            for (int i = 0; i < Input.Count(); i += 4)
            {
                long aX = long.Parse(Input.ElementAt(i)[(Input.ElementAt(i).IndexOf("X+") + 2)..Input.ElementAt(i).IndexOf(",")]);
                long aY = long.Parse(Input.ElementAt(i)[(Input.ElementAt(i).IndexOf("Y+") + 2)..]);

                long bX = long.Parse(Input.ElementAt(i + 1)[(Input.ElementAt(i + 1).IndexOf("X+") + 2)..Input.ElementAt(i + 1).IndexOf(',')]);
                long bY = long.Parse(Input.ElementAt(i + 1)[(Input.ElementAt(i + 1).IndexOf("Y+") + 2)..]);

                long pX = long.Parse(Input.ElementAt(i + 2)[(Input.ElementAt(i + 2).IndexOf("X=") + 2)..Input.ElementAt(i + 2).IndexOf(',')]);
                long pY = long.Parse(Input.ElementAt(i + 2)[(Input.ElementAt(i + 2).IndexOf("Y=") + 2)..]);
                pX += part ? 0 : 10000000000000;
                pY += part ? 0 : 10000000000000;
                machines.Add(new Machine { A = (aX, aY), B = (bX, bY), Prize = (pX, pY) });
            }

            long total = 0;
            foreach (Machine machine in machines)
            {
                double det = machine.A.X * machine.B.Y - machine.A.Y * machine.B.X;
                long i = (long)Math.Round((machine.B.Y * machine.Prize.X - machine.B.X * machine.Prize.Y) / det);
                long j = (long)Math.Round((machine.A.X * machine.Prize.Y - machine.A.Y * machine.Prize.X) / det);

                (long, long) loc = (i * machine.A.X + j * machine.B.X, i * machine.A.Y + j * machine.B.Y);
                if (loc == machine.Prize)
                    total += i * 3 + j;
            }

            return total;
        }

        public struct Machine
        {
            public (long X, long Y) A; 
            public (long X, long Y) B;
            public (long X, long Y) Prize;
        }
    }
}
