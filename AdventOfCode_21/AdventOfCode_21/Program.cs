using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (long.TryParse(value, out long a))
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
        public static void PrintMonkeys(List<Monkey> monkeys)
        {
            foreach(var monkey in monkeys)
                Console.WriteLine(monkey.ToString());
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var monkeys = ParseMonkeys(input);
            long result = monkeys.Find(rootMonkey => rootMonkey.Name == "root").ObtainValue(monkeys);
            Console.WriteLine(result);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
