using System;

namespace AdventOfCode_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            int currentCycle = 0;
            int x = 1;
            int interestingSignals = 0;
            foreach(string line in input)
            {
                if (line == "noop")
                {
                    currentCycle++;
                    if (currentCycle % 40 == 20 && currentCycle <= 220)
                        interestingSignals += x * currentCycle;
                }
                if(line.Contains("addx"))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        currentCycle++;
                        if (currentCycle % 40 == 20 && currentCycle <= 220)
                            interestingSignals += x * currentCycle;
                    }
                    x += Convert.ToInt32(line.Split(' ')[1]);
                }
            }
            Console.WriteLine(interestingSignals);
            Console.ReadLine();
        }
    }
}
