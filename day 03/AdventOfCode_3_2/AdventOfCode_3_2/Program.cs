using System;
using System.Linq;
using System.IO;


namespace AdventOfCode_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ListOfBadges = "";
            string[] input = File.ReadAllLines("input.txt");
            int sum = 0;
            for (int i = 0; i < input.Length; i += 3)
                foreach (char c in input[i])
                    if (input[i + 1].Contains(c))
                        if (input[i + 2].Contains(c))
                        {
                            ListOfBadges += c;
                            break;
                        }
            foreach (char c in ListOfBadges)
            {
                if (c == char.ToUpper(c))
                    sum += (byte)c - 38;
                else
                    sum += (byte)c - 96;
            }
            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
