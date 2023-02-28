using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_11
{
    class Program
    {
        public class Monkey
        {
            public List<ulong> items;
            public ulong inspections;
            public Monkey()
            {
                items = new List<ulong>();
                inspections = 0;
            }
            public Monkey(List<ulong> items)
            {
                this.items = items;
                inspections = 0;
            }
        }
        public static List<Monkey> ParseMonkeys(string[] input, out ulong supermodulo)
        {
            List<ulong> currentListOfItems = new List<ulong>();
            List<Monkey> monkeys = new List<Monkey>();
            supermodulo = 0;
            for (int i = 1; i < input.Length; i += 7)                                 //Parse monkey
            {
                string itemLine = input[i];
                string[] tempLine = itemLine.Split(':');
                string[] tempItems = tempLine[1].Split(',');
                currentListOfItems = new List<ulong>();
                for (int itemIndex = 0; itemIndex < tempItems.Length; itemIndex++)
                    currentListOfItems.Add(Convert.ToUInt64(tempItems[itemIndex]));
                monkeys.Add(new Monkey(currentListOfItems));
                string testDivisor = input[i + 2].Split(' ')[5];
                if (supermodulo == 0)
                    supermodulo += Convert.ToUInt64(testDivisor);
                else
                    supermodulo *= Convert.ToUInt64(testDivisor);
            }
            return monkeys;
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Monkey> allMonkeys = ParseMonkeys(input, out ulong supermodulo);
            for (int roundIndex = 0; roundIndex < 10000; roundIndex++)                  //Number of monkey rounds
            {
                for (int i = 2; i < input.Length; i += 7)                               //Monkey operations
                {
                    Monkey currentMonkey = allMonkeys[i / 7];
                    string operationLine = input[i];
                    string testLine = input[i + 1];
                    string trueLine = input[i + 2];
                    string falseLine = input[i + 3];
                    
                    ulong firstOperand = 0;
                    ulong secondOperand = 0;
                    string op = "";
                    ulong newWorryLevel = 0;
                    ulong testDivider = 0;
                    string[] splitOperationLine = operationLine.Split(' ');
                    foreach (ulong item in currentMonkey.items)                         //Monkey inspects item
                    {
                        if (splitOperationLine[5] == "old")
                            firstOperand = item;
                        else
                            firstOperand = Convert.ToUInt64(splitOperationLine[5]);

                        if (splitOperationLine[7] == "old")
                            secondOperand = item;
                        else
                            secondOperand = Convert.ToUInt64(splitOperationLine[7]);
                        op = splitOperationLine[6];
                        switch (op)
                        {
                            case "*": newWorryLevel = firstOperand * secondOperand; break;
                            case "+": newWorryLevel = firstOperand + secondOperand; break;
                        }
                        string[] splitTestLine = testLine.Split(' ');
                        testDivider = Convert.ToUInt64(splitTestLine[5]);
                        if (newWorryLevel % testDivider == 0)
                        {
                            newWorryLevel %= supermodulo;
                            string[] splitTrueLine = trueLine.Split(' ');
                            allMonkeys[Convert.ToInt32(splitTrueLine[9])].items.Add(newWorryLevel);
                        }
                        else
                        {
                            newWorryLevel %= supermodulo;
                            string[] splitFalseLine = falseLine.Split(' ');
                            allMonkeys[Convert.ToInt32(splitFalseLine[9])].items.Add(newWorryLevel);
                        }
                        currentMonkey.inspections++;
                    }
                    currentMonkey.items.Clear();
                }
            }
            List<ulong> listOfInspections = new List<ulong>();
            foreach (Monkey monkee in allMonkeys)
                listOfInspections.Add(monkee.inspections);
            listOfInspections.Sort();
            Console.WriteLine(listOfInspections[listOfInspections.Count - 1] * listOfInspections[listOfInspections.Count - 2]);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();         
        }
    }
}
