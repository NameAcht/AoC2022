using System;
using System.Linq;
using System.IO;


namespace AdventOfCode_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int sum = 0;
            foreach(var line in input)
            {
                string left = line.Substring(0, line.Length / 2);
                string right = line.Substring(line.Length / 2, line.Length / 2);
                foreach (var c in left)
                    if (right.Contains(c))
                    {
                        if (c == char.ToUpper(c))
                            sum += (byte)c - 38;
                        else
                            sum += (byte)c - 96;
                        break;
                    }
            }
            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
