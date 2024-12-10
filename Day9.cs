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

            int lastPointer = _disk.Count - 1;
            long curId = _disk[lastPointer];
            while (lastPointer > 0)
            {
                (lastPointer, curId) = FormatDisk(lastPointer, curId);
            }

            return CheckSum();
        }

        private (int, long) FormatDisk(int lastPointer, long curId)
        {
            while (_disk[lastPointer] == -1 || _disk[lastPointer] > curId)
            {
                lastPointer--;
                if (lastPointer < 0) return (-1, curId);
            }
            curId = _disk[lastPointer];
            int len = LengthOfBlock(lastPointer, curId);
            if (len > 0)
            {
                int firstEmptyBlockIndex = FirstEmptyBlock(len, lastPointer);
                if (firstEmptyBlockIndex != -1) MoveBlock(firstEmptyBlockIndex, lastPointer, len);
            }
            return (lastPointer - len, curId);
        } 

        private void MoveBlock(int firstPointer, int lastPointer, long len)
        {
            for (int i = 0; i < len; i++)
            {
                _disk[firstPointer + i] = _disk[lastPointer - i];
                _disk[lastPointer - i] = -1;
            }
        }

        private int LengthOfBlock(int pointer, long curId)
        {
            int len = 0;
            while (_disk[pointer] == curId)
            {
                len++;
                pointer--;
                if (pointer < 0) return len;
            }
            return len;
        }

        private int FirstEmptyBlock(long len, int lastPointer)
        {
            for (int i = 0; i <= lastPointer-len; i++)
            {
                if (_disk[i] == -1)
                {
                    int j = i;
                    while (_disk[j] == -1 && j >= 0)
                    {
                        j--;
                    }
                    if (i - j >= len)
                    {
                        return j + 1;
                    }
                }
            }
            return -1;
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
