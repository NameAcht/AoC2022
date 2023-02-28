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
        public struct Side
        {
            public Cube coordinate;
            public int side;
        }
        public enum Sides
        {
            up,
            down,
            left,
            right,
            front,
            back
        }
        public class Cube
        {
            private int x;
            private int y;
            private int z;

            public Cube()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
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
            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }
            public int Z { get => z; set => z = value; }
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
        public static List<Cube> InitializeAirCubes(List<Cube> magmaCubes, int maxX, int maxY, int maxZ)
        {
            List<Cube> airCubes = new List<Cube>();
            for (int x = 0; x <= maxX; x++)
                for (int y = 0; y <= maxY; y++)
                    for (int z = 0; z <= maxZ; z++)
                    {
                        Cube add = new Cube(x, y, z);
                        if (magmaCubes.FindAll(find => find.X == add.X && find.Y == add.Y && find.Z == add.Z).Count == 0)
                            airCubes.Add(add);
                    }
            return airCubes;
        }
        public static void PrintCubes(List<Cube> cubes)
        {
            foreach (var cube in cubes)
                Console.WriteLine(cube.ToString());
        }
        public static Cube[] GetAdjacentCubes(Cube cube, List<Cube> allCubes)
        {
            Cube[] adjacentCubes = new Cube[6];

            adjacentCubes[(int)Sides.back] = allCubes.Find(findCube => findCube.X == cube.X + 1 && findCube.Y == cube.Y && findCube.Z == cube.Z);
            adjacentCubes[(int)Sides.front] = allCubes.Find(findCube => findCube.X == cube.X - 1 && findCube.Y == cube.Y && findCube.Z == cube.Z);
            adjacentCubes[(int)Sides.down] = allCubes.Find(findCube => findCube.X == cube.X && findCube.Y == cube.Y - 1 && findCube.Z == cube.Z);
            adjacentCubes[(int)Sides.up] = allCubes.Find(findCube => findCube.X == cube.X && findCube.Y == cube.Y + 1 && findCube.Z == cube.Z);
            adjacentCubes[(int)Sides.right] = allCubes.Find(findCube => findCube.X == cube.X && findCube.Y == cube.Y && findCube.Z == cube.Z + 1);
            adjacentCubes[(int)Sides.left] = allCubes.Find(findCube => findCube.X == cube.X && findCube.Y == cube.Y && findCube.Z == cube.Z - 1);

            return adjacentCubes;
        }
        public static bool CheckAllSidesFound(Cube cube, List<Cube> magmaCubes, int maxX, int maxY, int maxZ)
        {
            bool[] sidesFound = new bool[6];
            sidesFound.Initialize();

            if (cube == null)
                return false;

            for (int i = cube.X + 1; i <= maxX; i++)
                if (magmaCubes.FindAll(findCube => findCube.X == i && findCube.Y == cube.Y && findCube.Z == cube.Z).Count == 1)
                {
                    sidesFound[(int)Sides.front] = true;
                    break;
                }
            for (int i = cube.X - 1; i >= 0; i--)
                if (magmaCubes.FindAll(findCube => findCube.X == i && findCube.Y == cube.Y && findCube.Z == cube.Z).Count == 1)
                {
                    sidesFound[(int)Sides.back] = true;
                    break;
                }
            for (int i = cube.Y + 1; i <= maxY + 2; i++)
                if (magmaCubes.FindAll(findCube => findCube.X == cube.X && findCube.Y == i && findCube.Z == cube.Z).Count == 1)
                {
                    sidesFound[(int)Sides.up] = true;
                    break;
                }
            for (int i = cube.Y - 1; i >= 0; i--)
                if (magmaCubes.FindAll(findCube => findCube.X == cube.X && findCube.Y == i && findCube.Z == cube.Z).Count == 1)
                {
                    sidesFound[(int)Sides.down] = true;
                    break;
                }
            for (int i = cube.Z + 1; i <= maxZ + 2; i++)
                if (magmaCubes.FindAll(findCube => findCube.X == cube.X && findCube.Y == cube.Y && findCube.Z == i).Count == 1)
                {
                    sidesFound[(int)Sides.left] = true;
                    break;
                }
            for (int i = cube.Z - 1; i >= 0; i--)
                if (magmaCubes.FindAll(findCube => findCube.X == cube.X && findCube.Y == cube.Y && findCube.Z == i).Count == 1)
                {
                    sidesFound[(int)Sides.right] = true;
                    break;
                }
            bool allSidesFound = true;
            foreach (bool foundSide in sidesFound)
                if (!foundSide)
                    allSidesFound = false;

            return allSidesFound;
        }
        public static bool IsInterior(Cube cube, List<Cube> magmaCubes, List<Cube> allCubes, int maxX, int maxY, int maxZ, int searchDepth)
        {
            while(searchDepth < 2)
            {
                if (!CheckAllSidesFound(cube, magmaCubes, maxX, maxY, maxZ))
                    return false;

                Cube[] adjacentCubes = GetAdjacentCubes(cube, allCubes);
                List<Cube> adjacentAirCubes = new List<Cube>();

                foreach (var adjacentCube in adjacentCubes)
                    if (!magmaCubes.Contains(adjacentCube))
                        adjacentAirCubes.Add(adjacentCube);

                foreach (var adjacentAirCube in adjacentAirCubes)
                    if (!IsInterior(adjacentAirCube, magmaCubes, allCubes, maxX, maxY, maxZ, searchDepth + 1))
                        return false;
                return true;
            }
            return true;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var magmaCubes = ParseCubes(input);
            int maxX = 0;
            int maxY = 0;
            int maxZ = 0;

            int result = 0;
            foreach (var cube in magmaCubes)
            {
                if (cube.X > maxX)
                    maxX = cube.X;
                if (cube.Y > maxY)
                    maxY = cube.Y;
                if (cube.Z > maxZ)
                    maxZ = cube.Z;

                result += 6;
                result -= magmaCubes.FindAll(findCube => findCube.IsAdjacentTo(cube)).Count;
            }
            List<Cube> airCubes = InitializeAirCubes(magmaCubes, maxX, maxY, maxZ);
            List<Cube> allCubes = new List<Cube>();
            allCubes.AddRange(magmaCubes);
            allCubes.AddRange(airCubes);
            Console.WriteLine(result);
            List<Cube> interiorCubes = new List<Cube>();
            foreach(var cube in allCubes)
            {
                if (IsInterior(cube, magmaCubes, allCubes, maxX, maxY, maxZ, 0))
                    interiorCubes.Add(cube);
            }
            foreach(var cube in interiorCubes)
            {
                if (!magmaCubes.Contains(cube))
                    magmaCubes.Add(cube);
            }
            result = 0;
            foreach (var cube in magmaCubes)
            {
                result += 6;
                int numberOfAdjacentMagmaCubes = magmaCubes.FindAll(findCube => findCube.IsAdjacentTo(cube)).Count;
                result -= numberOfAdjacentMagmaCubes;
            }
            Console.WriteLine(result);
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
