namespace AdventOfCode
{
    internal class Day24 : BaseDayInputInLines<string>
    {
        private readonly Dictionary<string, bool> _wires = [];
        readonly Dictionary<string, (string In1, string In2, Operation Op)> _wirings = [];

        public Day24() : base(nameof(Day24))
        {
            int skip = 0;
            while (Input.Skip(skip).First().Length != 0)
            {
                var wire = Input.Skip(skip).First().Split(": ");
                string name = wire[0];
                bool value = wire[1] == "1";
                _wires[name] = value;
                skip++;
            }
            skip++;

            foreach (var wiring in Input.Skip(skip))
            {
                var input = wiring.Split(" -> ")[0].Split(' ');
                string output = wiring.Split(" -> ")[1];
                string in1 = input[0];
                Operation op = Enum.Parse<Operation>(input[1]);
                string in2 = input[2];
                _wirings.Add(output, (in1, in2, op));
            }
        }

        public override string SolvePart1()
        {
            bool progress = true;
            long z = 0;
            while (progress)
            {
                progress = false;
                foreach (var wiring in _wirings)
                {
                    if (_wires.TryGetValue(wiring.Value.In1, out bool vIn1) && _wires.TryGetValue(wiring.Value.In2, out bool vIn2) && !_wires.ContainsKey(wiring.Key))
                    {
                        bool v = false;
                        switch (wiring.Value.Op)
                        {
                            case Operation.AND:
                                v = vIn1 && vIn2;
                                break;
                            case Operation.OR:
                                v = vIn1 || vIn2;
                                break;
                            case Operation.XOR:
                                v = vIn1 ^ vIn2;
                                break;
                        }
                        _wires.Add(wiring.Key, v);
                        if (wiring.Key.StartsWith('z') && v)
                        {
                            z |= 1L << int.Parse(wiring.Key[1..]);
                        }
                        progress = true;
                    }
                }
            }

            return z.ToString();
        }

        public override string SolvePart2()
        {
            List<string> swaps = [];
            for(int i = 0; i < 4; i++)
            {
                int bit = Progress();
                bool found = false;
                foreach (var wiring1 in _wirings)
                {
                    foreach (var wiring2 in _wirings)
                    {
                        if (wiring1.Key == wiring2.Key) continue;

                        _wirings[wiring1.Key] = wiring2.Value;
                        _wirings[wiring2.Key] = wiring1.Value;

                        if (Progress() > bit)
                        {
                            found = true;
                            swaps.Add(wiring1.Key);
                            swaps.Add(wiring2.Key);
                            break;  
                        }

                        _wirings[wiring1.Key] = wiring1.Value;
                        _wirings[wiring2.Key] = wiring2.Value;
                    }
                    if (found) break;
                }
            }
            swaps.Sort();
            return string.Join(',', swaps);
        }

        private int Progress()
        {
            for (int i = 0; i < 44; i++)
            {
                string wire = ToWire("z", i);
                if (!VerifyZ(wire, i))
                {
                    return i;
                }
            }
            return 44;
        }

        private string Deconstruct(string wire, int depth)
        {
            if (wire.StartsWith('x') || wire.StartsWith('y'))
            {
                return string.Concat(Enumerable.Repeat(" ", depth)) + wire;
            }
            else
            {
                var (In1, In2, Op) = _wirings[wire];
                return string.Concat(Enumerable.Repeat(" ", depth)) + Op + ": " + wire + "\n" +
                    Deconstruct(In1, depth + 1) + "\n" +
                    Deconstruct(In2, depth + 1);
            }
        }

        private bool VerifyZ(string wire, int bit)
        {
            if (_wirings.TryGetValue(wire, out var wiring))
            {
                if (wiring.Op != Operation.XOR)
                {
                    return false;
                }
                if (bit == 0)
                {
                    return CheckWiring(wiring.In1, wiring.In2, bit);
                }
                return (VerifyIntermediateXor(wiring.In1, bit) && VerifyCarry(wiring.In2, bit))
                    || (VerifyIntermediateXor(wiring.In2, bit) && VerifyCarry(wiring.In1, bit));
            }
            else
            {
                return false;
            }
        }

        private bool VerifyCarry(string wire, int bit)
        {
            if(_wirings.TryGetValue(wire, out var wiring))
            {
                if (bit == 1)
                {
                    return wiring.Op == Operation.AND && CheckWiring(wiring.In1, wiring.In2, 0);
                }
                return wiring.Op == Operation.OR &&
                    ((VerifyDirectCarry(wiring.In1, bit - 1) && VerifyReCarry(wiring.In2, bit - 1)) || (VerifyDirectCarry(wiring.In2, bit - 1) && VerifyReCarry(wiring.In1, bit - 1)));
            }
            else
            {
                return false;
            }
        }

        private bool VerifyDirectCarry(string wire, int bit)
        {
            if (_wirings.TryGetValue(wire, out var wiring))
            {
                return wiring.Op == Operation.AND && CheckWiring(wiring.In1, wiring.In2, bit);
            }
            else
            {
                return false;
            }

        }

        private bool VerifyReCarry(string wire, int bit)
        {
            if (_wirings.TryGetValue(wire, out var wiring))
            {
                return wiring.Op == Operation.AND &&
                    ((VerifyIntermediateXor(wiring.In1, bit) && VerifyCarry(wiring.In2, bit)) || (VerifyIntermediateXor(wiring.In2, bit) && VerifyCarry(wiring.In1, bit)));
            }
            else
            {
                return false;
            }

        }

        private bool VerifyIntermediateXor(string wire, int bit)
        {
            if(_wirings.TryGetValue(wire, out var wiring))
            {
                return wiring.Op == Operation.XOR && CheckWiring(wiring.In1, wiring.In2, bit);
            }
            else
            {
                return false;
            }
        }

        private static bool CheckWiring(string In1, string In2, int bit)
        {
            List<string> actual = [In1, In2];
            List<string> exp = [ToWire("x", bit), ToWire("y", bit)];
            foreach (var e in exp)
            {
                if (!actual.Contains(e))
                {
                    return false;
                }
            }
            return true;
        }

        private static string ToWire(string prefix, int bit)
        {
            return prefix + bit.ToString("D2");
        }

        public enum Operation
        {
            AND,
            OR,
            XOR
        }
    }
}
