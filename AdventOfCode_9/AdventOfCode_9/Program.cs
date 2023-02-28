using System;
using System.Collections.Generic;
namespace AdventOfCode_9
{
    class Program
    {
        class Node
        {
            public Node()
            {
                x = 0;
                y = 0;
            }
            public int x;
            public int y;
            public void move(char direction)
            {
                switch(direction)
                {
                    case 'U': y++; return;
                    case 'D': y--; return;
                    case 'R': x++; return;
                    case 'L': x--; return;
                }
            }
            public void moveToward(Node head)
            {
                bool move = false;
                if (Math.Max(x, head.x) - Math.Min(x, head.x) > 1)
                    move = true;
                if (Math.Max(y, head.y) - Math.Min(y, head.y) > 1)
                    move = true;

                if (move)
                {
                    if (x < head.x)
                        x++;
                    if (x > head.x)
                        x--;
                    if (y < head.y)
                        y++;
                    if (y > head.y)
                        y--;
                }
            }
            public string position()
            {
                return Convert.ToString(x) + ";" + Convert.ToString(y);
            }
        }
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<string> tailPositions = new List<string>();
            Node head = new Node();
            Node tail = new Node();
            Node[] rope = new Node[10];
            for (int i = 0; i < rope.Length; i++)
                rope[i] = new Node();
            tailPositions.Add("0;0");
            foreach(string Line in input)
            {
                string[] split = Line.Split(' ');
                char direction = Convert.ToChar(split[0]);
                int distance = Convert.ToInt32(split[1]);
                for (int i = 0; i < distance; i++)
                {
                    head.move(direction);
                    tail.moveToward(head);
                    if (!tailPositions.Contains(tail.position()))
                        tailPositions.Add(tail.position());
                }
            }
            Console.WriteLine(tailPositions.Count);
            Console.ReadKey();
        }
    }
}
