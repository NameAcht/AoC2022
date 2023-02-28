using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class State
    {
        private int y;
        private int x;
        private Direction direction;
        public State(int y, int x, Direction direction)
        {
            this.y = y;
            this.x = x;
            Direction = direction;
        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public Direction Direction { get => direction; set => direction = value; }
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
            foreach(char c in fieldRow)
            {
                if (c == '.' || c == '#')
                    break;
                x++;
            }
            return x;
        }
        public static int GetLastOfColumn(string[] field, int x)
        {
            int y = 0;
            bool gridFound = false;
            foreach(var row in field)
            {
                try
                {
                    if (row[x] == '.' || row[x] == '#')
                        gridFound = true;
                    if(gridFound && row[x] == ' ')
                        return y - 1;
                    y++;
                }
                catch(IndexOutOfRangeException)
                {
                    return y - 1;
                }
            }
            return y - 1;
        }
        public static int GetFirstOfColumn(string[] field, int x)
        {
            int y = 0;
            foreach(var row in field)
            {
                try
                {
                    if (row[x] == '.' || row[x] == '#')
                        return y;
                    else
                        y++;
                }
                catch(IndexOutOfRangeException)
                {
                    y++;
                }
            }
            return y;
        }
        public static void TryMoveRight(State currentState, string[] field)
        {
            try
            {
                if (field[currentState.Y][currentState.X + 1] == '#')
                    return;
                else
                    currentState.X++;
            }
            catch (IndexOutOfRangeException)
            {
                if (field[currentState.Y][GetFirstOfRow(field[currentState.Y])] == '#')
                    return;
                else
                    currentState.X = GetFirstOfRow(field[currentState.Y]);
            }
            return;
        }
        public static void TryMoveLeft(State currentState, string[] field)
        {
            try
            {
                if (field[currentState.Y][currentState.X - 1] == '#')
                    return;
                else if (field[currentState.Y][currentState.X - 1] == ' ')
                    if (field[currentState.Y][field[currentState.Y].Length - 1] == '#')
                        return;
                    else
                        currentState.X = field[currentState.Y].Length - 1;
                else
                    currentState.X--;
            }
            catch(IndexOutOfRangeException)
            {
                if (field[currentState.Y][field[currentState.Y].Length - 1] != '#')
                    currentState.X = field[currentState.Y].Length - 1;
                else
                    return;
            }
        }
        public static void TryMoveUp(State currentState, string[] field)
        {
            try
            {
                if (field[currentState.Y - 1][currentState.X] == '#')
                    return;
                else if(field[currentState.Y - 1][currentState.X] == '.')
                    currentState.Y--;
                else if (field[GetLastOfColumn(field, currentState.X)][currentState.X] == '#')
                    return;
                else
                    currentState.Y = GetLastOfColumn(field, currentState.X);
            }
            catch (IndexOutOfRangeException)
            {
                if (field[GetLastOfColumn(field, currentState.X)][currentState.X] == '#')
                    return;
                else
                    currentState.Y = GetLastOfColumn(field, currentState.X);
            }
            return;
        }
        public static void TryMoveDown(State currentState, string[] field)
        {
            try
            {
                if (field[currentState.Y + 1][currentState.X] == '#')
                    return;
                else if(field[currentState.Y + 1][currentState.X] == '.')
                    currentState.Y++;
                else if (field[GetFirstOfColumn(field, currentState.X)][currentState.X] == '#')
                    return;
                else
                    currentState.Y = GetFirstOfColumn(field, currentState.X);
            }
            catch(IndexOutOfRangeException)
            {
                if (field[GetFirstOfColumn(field, currentState.X)][currentState.X] == '#')
                    return;
                else
                    currentState.Y = GetFirstOfColumn(field, currentState.X);
            }
        }
        public static void TryMove(State currentState, string[] field)
        {
            switch(currentState.Direction)
            {
                case Direction.right:
                    TryMoveRight(currentState, field); break;
                case Direction.left:
                    TryMoveLeft(currentState, field); break;
                case Direction.up:
                    TryMoveUp(currentState, field); break;
                case Direction.down:
                    TryMoveDown(currentState, field); break;
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
            foreach(var order in orderList)
            {
                int moveAmount = Convert.ToInt32(order.Split(rl)[0].Trim());
                for (int i = 0; i < moveAmount; i++)
                    TryMove(currentState, field);
                if (order[order.Length - 1] == 'R')
                    currentState.Direction = (Direction)Mod((int)currentState.Direction + 1, 4);
                else if(order[order.Length - 1] == 'L')
                    currentState.Direction = (Direction)Mod((int)currentState.Direction - 1, 4);
            }
            Console.WriteLine((currentState.Y + 1) * 1000 + (currentState.X + 1) * 4 + (int)currentState.Direction);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
