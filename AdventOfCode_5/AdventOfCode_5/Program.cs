using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode_5
{
    class Program
    {
        public static Stack<char>[] ParseStacks(string[] input)
        {
            int i = 0;
            for (i = 0; input[i][1] != '1'; i++)
                continue;
            i--;
            var crates = new Stack<char>[(input[i].Length / 4) + 1];
            for (int j = 0; j < crates.Length; j++)
                crates[j] = new Stack<char>();
            for (; i >= 0; i--)
                for (int j = 1; j < input[i].Length; j += 4)
                    if (input[i][j] != ' ')
                        crates[j / 4].Push(input[i][j]);
            return crates;
        }
        public static void MoveCrates(string[] input, Stack<char>[] stacks)
        {
            foreach (var line in input)
                if (line.Contains("m"))
                {
                    var orders = line.Split(' ');
                    int toPop = int.Parse(orders[1]);
                    int fromStack = int.Parse(orders[3]) - 1;
                    int toStack = int.Parse(orders[5]) - 1;
                    for (int i = 0; i < toPop; i++)
                        stacks[toStack].Push(stacks[fromStack].Pop());
                }
        }
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            var stacks = ParseStacks(input);
            MoveCrates(input, stacks);
            foreach (var stack in stacks)
                Console.Write(stack.Peek());
            Console.ReadKey();
        }
    }
}
