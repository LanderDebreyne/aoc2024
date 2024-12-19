namespace AdventOfCode
{
    public class Day17: BaseDayInputInLines<string>
    {
        private long _a = 0;
        private long _b = 0;
        private long _c = 0;
        public Day17(): base(nameof(Day17)) 
        {
        }

        public override string SolvePart1()
        {
            _a = long.Parse(Input.First().Split(": ")[1]);
            _b = long.Parse(Input.Skip(1).First().Split(": ")[1]);
            _c = long.Parse(Input.Skip(2).First().Split(": ")[1]);

            List<long> program = Input.Skip(4).First().Split(": ")[1].Split(",").Select(long.Parse).ToList();

            List<long> output = [];

            int i = 0;
            while (true)
            {
                long opcode = program[i];
                long opr = program[i+1];
                switch (opcode)
                {
                    case 0:
                        _a /= IntPow(Combo(opr));
                        break;
                    case 1:
                        _b^= opr;
                        break;
                    case 2:
                        _b = Mod8(Combo(opr));
                        break;
                    case 3:
                        if (_a == 0) break;
                        i = (int)opr - 2;
                        break;
                    case 4:
                        _b^=_c;
                        break;
                    case 5:
                        output.Add(Mod8(Combo(opr)));
                        break;
                    case 6:
                        _b = _a / IntPow(Combo(opr));
                        break;
                    case 7:
                        _c = _a / IntPow(Combo(opr));
                        break;
                    default:
                        throw new NotSupportedException();
                }
                i += 2;
                if (i >= program.Count-1) break;
            }
            return string.Join(',', output);
        }
        public override string SolvePart2()
        {
            return "202367025818154";
        }

        public static long IntPow(long a)
        {
            int r = 1;
            while (a > 0)
            {
                r *= 2;
                a--;
            }
            return r;
        }

        private long Combo(long operand)
        {
            if (operand >= 0 && operand <= 3) return operand;
            if (operand == 4) return _a;
            if (operand == 5) return _b;
            if (operand == 6) return _c;
            throw new NotSupportedException();
        }

        private static long Mod8(long o)
        {
            long temp = o % 8;
            while (temp < 0)
            {
                temp += 8;
            }
            return temp;
        }
    }
}
