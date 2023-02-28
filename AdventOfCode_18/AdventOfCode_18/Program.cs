using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AdventOfCode_18
{
    class Program
    {
        public class Cube
        {
            public Cube(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            public bool IsAdjacentTo(Cube toCheck)
            {
                if (Math.Max(X + Y + Z, toCheck.X + toCheck.Y + toCheck.Z) - Math.Min(X + Y + Z, toCheck.X + toCheck.Y + toCheck.Z) == 1)
                {
                    if (X == toCheck.X && Y == toCheck.Y || X == toCheck.X && Z == toCheck.Z || Y == toCheck.Y && Z == toCheck.Z)
                        return true;
                    else return false;
                }
                else
                    return false;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }
        public static List<Cube> ParseCubes(string[] input)
        {
            var result = new List<Cube>();
            foreach (var line in input)
            {
                string[] coordinates = line.Split(',');
                Cube currentCube = new Cube(Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1]), Convert.ToInt32(coordinates[2]));
                result.Add(currentCube);
            }
            return result;
        } 
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var magmaCubes = ParseCubes(input);
            int result = 0;
            foreach (var cube in magmaCubes)
            {
                result += 6;
                result -= magmaCubes.FindAll(findCube => findCube.IsAdjacentTo(cube)).Count;
            }
            Console.WriteLine(result);
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
