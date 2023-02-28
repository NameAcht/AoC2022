using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode_21
{
    public class Monkey
    {
        private string name;
        private string value;
        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
        public Monkey(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
        public long ObtainValue(List<Monkey> monkeys)
        {
            if(long.TryParse(value, out long a))
                return a;
            else
            {
                string[] mcSplittington = value.Split(new char[] { '*', '+', '-', '/' });
                long monkey1Value = monkeys.Find(findMonkey => findMonkey.Name == mcSplittington[0].Trim()).ObtainValue(monkeys);
                long monkey2Value = monkeys.Find(findMonkey => findMonkey.Name == mcSplittington[1].Trim()).ObtainValue(monkeys);
                switch (value[5])
                {
                    case '*': return monkey1Value * monkey2Value;
                    case '/': return monkey1Value / monkey2Value;
                    case '-': return monkey1Value - monkey2Value;
                    case '+': return monkey1Value + monkey2Value;
                    default: return -1;
                }
            }
        }
    }
    class Program
    {
        public static List<Monkey> ParseMonkeys(string[] input)
        {
            var monkeys = new List<Monkey>();
            foreach (var line in input)
            {
                string[] mcSplittington = line.Split(':');
                monkeys.Add(new Monkey(mcSplittington[0].Trim(), mcSplittington[1].Trim()));
            }
            return monkeys;
        }
        public static bool GetVerhalten(Monkey a, Monkey b, Monkey human, List<Monkey> monkeys)
        {
            long aValue = a.ObtainValue(monkeys);
            long bValue = b.ObtainValue(monkeys);
            human.Value = Convert.ToString(long.Parse(human.Value) + 10);
            long aNewValue = a.ObtainValue(monkeys);
            long bNewValue = b.ObtainValue(monkeys);
            human.Value = Convert.ToString(long.Parse(human.Value) - 10);
            if (Math.Max(aValue, bValue) - Math.Min(aValue, bValue) > Math.Max(aNewValue, bNewValue) - Math.Min(aNewValue, bNewValue))
                return true; 
            else
                return false;
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var monkeys = ParseMonkeys(input);
            var humanMonkey = monkeys.Find(findHuman => findHuman.Name == "humn");
            string a = monkeys.Find(rootMonkey => rootMonkey.Name == "root").Value.Split('+')[0].Trim();
            string b = monkeys.Find(rootMonkey => rootMonkey.Name == "root").Value.Split('+')[1].Trim();
            Monkey monkeyA = monkeys.Find(aMonkey => aMonkey.Name == a);
            Monkey monkeyB = monkeys.Find(bMonkey => bMonkey.Name == b);
            long resultA = monkeyA.ObtainValue(monkeys);
            long resultB = monkeyB.ObtainValue(monkeys);
            bool behavior = GetVerhalten(monkeyA, monkeyB, humanMonkey, monkeys);       //verhalten = true -> increase human value
                                                                                        //verhalten = false -> decrease human value
            long result = 0;
            for (int exponent = 12; exponent >= 0; exponent--)
            {
                resultA = monkeyA.ObtainValue(monkeys);
                resultB = monkeyB.ObtainValue(monkeys);
                bool skip = false;
                while (behavior)
                {
                    humanMonkey.Value = Convert.ToString(long.Parse(humanMonkey.Value) + Math.Pow(10, exponent));
                    long prevResult = resultA - resultB;
                    resultA = monkeyA.ObtainValue(monkeys);
                    resultB = monkeyB.ObtainValue(monkeys);
                    long newResult = resultA - resultB;
                    if (newResult == 0)
                        break;        
                    if (Math.Abs(prevResult) < Math.Abs(newResult))
                    {
                        behavior = !behavior;
                        skip = true;
                        break;
                    }
                }
                while (!behavior && !skip)
                {
                    humanMonkey.Value = Convert.ToString(long.Parse(humanMonkey.Value) - Math.Pow(10, exponent));
                    long prevResult = resultA - resultB;
                    resultA = monkeyA.ObtainValue(monkeys);
                    resultB = monkeyB.ObtainValue(monkeys);
                    long newResult = resultA - resultB;
                    if (newResult == 0)
                        break;
                    if (Math.Abs(prevResult) < Math.Abs(newResult))
                    {
                        behavior = !behavior;
                        skip = true;
                        break;
                    }
                }
                if (monkeyA.ObtainValue(monkeys) - monkeyB.ObtainValue(monkeys) == 0)
                {
                    result = long.Parse(humanMonkey.Value);
                    break;
                }
            }
            Console.WriteLine(humanMonkey.Value);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
