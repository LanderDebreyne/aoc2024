namespace AdventOfCode
{
    public class Day9() : BaseDayInputAsString<long>(nameof(Day9))
    {
        private readonly List<long> _disk = [];

        public override long SolvePart1()
        {
            ParseDisk();

            int lastPointer = _disk.Count - 1;
            int firstPointer = 0;
            while (firstPointer < lastPointer)
            {
                if (_disk[firstPointer] == -1)
                {
                    while (_disk[lastPointer] == -1 && firstPointer < lastPointer)
                    {
                        lastPointer--;
                    }
                    if (firstPointer < lastPointer)
                    {
                        _disk[firstPointer] = _disk[lastPointer];
                        _disk[lastPointer] = -1;
                    }
                }
                firstPointer++;
            }
            return CheckSum();
        }

        public override long SolvePart2()
        {
            ParseDisk();

            // TODO

            return CheckSum();
        }

        private void ParseDisk()
        {
            bool isFree = false;
            int id = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                for (int j = 0; j < int.Parse(Input[i].ToString()); j++)
                {
                    if (isFree)
                    {
                        _disk.Add(-1);
                    }
                    else
                    {
                        _disk.Add(id);
                    }
                }
                isFree = !isFree;
                if (isFree) id++;
            }
        }

        private long CheckSum() => _disk.Select((x, i) => x == -1 ? 0 : x * i).Sum();
    }
}
