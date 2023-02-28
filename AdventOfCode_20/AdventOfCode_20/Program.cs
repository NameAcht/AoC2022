using System.Diagnostics;

namespace AdventOfCode_20
{
    public class SortNumber
    {
        public int value;
        public bool isSorted;
    }
    class Program
    {
        public static int Mod(int x, int m) => (x % m + m) % m;
        public static void MoveNumber(List<SortNumber> circularNumbers, int index)
        {
            int newIndex = Mod(index + circularNumbers[index].value, circularNumbers.Count - 1);
            SortNumber temp = circularNumbers[index];
            SortNumber insert = new SortNumber();
            insert.value = temp.value;
            insert.isSorted = true;
            circularNumbers.Remove(temp);
            circularNumbers.Insert(newIndex, insert);
            circularNumbers.Remove(temp);
            return;
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var circularNumbers = new List<SortNumber>();
            var numbers = new List<int>();
            foreach (string line in File.ReadAllLines("input.txt"))
            {
                var n = new SortNumber();
                n.value = int.Parse(line);
                n.isSorted = false;
                circularNumbers.Add(n);
                numbers.Add(n.value);
            }
            for (int i = 0; i < numbers.Count; i++)
            {
                int numberToMovePos = int.MaxValue;
                numberToMovePos = circularNumbers.FindIndex(findNumber => findNumber.value == numbers[i] && findNumber.isSorted == false);
                MoveNumber(circularNumbers, numberToMovePos);
            }
            int zeroIndex = circularNumbers.FindIndex(findNumber => findNumber.value == 0);
            int a = circularNumbers[Mod(zeroIndex + 1000, numbers.Count)].value;
            int b = circularNumbers[Mod(zeroIndex + 2000, numbers.Count)].value;
            int c = circularNumbers[Mod(zeroIndex + 3000, numbers.Count)].value;
            Console.WriteLine(a + b + c);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
