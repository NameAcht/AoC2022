using System.Diagnostics;
namespace AdventOfCode_24
{
    public enum Direction
    {
        right,
        down,
        wait,
        up,
        left
    }
    public class Blizzard
    {
        public Blizzard(int x, int y, Direction direction)
        {
            Direction = direction;
            Y = y;
            X = x;
        }
        public Direction Direction { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public void Move(string[] field)
        {
            switch (Direction)
            {
                case Direction.right:
                    if (field[Y][X + 1] == '#')
                        X = 1;
                    else
                        X++; break;
                case Direction.left:
                    if (field[Y][X - 1] == '#')
                        X = field[Y].Length - 2;
                    else
                        X--; break;
                case Direction.down:
                    if (field[Y + 1][X] == '#')
                        Y = 1;
                    else
                        Y++; break;
                case Direction.up:
                    if (field[Y - 1][X] == '#')
                        Y = field.Length - 2;
                    else
                        Y--
                            ; break;
            }
        }
    }
    internal class Program
    {
        public static int Mod(int x, int m) => (x % m + m) % m;
        public static Dictionary<(int column, int row, Direction), Blizzard> NextBlizzards(Dictionary<(int column, int row, Direction), Blizzard> blizzards, string[] field)
        {
            var nextBlizzards = new Dictionary<(int nextColumn, int nextRow, Direction), Blizzard>();
            var kvpList = blizzards.ToList();
            foreach (var kvp in kvpList)
            {
                var nextBlizzard = new Blizzard(kvp.Value.X, kvp.Value.Y, kvp.Value.Direction);
                nextBlizzard.Move(field);
                nextBlizzards.Add((nextBlizzard.X, nextBlizzard.Y, nextBlizzard.Direction), nextBlizzard);
            }
            return nextBlizzards;
        }
        public static Dictionary<(int column, int row, Direction direction), Blizzard> ParseBlizzards(string[] field)
        {
            var blizzads = new Dictionary<(int column, int row, Direction direction), Blizzard>();
            for (int row = 0; row < field.Length; row++)
                for (int column = 0; column < field[row].Length; column++)
                    switch (field[row][column])
                    {
                        case '.': continue;
                        case '#': continue;
                        case '>': blizzads.Add((column, row, Direction.right), new Blizzard(column, row, Direction.right)); break;
                        case '^': blizzads.Add((column, row, Direction.up), new Blizzard(column, row, Direction.up)); break;
                        case '<': blizzads.Add((column, row, Direction.left), new Blizzard(column, row, Direction.left)); break;
                        case 'v': blizzads.Add((column, row, Direction.down), new Blizzard(column, row, Direction.down)); break;
                        default: break;
                    }
            return blizzads;
        }
        public static bool CallLegalPos(Dictionary<(int, int, Direction), Blizzard> config, (int column, int row) state, string[] field)
        {
            if (state.row < 0 || state.row >= field.Length || field[state.row][state.column] == '#') return false;      //Check for wall
            if (config.ContainsKey((state.column, state.row, Direction.down))) return false;                            //Check for blizzard
            if (config.ContainsKey((state.column, state.row, Direction.right))) return false;
            if (config.ContainsKey((state.column, state.row, Direction.up))) return false;
            if (config.ContainsKey((state.column, state.row, Direction.left))) return false;
            return true;
        }
        public static Dictionary<int, Dictionary<(int, int, Direction), Blizzard>> PrecomputeBlizzards(string[] field, Dictionary<(int, int, Direction), Blizzard> firstBlizzards, int compDepth)
        {
            var precompBlizzards = new Dictionary<int, Dictionary<(int, int, Direction), Blizzard>>();
            var currBlizzards = firstBlizzards;
            for (int i = 0; i < compDepth; i++)
            {
                precompBlizzards.Add(i, currBlizzards);
                currBlizzards = NextBlizzards(currBlizzards, field);
            }
            return precompBlizzards;
        }
        public static void AddNewEntries(Queue<(int column, int row, int minute)> queue, (int column, int row, int minute) cState, Dictionary<int, Dictionary<(int, int, Direction), Blizzard>> blizzConfigs, (int column, int row) endPos, string[] field, HashSet<(int, int, int)> seen)
        {
            var currConf = blizzConfigs[(cState.minute + 1) % blizzConfigs.Count];
            int nextMin = cState.minute + 1;

            if (CallLegalPos(currConf, (cState.column + 1, cState.row), field)) //Add right
                if (seen.Add((cState.column + 1, cState.row, nextMin)))
                    queue.Enqueue((cState.column + 1, cState.row, nextMin));

            if (CallLegalPos(currConf, (cState.column, cState.row + 1), field)) //Add down
                if (seen.Add((cState.column, cState.row + 1, nextMin)))
                    queue.Enqueue((cState.column, cState.row + 1, nextMin));

            if (CallLegalPos(currConf, (cState.column, cState.row), field))     //Add wait
                if (seen.Add((cState.column, cState.row, nextMin)))
                    queue.Enqueue((cState.column, cState.row, nextMin));

            if (CallLegalPos(currConf, (cState.column - 1, cState.row), field)) //Add left
                if (seen.Add((cState.column - 1, cState.row, nextMin)))
                    queue.Enqueue((cState.column - 1, cState.row, nextMin));

            if (CallLegalPos(currConf, (cState.column, cState.row - 1), field)) //Add up
                if (seen.Add((cState.column, cState.row - 1, nextMin)))
                    queue.Enqueue((cState.column, cState.row - 1, nextMin));
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var field = System.IO.File.ReadAllLines("input.txt");
            var blizzConfigs = PrecomputeBlizzards(field, ParseBlizzards(field), 300);
            Queue<(int column, int row, int minute)> queue = new Queue<(int, int, int)>();
            queue.Enqueue((1, 0, 0));
            (int column, int row, int minute) finalState = (field[0].Length - 2, field.Length - 1, 0);
            var seen = new HashSet<(int, int, int)>();
            while (true)
            {
                var cState = queue.Dequeue();
                if (cState.column == finalState.column && cState.row == finalState.row) { 
                    finalState = cState; break; }
                AddNewEntries(queue, cState, blizzConfigs, (finalState.column, finalState.row), field, seen);
            }
            Console.WriteLine(finalState.minute);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
