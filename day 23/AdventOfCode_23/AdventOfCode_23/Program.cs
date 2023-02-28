using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_23
{
    public enum Directions
    {
        north,
        south,
        west,
        east
    }
    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    internal class Program
    {
        public static bool CheckAdjacent(Coordinate elf, char[][] mutableField)
        {
            for (int column = elf.Y - 1; column <= elf.Y + 1; column++)
                for (int row = elf.X - 1; row <= elf.X + 1; row++)
                    try
                    {
                        if ((elf.X != row || elf.Y != column) && mutableField[column][row] == '#')
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    { continue; }
            return true;
        }
        public static Coordinate GetCoordinate(Coordinate elf, Directions direction)
        {
            switch(direction)
            {
                case Directions.north: return new Coordinate(elf.X, elf.Y - 1);
                case Directions.south: return new Coordinate(elf.X, elf.Y + 1);
                case Directions.west: return new Coordinate(elf.X - 1, elf.Y);
                case Directions.east: return new Coordinate(elf.X + 1, elf.Y);
                default: return null;
            }
        }
        public static bool ProposeMove(Coordinate elf, char[][] mutableField, Directions direction)
        {
            var toCheck = new Coordinate[3];
            switch(direction)
            {
                case Directions.north:
                    toCheck[0] = new Coordinate(elf.X - 1, elf.Y - 1);
                    toCheck[1] = new Coordinate(elf.X, elf.Y - 1);
                    toCheck[2] = new Coordinate(elf.X + 1, elf.Y - 1);
                    break;
                case Directions.south:
                    toCheck[0] = new Coordinate(elf.X - 1, elf.Y + 1);
                    toCheck[1] = new Coordinate(elf.X, elf.Y + 1);
                    toCheck[2] = new Coordinate(elf.X + 1, elf.Y + 1);
                    break;
                case Directions.west:
                    toCheck[0] = new Coordinate(elf.X - 1, elf.Y - 1);
                    toCheck[1] = new Coordinate(elf.X - 1, elf.Y);
                    toCheck[2] = new Coordinate(elf.X - 1, elf.Y + 1);
                    break;
                case Directions.east:
                    toCheck[0] = new Coordinate(elf.X + 1, elf.Y - 1);
                    toCheck[1] = new Coordinate(elf.X + 1, elf.Y);
                    toCheck[2] = new Coordinate(elf.X + 1, elf.Y + 1);
                    break;
            }
            int outOfRangeCount = 0;
            foreach (var proposedField in toCheck)
                try
                {
                    if (mutableField[proposedField.Y][proposedField.X] == '#')
                        return false;
                }
                catch(IndexOutOfRangeException)
                {
                    outOfRangeCount++;
                }
            if (outOfRangeCount == 3)
                return false;
            return true;
        }
        public static List<Coordinate> ParseElves(char[][] field)
        {
            var elves = new List<Coordinate>();
            for (int y = 0; y < field.Length; y++)
                for (int x = 0; x < field[y].Length; x++)
                    if (field[y][x] == '#')
                        elves.Add(new Coordinate(x, y));
            return elves;
        }
        public static char[][] RewriteMap(List<Coordinate> newElfPositions, char[][] mutableField)
        {
            for (int column = 0; column < mutableField.Length; column++)
                for (int row = 0; row < mutableField[column].Length; row++)
                    mutableField[column][row] = '.';
            foreach (var elf in newElfPositions)
                mutableField[elf.Y][elf.X] = '#';
            return mutableField;
        }
        public static void PrintField(char[][] field)
        {
            foreach(var line in field)
            {
                foreach (var c in line)
                    Console.Write(c);
                Console.WriteLine();
            }
        }
        public static int GetGroundTiles(char[][] field)
        {
            int groundTiles = 0;
            foreach(var line in field)
                foreach(var c in line)
                    if(c == '.')
                        groundTiles++;
            return groundTiles;
        }
        public static char[][] GetExpandedField(string[] field)
        {
            var mutableField = new char[field.Length * 5][];
            for (int i = 0; i < mutableField.Length; i++)
                mutableField[i] = new char[field[0].Length * 5];
            int fieldRow = 0;
            for (int mutableFieldRow = mutableField.Length / 2 - field.Length / 2; fieldRow < field.Length; mutableFieldRow++)
            {
                int fieldColumn = 0;
                for (int mutableFieldColumn = mutableField[mutableFieldRow].Length / 2 - field[fieldRow].Length / 2; fieldColumn < field[fieldRow].Length; mutableFieldColumn++)
                {
                    mutableField[mutableFieldRow][mutableFieldColumn] = field[fieldRow][fieldColumn];
                    fieldColumn++;
                }
                fieldRow++;
            }
            return mutableField;
        }

        public static char[][] CollapseField(char[][] mutableField)
        {
            int top = int.MaxValue;
            int bottom = 0;
            int right = 0;
            int left = int.MaxValue;
            for (int row = 0; row < mutableField.Length; row++)
                for (int column = 0; column < mutableField[row].Length; column++)
                    if (mutableField[row][column] == '#')
                    {
                        if (row < top)
                            top = row;
                        else if (row > bottom)
                            bottom = row;
                        if(column < left)
                            left = column;
                        else if (column > right)
                            right = column;
                    }
            var collapsedField = new char[bottom - top + 1][];
            for (int i = 0; i < collapsedField.Length; i++)
            {
                collapsedField[i] = new char[right - left + 1];
                for (int j = 0; j < collapsedField[i].Length; j++)
                    collapsedField[i][j] = mutableField[top + i][left + j];
            }
            return collapsedField;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string[] field = System.IO.File.ReadAllLines("input.txt");
            char[][] mutableField = GetExpandedField(field);
            var elves = ParseElves(mutableField);
            Directions firstProposition = Directions.north;
            List<Coordinate> proposedMoves = new List<Coordinate>();
            List<Coordinate> newElfPositions = new List<Coordinate>();
            for (int i = 0; i < 10; i++)
            {
                for (int elfIter = 0; elfIter < elves.Count; elfIter++)
                {
                    Coordinate elf = elves[elfIter];
                    Directions currentProposition = firstProposition;
                    if (CheckAdjacent(elf, mutableField))
                    {
                        newElfPositions.Add(elf);
                        continue;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        if (ProposeMove(elf, mutableField, currentProposition))
                        {
                            var newPos = GetCoordinate(elf, currentProposition);
                            if (proposedMoves.FindAll(findCoordinate => findCoordinate.X == newPos.X && findCoordinate.Y == newPos.Y).Count == 0)
                            {
                                proposedMoves.Add(newPos);
                                newElfPositions.Add(newPos);
                            }
                            else
                            {
                                int temp = newElfPositions.FindIndex(findCoordinate => findCoordinate.X == newPos.X && findCoordinate.Y == newPos.Y);
                                newElfPositions[temp] = elves[temp];
                            }
                            break;
                        }
                        else
                            currentProposition = (Directions)(((int)currentProposition + 1) % 4);
                    }
                    if (newElfPositions.Count != elfIter + 1)
                        newElfPositions.Add(elf);
                }
                firstProposition = (Directions)(((int)firstProposition + 1) % 4);
                mutableField = RewriteMap(newElfPositions, mutableField);
                elves.Clear();
                elves.AddRange(newElfPositions);
                proposedMoves.Clear();
                newElfPositions.Clear();
            }
            mutableField = CollapseField(mutableField);
            Console.WriteLine(GetGroundTiles(mutableField));
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
