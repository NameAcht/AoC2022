using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace AdventOfCode_2
{
    internal class Program
    {
        //A = Rock, B = Paper, C = Scissors
        //X = Rock = 1 Point, Y = Paper = 2 Points, Z = Scissors = 3 Points
        static void Main(string[] args)
        {
            int score = 0;
            string[] input = System.IO.File.ReadAllLines("input.txt");
            foreach (var line in input)
            {
                switch (line[2])
                {
                    case 'X': break;
                    case 'Y': score += 3; break;
                    case 'Z': score += 6; break;
                }
                switch (line[0])
                {
                    case 'A':
                        if (line[2] == 'X')
                            score += 3;
                        if (line[2] == 'Y')
                            score += 1;
                        if (line[2] == 'Z')
                            score += 2;
                        break;

                    case 'B':
                        if (line[2] == 'X')
                            score += 1;
                        if (line[2] == 'Y')
                            score += 2;
                        if (line[2] == 'Z')
                            score += 3;
                        break;

                    case 'C':
                        if (line[2] == 'X')
                            score += 2;
                        if (line[2] == 'Y')
                            score += 3;
                        if (line[2] == 'Z')
                            score += 1;
                        break;
                }
            }
            Console.WriteLine(score);
            Console.ReadLine();
        }
    }
}
