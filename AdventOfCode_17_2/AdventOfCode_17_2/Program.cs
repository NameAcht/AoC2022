using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_17
{
    public class Coordinate
    {
        private int y;
        private int x;
        public Coordinate()
        {
            y = 0;
            x = 0;
        }
        public Coordinate(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public override string ToString() => X.ToString() + ";" + Y.ToString();

    }
    class Program
    {
        enum Rocks
        {
            minus,
            cross,
            L,
            I,
            square
        }
        public static char[] InitizializeNewLine()
        {
            char[] newLine = new char[7];
            for (int i = 0; i < newLine.Length; i++)
                newLine[i] = '.';
            return newLine;
        }

        public static char[] InitizializeMinusLine(int chamberSize, List<Coordinate> rockPos)
        {
            char[] newLine = new char[7];
            for (int i = 0; i < newLine.Length; i++)
            {
                if (i >= 1 && i <= 4)
                {
                    newLine[i] = '@';
                    rockPos.Add(new Coordinate(chamberSize, i));
                }
                else
                    newLine[i] = '.';
            }
            return newLine;
        }

        public static List<char[]> InitializeCrossLines(int chamberSize, List<Coordinate> rockPos)
        {
            List<char[]> lines = new List<char[]>();
            lines.Add(InitizializeNewLine());
            lines[0][3] = '@';
            rockPos.Add(new Coordinate(chamberSize, 3));
            lines.Add(InitizializeNewLine());
            lines[1][2] = '@';
            lines[1][3] = '@';
            lines[1][4] = '@';
            rockPos.Add(new Coordinate(chamberSize + 1, 2));
            rockPos.Add(new Coordinate(chamberSize + 1, 3));
            rockPos.Add(new Coordinate(chamberSize + 1, 4));
            lines.Add(InitizializeNewLine());
            lines[2][3] = '@';
            rockPos.Add(new Coordinate(chamberSize + 2, 3));
            return lines;
        }

        public static List<char[]> InitializeLlines(int chamberSize, List<Coordinate> rockPos)
        {
            List<char[]> lines = new List<char[]>();
            lines.Add(InitizializeNewLine());
            lines[0][2] = '@';
            lines[0][3] = '@';
            lines[0][4] = '@';
            rockPos.Add(new Coordinate(chamberSize, 2));
            rockPos.Add(new Coordinate(chamberSize, 3));
            rockPos.Add(new Coordinate(chamberSize, 4));
            lines.Add(InitizializeNewLine());
            lines[1][2] = '@';
            rockPos.Add(new Coordinate(chamberSize + 1, 2));
            lines.Add(InitizializeNewLine());
            lines[2][2] = '@';
            rockPos.Add(new Coordinate(chamberSize + 2, 2));
            return lines;
        }

        public static List<char[]> InitializeILines(int chamberSize, List<Coordinate> rockPos)
        {
            List<char[]> lines = new List<char[]>();
            for (int i = 0; i < 4; i++)
            {
                lines.Add(InitizializeNewLine());
                lines[i][4] = '@';
                rockPos.Add(new Coordinate(chamberSize + i, 4));
            }
            return lines;
        }

        public static List<char[]> InitializeSquareLines(int chamberSize, List<Coordinate> rockPos)
        {
            var lines = new List<char[]>();

            lines.Add(InitizializeNewLine());
            lines[0][3] = '@';
            lines[0][4] = '@';
            rockPos.Add(new Coordinate(chamberSize, 3));
            rockPos.Add(new Coordinate(chamberSize, 4));
            lines.Add(InitizializeNewLine());
            lines[1][3] = '@';
            lines[1][4] = '@';
            rockPos.Add(new Coordinate(chamberSize + 1, 3));
            rockPos.Add(new Coordinate(chamberSize + 1, 4));

            return lines;
        }
        public static List<char[]> SpawnRock(int cycle, int chamberSize, List<Coordinate> rockPos)
        {
            var newLines = new List<char[]>();

            newLines.Add(InitizializeNewLine());
            newLines.Add(InitizializeNewLine());
            newLines.Add(InitizializeNewLine());
            chamberSize += 3;

            switch (cycle)
            {
                case (int)Rocks.minus:
                    newLines.Add(InitizializeMinusLine(chamberSize, rockPos));
                    break;
                case (int)Rocks.cross:
                    foreach (char[] line in InitializeCrossLines(chamberSize, rockPos))
                        newLines.Add(line);
                    break;
                case (int)Rocks.L:
                    foreach (char[] line in InitializeLlines(chamberSize, rockPos))
                        newLines.Add(line);
                    break;
                case (int)Rocks.I:
                    foreach (char[] line in InitializeILines(chamberSize, rockPos))
                        newLines.Add(line);
                    break;
                case (int)Rocks.square:
                    foreach (char[] line in InitializeSquareLines(chamberSize, rockPos))
                        newLines.Add(line);
                    break;
            }

            return newLines;
        }

        public static void PrintChamber(List<char[]> chamber)
        {
            foreach (char[] line in chamber)
            {
                foreach (char c in line)
                    Console.Write(c);
                Console.WriteLine();
            }
        }

        public static void PrintChamberFlipped(List<char[]> chamber)
        {
            for (int i = chamber.Count - 1; i >= 0; i--)
            {
                for (int j = chamber[i].Length - 1; j >= 0; j--)
                    Console.Write(chamber[i][j]);
                Console.WriteLine();
            }
        }

        public static void MoveJet(List<char[]> chamber, char leftRight, List<Coordinate> rockPos)
        {
            bool move = true;
            bool goesRight = true;
            if (leftRight == '<')
                goesRight = false;
            foreach (var coordinate in rockPos)
            {
                if (goesRight)
                {
                    if (coordinate.X == 0)
                    {
                        move = false;
                        return;
                    }
                    if (chamber[coordinate.Y][coordinate.X - 1] == '#')
                    {
                        move = false;
                        return;
                    }
                    continue;
                }
                else if (coordinate.X == 6)
                {
                    move = false;
                    return;
                }
                if (chamber[coordinate.Y][coordinate.X + 1] == '#')
                {
                    move = false;
                    return;
                }
            }
            if (move)
                if (goesRight)
                {
                    for (int i = 0; i < rockPos.Count; i++)
                    {
                        chamber[rockPos[i].Y][rockPos[i].X] = '.';
                        rockPos[i].X--;
                    }
                }
                else
                {
                    for (int i = 0; i < rockPos.Count; i++)
                    {
                        chamber[rockPos[i].Y][rockPos[i].X] = '.';
                        rockPos[i].X++;
                    }
                }
                for (int i = 0; i < rockPos.Count; i++)
                    chamber[rockPos[i].Y][rockPos[i].X] = '@';
        }

        public static bool MoveRockDown(List<char[]> chamber, List<Coordinate> rockPos)
        {
            bool rockRests = false;
            foreach (var coordinate in rockPos)
            {
                if (coordinate.Y == 0)
                    rockRests = true;
                else if (chamber[coordinate.Y - 1][coordinate.X] == '#')
                    rockRests = true;
            }

            if (rockRests)
            {
                foreach (var coordinate in rockPos)
                    chamber[coordinate.Y][coordinate.X] = '#';
                return true;
            }


            for (int i = 0; i < rockPos.Count; i++)
            {
                chamber[rockPos[i].Y][rockPos[i].X] = '.';
                rockPos[i].Y--;
            }
            for (int i = 0; i < rockPos.Count; i++)
                chamber[rockPos[i].Y][rockPos[i].X] = '@';
            return false;
        }

        public static void ClearEmptyLines(List<char[]> chamber)
        {
            for (int i = chamber.Count - 1; !chamber[i].Contains('#'); i--)
                chamber.Remove(chamber[i]);
        }

        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            string input = System.IO.File.ReadAllText("input.txt");

            var chamber = new List<char[]>();
            var chamberCycle = new List<char[]>();
            int startCycleDetection = 0;
            int firstJetCycle = -1;
            bool cycleFound = false;
            int rocksPerCycle = 0;
            int i = 0;

            int cycle = 0;
            int jetCycle = 0;
            char[] emptyLine = InitizializeNewLine();

            List<Coordinate> rockPos = new List<Coordinate>();
            for (i = 0; i < 20000; i++)
            {
                if (i > 499 && !cycleFound)
                {
                    if (firstJetCycle == -1)
                        firstJetCycle = jetCycle % input.Length;
                    if (cycle % 5 == 0 && jetCycle % input.Length == firstJetCycle && startCycleDetection > 0)
                    {
                        for (int j = startCycleDetection; j < chamber.Count; j++)
                            chamberCycle.Add(chamber[j]);
                        break;
                    }
                    if (cycle % 5 == 0 && jetCycle % input.Length == firstJetCycle && startCycleDetection == 0)
                        startCycleDetection = chamber.Count;
                }

                ///////////////////////////////////////////////////////////////////////////

                int chamberSize = chamber.Count;
                rockPos.Clear();
                chamber.AddRange(SpawnRock(cycle % 5, chamberSize, rockPos));
                bool rockRests = false;
                while (!rockRests)
                {
                    MoveJet(chamber, input[jetCycle % input.Length], rockPos);
                    rockRests = MoveRockDown(chamber, rockPos);
                    jetCycle++;
                }
                ClearEmptyLines(chamber);
                cycle++;
                if (firstJetCycle > -1)
                    rocksPerCycle++;
            }
            long rocksFallen = i;
            long toStackTo = 1000000000000 - rocksFallen;
            toStackTo /= rocksPerCycle;
            toStackTo *= rocksPerCycle;
            long height = toStackTo / rocksPerCycle;
            height *= chamberCycle.Count;
            height += chamber.Count;
            rocksFallen += toStackTo;
            var topChamber = new List<char[]>();
            jetCycle = firstJetCycle;
            for (; rocksFallen < 1000000000000; rocksFallen++)
            {
                int chamberSize = topChamber.Count;
                rockPos.Clear();
                topChamber.AddRange(SpawnRock(cycle % 5, chamberSize, rockPos));
                bool rockRests = false;
                while (!rockRests)
                {
                    MoveJet(topChamber, input[jetCycle % input.Length], rockPos);
                    rockRests = MoveRockDown(topChamber, rockPos);
                    jetCycle++;
                }
                ClearEmptyLines(topChamber);
                cycle++;
            }
            height += topChamber.Count;
            Console.WriteLine(height);
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
