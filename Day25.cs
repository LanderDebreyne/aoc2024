namespace AdventOfCode
{
    public class Day25 : BaseDayInputInLines<long>
    {
        private readonly List<int[]> _keys = [];
        private readonly List<int[]> _locks = [];
        public Day25() : base(nameof(Day25))
        {
            int skip = 0;
            while (skip < Input.Count() && Input.Skip(skip).First().Length != 0)
            {
                if (Input.Skip(skip).First() == "#####")
                {
                    var key = new int[5];
                    skip++;
                    for (int i = 0; i < 5; i++)
                    {
                        string line = (Input.Skip(skip + i).First());
                        for (int j = 0; j < 5; j++)
                        {
                            if (line[j] == '#')
                            {
                                key[j]++;
                            }
                        }
                    }
                    _keys.Add(key);
                }

                if (Input.Skip(skip).First() == ".....")
                {
                    var cLock = new int[5];
                    skip++;
                    for (int i = 0; i < 5; i++)
                    {
                        string line = (Input.Skip(skip + i).First());
                        for (int j = 0; j < 5; j++)
                        {
                            if (line[j] == '#')
                            {
                                cLock[j]++;
                            }
                        }
                    }
                    _locks.Add(cLock);
                }
                skip += 7;
            }
        }
        public override long SolvePart1()
        {
            long r = 0;
            foreach (var key in _keys)
            {
                foreach (var cLock in _locks)
                {
                    bool match = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if (key[i] + cLock[i] > 5)
                        {
                            match = false;
                        }
                    }
                    if (match)
                    {
                        r++;
                    }
                }
            }
            return r;
        }
        public override long SolvePart2()
        {
            return 0;
        }
    }
}
