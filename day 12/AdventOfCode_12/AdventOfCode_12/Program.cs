using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_12
{
    internal class Program
    {
        public class Node
        {
            public int x;
            public int y;
            public int distanceFromStart;
            public int height;
            public List<Node> surroundingNodes;
            public bool isStart;
            public bool isEnd;
            public Node()
            {
                x = 0;
                y = 0;
                distanceFromStart = 1000000;
                surroundingNodes = new List<Node>();
                height = 0;
                isStart = false;
                isEnd = false;
            }
            public Node(int x, int y, int height, bool isStart, bool isEnd)
            {
                this.x = x;
                this.y = y;
                distanceFromStart = 1000000;
                this.height = height;
                surroundingNodes = new List<Node>();
                this.isStart = isStart;
                this.isEnd = isEnd;
            }

            public bool CheckMove(Node toNode)
            {
                if (toNode.height <= this.height + 1)
                    return true;
                else
                    return false;
            }

            public override string ToString()
            {
                return x + "\t" + y + "\t" + height;
            }
        }

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = File.ReadAllLines("input.txt");
            List<Node> nodes = new List<Node>();
            char height = ' ';
            bool isStart = false;
            bool isEnd = false;

            for (int row = 0; row < input.Length; row++)                                //Create nodes
            {
                for (int column = 0; column < input[row].Length; column++)
                {
                    isStart = false;
                    isEnd = false;
                    if (input[row][column] == 'S')
                    {
                        height = 'a';
                        isStart = true;
                    }
                    else if (input[row][column] == 'E')
                    { 
                        height = 'z';
                        isEnd = true;
                    }
                    else
                        height = input[row][column];
                    Node toAdd = new Node(column, row, height, isStart, isEnd);
                    nodes.Add(toAdd);
                }
            }
            foreach(Node n in nodes)                                                    //Create surrounding nodes lists
            {
                if (n.x != 0 && n.x != input[0].Length - 1)
                {
                    n.surroundingNodes.Add(nodes.Find(x => x.x == n.x + 1 && x.y == n.y));
                    n.surroundingNodes.Add(nodes.Find(x => x.x == n.x - 1 && x.y == n.y));
                }
                else if (n.x == 0)
                    n.surroundingNodes.Add(nodes.Find(x => x.x == n.x + 1 && x.y == n.y));
                else
                    n.surroundingNodes.Add(nodes.Find(x => x.x == n.x - 1 && x.y == n.y));


                if (n.y != 0 && n.y != input.Length - 1)
                {
                    n.surroundingNodes.Add(nodes.Find(x => x.y == n.y + 1 && x.x == n.x));
                    n.surroundingNodes.Add(nodes.Find(x => x.y == n.y - 1 && x.x == n.x));
                }
                else if (n.y == 0)
                    n.surroundingNodes.Add(nodes.Find(x => x.y == n.y + 1 && x.x == n.x));
                else
                    n.surroundingNodes.Add(nodes.Find(x => x.y == n.y - 1 && x.x == n.x));
            }
            Node startNode = nodes.Find(x => x.isStart == true);
            startNode.distanceFromStart = 0;
            Node endNode = nodes.Find(x => x.isEnd == true);
            Node currentNode = startNode;
            Node lastChecked = startNode;
            bool changed = false;

            while(endNode.distanceFromStart == 1000000)
                for (int i = 0; i < nodes.Count; i++)
                    foreach (Node surrNode in nodes[i].surroundingNodes)
                        if (nodes[i].CheckMove(surrNode))
                            if (surrNode.distanceFromStart > nodes[i].distanceFromStart + 1)
                                surrNode.distanceFromStart = nodes[i].distanceFromStart + 1;

            Console.WriteLine(endNode.distanceFromStart);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
