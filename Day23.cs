namespace AdventOfCode
{
    internal class Day23 : BaseDayInputInLines<string>
    {
        private readonly Dictionary<string, HashSet<string>> _networks = [];
        private List<List<string>> _sets = [];

        public Day23() : base(nameof(Day23))
        {

            foreach (var line in Input)
            {
                var computers = line.Split("-");
                var computer1 = computers[0];
                var computer2 = computers[1];

                if (!_networks.ContainsKey(computer1))
                {
                    _networks[computer1] = [];
                }
                if (!_networks.ContainsKey(computer2))
                {
                    _networks[computer2] = [];
                }

                _networks[computer1].Add(computer2);
                _networks[computer2].Add(computer1);
            }
        }

        public override string SolvePart1()
        {
            HashSet<(string, string, string)> networks2 = [];
            foreach (var network in _networks)
            {
                foreach (var computer in network.Value)
                {
                    foreach (var computer2 in _networks[computer])
                    {
                        if (network.Key != computer2 && _networks[computer2].Contains(network.Key))
                        {
                            string[] newNetwork = [network.Key, computer, computer2];
                            Array.Sort(newNetwork);
                            networks2.Add((newNetwork[0], newNetwork[1], newNetwork[2]));
                        }
                    }
                }
            }

            return networks2.Count(network => network.Item1.StartsWith('t') || network.Item2.StartsWith('t') || network.Item3.StartsWith('t')).ToString();
        }

        public override string SolvePart2()
        {
            foreach (var network in _networks)
            {
                FindNetworks(network.Key, [network.Key]);
            }
            
            return string.Join(",", _sets.OrderByDescending(set => set.Count).First());
        }

        private void FindNetworks(string computer, List<string> network)
        {
            network.Sort();
            if (_sets.Any(s => s.Equals(network)))
            {
                return;
            }
            _sets.Add(network);
            foreach (var connectedComputer in _networks[computer])
            {
                if (network.Contains(connectedComputer))
                {
                    continue;
                }
                if (!network.All(n => _networks[n].Contains(connectedComputer)))
                {
                    continue;
                }
                network.Add(connectedComputer);
                FindNetworks(connectedComputer, network);
            }
        }
    }
}
