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
                    case 'X':
                        score++;
                        if (line[0] == 'A')
                            score += 3;
                        if (line[0] == 'B')
                            break;
                        if (line[0] == 'C')
                            score += 6;
                        break;

                    case 'Y':
                        score += 2;
                        if (line[0] == 'A')
                            score += 6;
                        if (line[0] == 'B')
                            score += 3;
                        if (line[0] == 'C')
                            break;
                        break;

                    case 'Z':
                        score += 3;
                        if (line[0] == 'A')
                            break;
                        if (line[0] == 'B')
                            score += 6;
                        if (line[0] == 'C')
                            score += 3;
                        break;
                }
            }
            Console.WriteLine(score);
            Console.ReadLine();
        }
    }
}
