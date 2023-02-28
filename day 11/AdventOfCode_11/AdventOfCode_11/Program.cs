using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_11
{
    class Program
    {
        public class Monkey
        {
            public List<int> items;
            public int inspections;
            public Monkey()
            {
                items = new List<int>();
                inspections = 0;
            }
            public Monkey(List<int> items)
            {
                this.items = items;
                inspections = 0;
            }
        }
        public static List<Monkey> ParseMonkeys(string[] input)
        {
            List<int> currentListOfItems = new List<int>();
            List<Monkey> monkeys = new List<Monkey>();
            for (int i = 1; i < input.Length; i += 7)                                   //Parse monkey
            {
                string itemLine = input[i];
                string[] tempLine = itemLine.Split(':');
                string[] itemList = tempLine[1].Split(',');
                currentListOfItems = new List<int>();
                for (int itemIndex = 0; itemIndex < itemList.Length; itemIndex++)
                    currentListOfItems.Add(Convert.ToInt32(itemList[itemIndex]));
                monkeys.Add(new Monkey(currentListOfItems));
            }
            return monkeys;
        }
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Monkey> allMonkeys = ParseMonkeys(input);
            for (int roundIndex = 0; roundIndex < 20; roundIndex++)                     //Number of monkey rounds
            {
                for (int i = 2; i < input.Length; i += 7)                               //Monkey operations
                {
                    Monkey currentMonkey = allMonkeys[i / 7];
                    string operationLine = input[i];
                    string testLine = input[i + 1];
                    string trueLine = input[i + 2];
                    string falseLine = input[i + 3];

                    int firstOperand = 0;
                    int secondOperand = 0;
                    string op = "";
                    int newWorryLevel = 0;
                    int testDivider = 0;
                    string[] splitOperationLine = operationLine.Split(' ');
                    foreach (int item in currentMonkey.items)                           //Monkey inspects item
                    {
                        if (splitOperationLine[5] == "old")
                            firstOperand = item;
                        else
                            firstOperand = Convert.ToInt32(splitOperationLine[5]);
                        if (splitOperationLine[7] == "old")
                            secondOperand = item;
                        else
                            secondOperand = Convert.ToInt32(splitOperationLine[7]);
                        op = splitOperationLine[6];
                        switch (op)
                        {
                            case "*": newWorryLevel = (firstOperand * secondOperand) / 3; break;
                            case "+": newWorryLevel = (firstOperand + secondOperand) / 3; break;
                            case "/": newWorryLevel = (firstOperand / secondOperand) / 3; break;
                            case "-": newWorryLevel = (firstOperand - secondOperand) / 3; break;
                        }
                        string[] splitTestLine = testLine.Split(' ');
                        testDivider = Convert.ToInt32(splitTestLine[5]);
                        if (newWorryLevel % testDivider == 0)
                        {
                            string[] splitTrueLine = trueLine.Split(' ');
                            allMonkeys[Convert.ToInt32(splitTrueLine[9])].items.Add(newWorryLevel);
                        }
                        else
                        {
                            string[] splitFalseLine = falseLine.Split(' ');
                            allMonkeys[Convert.ToInt32(splitFalseLine[9])].items.Add(newWorryLevel);
                        }
                        currentMonkey.inspections++;
                    }
                    currentMonkey.items.Clear();
                }
            }
            List<int> listOfInspections = new List<int>();
            foreach(Monkey monkee in allMonkeys)
                listOfInspections.Add(monkee.inspections);
            listOfInspections.Sort();
            Console.WriteLine(listOfInspections[listOfInspections.Count - 1] * listOfInspections[listOfInspections.Count - 2]);
            Console.ReadLine();
        }
    }
}
