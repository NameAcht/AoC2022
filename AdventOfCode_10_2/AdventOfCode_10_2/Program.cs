using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            int currentCycle = 0;
            int x = 1;
            char[][] CRT = new char[6][];


            for (int i = 0; i < CRT.Length; i++)
            {
                CRT[i] = new char[40];
                for (int i2 = 0; i2 < 40; i2++)
                {
                    CRT[i][i2] = '.';
                }
            }


            foreach (string line in input)
            {
                if (line == "noop")
                {
                    currentCycle++;
                    if (Math.Max(x, currentCycle % 40 - 1) - Math.Min(x, currentCycle % 40 - 1) <= 1)
                    {
                        CRT[(currentCycle - 1) / 40][(currentCycle - 1) % 40] = '#';
                    }
                }

                if (line.Contains("addx"))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        currentCycle++;
                        if (Math.Max(x, currentCycle % 40 - 1) - Math.Min(x, currentCycle % 40 - 1) <= 1)
                        {
                            CRT[(currentCycle - 1) / 40][(currentCycle - 1) % 40] = '#';
                        }    
                    }
                    x += Convert.ToInt32(line.Split(' ')[1]);
                }
            }
            

            foreach(char[] pixelLine in CRT)
            {
                Console.WriteLine(pixelLine);
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
