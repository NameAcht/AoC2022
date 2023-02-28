using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode_14
{
    internal class Program
    {
        public class Coordinate
        {
            private int x;
            private int y;
            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }
            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public static List<Coordinate> ParseRocks(string[] input)
        {
            List<Coordinate> result = new List<Coordinate>();
            for (int i = 0; i < input.Length; i++)
            {
                string currentLine = input[i];
                string[] points = currentLine.Split(' ');
                for (int j = 0; j < points.Length; j += 2)
                {
                    string[] coordinates = points[j].Split(',');
                    int x = Convert.ToInt32(coordinates[0]);
                    int y = Convert.ToInt32(coordinates[1]);
                    if(j != 0)
                    {
                        int endOfPathX = Convert.ToInt32(points[j - 2].Split(',')[0]);
                        int endOfPathY = Convert.ToInt32(points[j - 2].Split(',')[1]);
                        if (x < endOfPathX)
                            for (int pathIndex = x + 1; pathIndex <= endOfPathX - 1; pathIndex++)
                                result.Add(new Coordinate(pathIndex, y));
                        if (x > endOfPathX)
                            for (int pathIndex = endOfPathX + 1; pathIndex <= x - 1; pathIndex++)
                                result.Add(new Coordinate(pathIndex, y));
                        if (y < endOfPathY)
                            for (int pathIndex = y + 1; pathIndex <= endOfPathY - 1; pathIndex++)
                                result.Add(new Coordinate(x, pathIndex));
                        if (y > endOfPathY)
                            for (int pathIndex = endOfPathY + 1; pathIndex <= y - 1; pathIndex++)
                                result.Add(new Coordinate(x, pathIndex));
                    }
                    result.Add(new Coordinate(x, y));
                }
            }
            return result;
        }
        public static int CompareY(Coordinate first, Coordinate second) => first.Y.CompareTo(second.Y);
        public static int FindLargestY(List<Coordinate> rocks)
        {
            List<Coordinate> temp = rocks;
            temp.Sort(CompareY);
            return temp[temp.Count - 1].Y;
        }
        public static char[][] InitializeDisplay(int largestY)
        {
            char[][] display = new char[largestY + 1][];
            for (int i = 0; i < display.Length; i++)
            {
                display[i] = new char[1000];
                for (int j = 0; j < display[i].Length; j++)
                    display[i][j] = '.';
            }
            return display;
        }
        public static void PrintDisplay(char[][] display)
        {
            foreach (char[] line in display)
            {
                foreach(char c in line)
                    Console.Write(c);
                Console.WriteLine();
            }
        }
        public static bool AddSand(char[][] display)
        {
            bool rested = false;
            int x = 500;
            int y = 0;
            while(!rested)
            {
                if(y == display.Length - 1)
                    return false;
                if (display[y + 1][x] == '.')
                {
                    y++;
                    continue;
                }
                if (display[y + 1][x - 1] == '.')
                {
                    y++;
                    x--;
                    continue;
                }
                if (display[y + 1][x + 1] == '.')
                {
                    y++;
                    x++;
                    continue;
                }
                rested = true;
                display[y][x] = 'O';
            }
            return true;
        }

        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Coordinate> rocks = ParseRocks(input);
            int largestY;
            int numberOfRestedSand = 0;
            largestY = FindLargestY(rocks);
            Console.WriteLine(largestY);
            char[][] display = InitializeDisplay(largestY);

            foreach(Coordinate rock in rocks)
                display[rock.Y][rock.X] = '#';
            display[0][500] = '+';
            while(AddSand(display))
                numberOfRestedSand++;
            PrintDisplay(display);
            Console.WriteLine(numberOfRestedSand);
            Console.ReadKey();
        }
    }
}
