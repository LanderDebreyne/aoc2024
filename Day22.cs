using System;
using System.Numerics;
namespace AdventOfCode
{
    public class Day22 : BaseDayInputInLines<long>
    { 
        public Day22() : base(nameof(Day22))
        {
        }
        public override long SolvePart1()
        {
            long result = 0;
            foreach (var line in Input)
            {
                long secret = long.Parse(line);
                for (int i = 0; i < 2000; i++)
                {
                    secret = (secret ^ (secret * 64)) % 16777216;
                    secret = (secret ^ (long)Math.Floor(secret / 32d)) % 16777216;
                    secret = (secret ^ (secret * 2048)) % 16777216;
                }

                result += secret; 
            }
            return result;
        }
        public override long SolvePart2()
        {
            Dictionary<(long, long, long, long), long> totalSeq = [];

            foreach (var line in Input)
            {
                Dictionary<(long,long,long,long), long> seq = [];
                long[] prices = new long[2001];
                long secret = long.Parse(line);
                prices[0] = secret % 10;
                for (int i = 1; i < 2001; i++)
                {
                    secret = (secret ^ (secret * 64)) % 16777216;
                    secret = (secret ^ (long)Math.Floor(secret / 32d)) % 16777216;
                    secret = (secret ^ (secret * 2048)) % 16777216;

                    prices[i] = (secret % 10);
                }
                
                for (int j = 0; j < 2001 - 4; j++)
                {
                    long a = prices[j];
                    long b = prices[j + 1];
                    long c = prices[j + 2];
                    long d = prices[j + 3];
                    long e = prices[j + 4];
                    (long,long,long,long) cSeq = (b-a, c - b, d - c, e - d);
                    if (!seq.ContainsKey(cSeq))
                    {
                        seq[cSeq] = e;
                    }
                }
                foreach (var pair in seq)
                {
                    if (totalSeq.TryGetValue(pair.Key, out long price))
                    {
                            totalSeq[pair.Key] += pair.Value;
                    }
                    else
                    {
                        totalSeq[pair.Key] = pair.Value;
                    }
                }
            }

            long result = 0;
            foreach (var pair in totalSeq)
            {
                if (pair.Value > result)
                {
                    result = pair.Value;
                }
            }

            return result;
        }

    }
}
