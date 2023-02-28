using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode_22
{
    public enum Direction
    {
        right,
        down,
        left,
        up
    }
    public class Coordinate
    {
        int x;
        int y;
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
    public class State
    {
        public State(int y, int x, Direction direction)
        {
            Y = y;
            X = x;
            Direction = direction;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
        public override string ToString() => X + ";" + Y + ";" + Direction.ToString();
        public bool Overwrite(string[] field, Dictionary<string, State> edgeDictionary)
        {
            edgeDictionary.TryGetValue(ToString(), out State wrapState);
            if (field[wrapState.Y][wrapState.X] == '.')
            {
                X = wrapState.X;
                Y = wrapState.Y;
                Direction = wrapState.Direction;
                return true;
            }
            return false;
        }
        public void TryMoveRight(string[] field, Dictionary<string, State> edgeDictionary)
        {
            try
            {   if (field[Y][X + 1] == '.')
                {   X++;
                    return; }
                else if (field[Y][X + 1] == '#')
                    return; }
            catch { }
            X++; if(!Overwrite(field, edgeDictionary))
            X--;
        }
        public void TryMoveLeft(string[] field, Dictionary<string, State> edgeDictionary)
        {
            try
            {   if (field[Y][X - 1] == '.')
                {   X--;
                    return; }
                else if(field[Y][X - 1] == '#')
                    return; }
            catch { }
            X--; if(!Overwrite(field, edgeDictionary))
            X++;
        }
        public void TryMoveUp(string[] field, Dictionary<string, State> edgeDictionary)
        {
            try
            {   if (field[Y - 1][X] == '.')
                {   Y--;
                    return; }
                else if (field[Y - 1][X] == '#')
                    return; }
            catch { }
            Y--; if(!Overwrite(field, edgeDictionary))
            Y++;
        }
        public void TryMoveDown(string[] field, Dictionary<string, State> edgeDictionary)
        {
            try
            {   if (field[Y + 1][X] == '.')
                {   Y++;
                    return; }
                else if (field[Y + 1][X] == '#')
                    return; }
            catch { }
            Y++; if(!Overwrite(field, edgeDictionary))
            Y--;
        }
        public void TryMove(string[] field, Dictionary<string, State> edgeDictionary)
        {
            switch (Direction)
            {
                case Direction.right:
                    TryMoveRight(field, edgeDictionary); break;
                case Direction.left:
                    TryMoveLeft(field, edgeDictionary); break;
                case Direction.up:
                    TryMoveUp(field, edgeDictionary); break;
                case Direction.down:
                    TryMoveDown(field, edgeDictionary); break;
            }
        }
    }
    class Program
    {
        public static int Mod(int x, int m) => (x % m + m) % m;
        public static List<string> ParseOrders(string orders)
        {
            var orderList = new List<string>();
            string buffer = "";
            foreach (char c in orders)
            {
                buffer += c;
                if (c == 'L' || c == 'R')
                {
                    orderList.Add(buffer);
                    buffer = "";
                }
            }
            orderList.Add(buffer);
            return orderList;
        }
        public static int GetFirstOfRow(string fieldRow)
        {
            int x = 0;
            foreach (char c in fieldRow)
            {
                if (c == '.' || c == '#')
                    break;
                x++;
            }
            return x;
        }
        public static void AddEdge(Direction before, int x, int y, int maxX, int maxY, Direction after, bool connectsForeward, int newX, int newY, int newMaxX, int newMaxY, Dictionary<string, State> edgeDictionary)
        {
            var oldStates = new List<State>();
            for (int row = y; row <= maxY; row++)
                for (int column = x; column <= maxX; column++)
                {
                    oldStates.Add(new State(row, column, before));
                    if (oldStates.Count == 50)
                        break;
                }
            if(connectsForeward)
            {
                int i = 0;
                for (int row = newY; row <= newMaxY; row++)
                    for (int column = newX; column <= newMaxX; column++)
                    {
                        edgeDictionary.Add(oldStates[i].ToString(), new State(row, column, after));
                        i++;
                        if (i == 50)
                            break;
                    }
            }
            else
            {
                int i = 49;
                for (int row = newY; row <= newMaxY; row++)
                    for (int column = newX; column <= newMaxX; column++)
                    {
                        edgeDictionary.Add(oldStates[i].ToString(), new State(row, column, after));
                        i--;
                        if (i == 0)
                            break;
                    }
            }
            oldStates.Clear();
            return;
        }
        public static Dictionary<string, State> ConstructEdges()
        {
            var edgeDictionary = new Dictionary<string, State>();
            AddEdge(Direction.up, 50, -1, 99, -1, Direction.right, true, 0, 150, 0, 199, edgeDictionary);
            AddEdge(Direction.up, 100, -1, 149, -1, Direction.up, true, 0, 199, 49, 199, edgeDictionary);
            AddEdge(Direction.right, 150, 0, 150, 49, Direction.left, false, 99, 100, 99, 149, edgeDictionary);
            AddEdge(Direction.down, 100, 50, 149, 50, Direction.left, true, 99, 50, 99, 99, edgeDictionary);
            AddEdge(Direction.right, 100, 50, 100, 99, Direction.up, true, 100, 49, 149, 49, edgeDictionary);
            AddEdge(Direction.right, 100, 100, 100, 149, Direction.left, false, 149, 0, 149, 49, edgeDictionary);
            AddEdge(Direction.down, 50, 150, 99, 150, Direction.left, true, 49, 150, 49, 199, edgeDictionary);
            AddEdge(Direction.right, 50, 150, 50, 199, Direction.up, true, 50, 149, 99, 149, edgeDictionary);
            AddEdge(Direction.down, 0, 200, 49, 200, Direction.down, true, 100, 0, 149, 0, edgeDictionary);
            AddEdge(Direction.left, -1, 150, -1, 199, Direction.down, true, 50, 0, 99, 0, edgeDictionary);
            AddEdge(Direction.left, -1, 100, -1, 149, Direction.right, false, 50, 0, 50, 49, edgeDictionary);
            AddEdge(Direction.up, 0, 99, 49, 99, Direction.right, true, 50, 50, 50 , 99, edgeDictionary);
            AddEdge(Direction.left, 49, 50, 49, 99, Direction.down, true, 0, 100, 49, 100, edgeDictionary);
            AddEdge(Direction.left, 49, 0, 49, 49, Direction.right, false, 0, 100, 0, 149, edgeDictionary);
            return edgeDictionary;
        }
        public static void PrintField(string[] field)
        {
            foreach(var line in field)
            {
                foreach (var c in line)
                    Console.Write(c);
                Console.WriteLine();
            }    
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] field = File.ReadAllLines("inputField.txt");
            string orders = File.ReadAllText("inputOrders.txt");
            var orderList = ParseOrders(orders);
            var currentState = new State(0, GetFirstOfRow(field[0]), Direction.right);
            char[] rl = { 'R', 'L' };
            var edgeDictionary = ConstructEdges();
            foreach (var order in orderList)
            {
                int moveAmount = Convert.ToInt32(order.Split(rl)[0].Trim());
                for (int i = 0; i < moveAmount; i++)
                    currentState.TryMove(field, edgeDictionary);
                if (order[order.Length - 1] == 'R')
                    currentState.Direction = (Direction)Mod((int)currentState.Direction + 1, 4);
                else if (order[order.Length - 1] == 'L')
                    currentState.Direction = (Direction)Mod((int)currentState.Direction - 1, 4);
            }
            Console.WriteLine((currentState.Y + 1) * 1000 + (currentState.X + 1) * 4 + currentState.Direction);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
