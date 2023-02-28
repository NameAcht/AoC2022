
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AdventOfCode_16_2
{
    public class Node
    {
        public Node(int flowRate, List<string> adjacent, string name)
        {
            Name = name;
            FlowRate = flowRate;
            Adjacent = adjacent;
            Distances = new Dictionary<Node, int>();
        }
        public int FlowRate { get; set; }
        public string Name { get; set; }
        internal Dictionary<Node, int> Distances { get; set; }
        internal List<string> Adjacent { get; set; }
        public override string ToString()
        {
            string adjacent = "";
            foreach (var s in Adjacent)
                adjacent += s + " ";
            return Name + FlowRate + adjacent;
        }
        public void DistanceBFS(Dictionary<string, Node> nodes)
        {
            HashSet<Node> marked = new HashSet<Node>();
            int distance = 1;
            List<Node> tempNeighbors = new List<Node>();
            var BFSList = new List<Node>(); BFSList.Add(this);
            marked.Add(this);
            while (BFSList.Any())
            {
                foreach(var node in BFSList)
                    foreach(var strNeighbor in node.Adjacent)
                    {
                        nodes.TryGetValue(strNeighbor, out Node neighbor);
                        if(marked.Add(neighbor))
                        {
                            if (neighbor.FlowRate > 0 || neighbor.Name == "AA")
                                Distances.Add(neighbor, distance);
                            tempNeighbors.Add(neighbor);
                        }
                    }
                distance++;
                BFSList.Clear();
                BFSList.AddRange(tempNeighbors);
                tempNeighbors.Clear();
            }
            return;
        }
    }
    public class State
    {
        public Node Node { get; set; }
        public int Minute { get; set; }
        public int Bitmask { get; set; }
        public State(Node node, int minute, int bitmask)
        {
            Node = node;
            Minute = minute;
            Bitmask = bitmask;
        }
    }
    class Program
    {
        public static Dictionary<string, Node> ParseNodes(string[] input)
        {
            var nodes = new Dictionary<string, Node>();
            char[] delim = { '=', ' ', ',', ';' };
            foreach (var line in input)
            {
                string[] splitLine = line.Split(delim);
                var adjacent = new List<string>();
                for (int i = splitLine.Length - 1; i >= 11; i -= 2)
                    adjacent.Add(splitLine[i]);
                nodes.Add(splitLine[1], new Node(int.Parse(splitLine[5]), adjacent, splitLine[1]));
            }
            return nodes;
        }
        public static void FindDistances(Dictionary<string, Node> nodes)
        {
            var nodeList = nodes.ToList();
            foreach (var node in nodeList)
                node.Value.DistanceBFS(nodes);
        }
        public static int SumOfNodesLeft(List<Node> nodesLeft, int minutesLeft)
        {
            int result = 0;
            foreach (var node in nodesLeft)
                result += node.FlowRate * minutesLeft;
            return result;
        }
        public static Dictionary<(string, int, int), int> stateCache = new Dictionary<(string, int, int), int>();
        public static int Dfs(State current, Dictionary<Node, int> nodeIndices, List<Node> relevantValves)
        {
            if (stateCache.TryGetValue((current.Node.Name, current.Minute, current.Bitmask), out int v))
                return v;
            int max = 0;
            var distances = current.Node.Distances.ToList();
            for (int i = 0; i < distances.Count; i++)
            {
                int bit = 1 << nodeIndices[distances[i].Key];
                if ((current.Bitmask & bit) != 0)
                    continue;

                int timeRemaining = current.Minute - distances[i].Value - 1;
                if (timeRemaining <= 0)
                    continue;
                max = Math.Max(max, Dfs(new State(distances[i].Key, timeRemaining, current.Bitmask | bit), nodeIndices, relevantValves) + distances[i].Key.FlowRate * timeRemaining);
            }
            stateCache[(current.Node.Name, current.Minute, current.Bitmask)] = max;
            return max;
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var nodes = ParseNodes(input);
            FindDistances(nodes);
            foreach (var node in nodes.ToList())
                if (node.Value.FlowRate == 0 && node.Value.Name != "AA")
                    nodes.Remove(node.Key);
            nodes.TryGetValue("AA", out Node start);
            var list = new List<Node>();
            foreach (var node in nodes.ToList())
                list.Add(node.Value);

            var indices = new Dictionary<Node, int>();
            for (int i = 0; i < list.Count; i++)
                indices.Add(list[i], i);

            int sets = (1 << list.Count) - 1;
            int maxMinutes = 26;
            int max = 0;
            for (int i = 0; i < (sets + 1) / 2; i++)
                max = Math.Max(max, Dfs(new State(start, maxMinutes, i), indices, list) + Dfs(new State(start, maxMinutes, sets ^ i), indices, list));
            
            Console.WriteLine(max);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
