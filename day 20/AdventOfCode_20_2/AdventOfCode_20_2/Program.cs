using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_20
{
    public class SortNumber
    {
        private long value;
        private int currentSortState;
        public long Value { get => value; set => this.value = value; }
        public int CurrentSortState { get => currentSortState; set => currentSortState = value; }
        public SortNumber()
        {
            value = 0;
            currentSortState = 0;
        }
        public SortNumber(long value, int currentSortState)
        {
            this.value = value;
            this.currentSortState = currentSortState;
        }
    }
    class Program
    {
        public static List<SortNumber> CreateStates(List<long> numbers)
        {
            var states = new List<SortNumber>();
            foreach (var number in numbers)
                if(states.FindAll(findState => findState.Value == number).Count == 0)
                {
                    var toAdd = new SortNumber();
                    toAdd.Value = number;
                    toAdd.CurrentSortState = 0;
                    states.Add(toAdd);
                }
            return states;
        }
        public static void ResetStates(List<SortNumber> states)
        {
            foreach(var state in states)
                state.CurrentSortState = 0;
        }
        public static long Mod(long x, long m) => (x % m + m) % m;
        public static void MoveNumber(List<SortNumber> circularNumbers, long index)
        {
            long newIndex = Mod(index + circularNumbers[(int)index].Value, circularNumbers.Count - 1);
            SortNumber temp = circularNumbers[(int)index];
            SortNumber insert = new SortNumber();
            insert.Value = temp.Value;
            insert.CurrentSortState = temp.CurrentSortState;
            circularNumbers.Remove(temp);
            circularNumbers.Insert((int)newIndex, insert);
            circularNumbers.Remove(temp);
            return;
        }
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var numbers = new List<long>();
            var circularNumbers = new List<SortNumber>();
            long decryptionKey = 811589153;
            foreach (string line in System.IO.File.ReadAllLines("input.txt"))
            {
                var n = new SortNumber(long.Parse(line) * decryptionKey, circularNumbers.FindAll(findNumber => findNumber.Value == long.Parse(line) * decryptionKey).Count);
                numbers.Add(n.Value);
                circularNumbers.Add(n);
            }
            var states = CreateStates(numbers);
            for (int rounds = 0; rounds < 10; rounds++)
            {
                ResetStates(states);
                for (int i = 0; i < numbers.Count; i++)
                {
                    long numberToMovePos = long.MaxValue;
                    var currState = states.Find(findState => findState.Value == numbers[i]);
                    numberToMovePos = circularNumbers.FindIndex(findNumber => findNumber.Value == numbers[i] && findNumber.CurrentSortState == currState.CurrentSortState);
                    currState.CurrentSortState++;
                    MoveNumber(circularNumbers, numberToMovePos);
                }
            }
            long zeroIndex = circularNumbers.FindIndex(findNumber => findNumber.Value == 0);
            long a = circularNumbers[(int)Mod(zeroIndex + 1000, numbers.Count)].Value;
            long b = circularNumbers[(int)Mod(zeroIndex + 2000, numbers.Count)].Value;
            long c = circularNumbers[(int)Mod(zeroIndex + 3000, numbers.Count)].Value;
            Console.WriteLine(a + b + c);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
