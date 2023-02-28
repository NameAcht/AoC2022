using System;
using System.Collections.Generic;

namespace AdventOfCode_13
{
    internal class Program
    {
        public class Element
        {
            public bool isList;
            public int number;
            public List<Element> list;
            public Element parent;

            public Element(int number)
            {
                isList = false;
                this.number = number;
            }
            public Element(List<Element> list)
            {
                isList = true;
                this.list = list;
            }
            public Element(List<Element> list, Element parent)
            {
                isList = true;
                this.list = list;
                this.parent = parent;
            }
            public override string ToString()
            {
                string str = "";
                if (isList)
                {
                    str += "[";
                    foreach (Element e in list)
                    {
                        str += e.ToString();
                        str += ",";
                    }
                    str += "]";
                    return str;
                }
                else return (number - 48).ToString();
            }
        }

        public class Pair
        {
            public List<Element> list;
            public Pair()
            {
                list = new List<Element>();
            }
            public override string ToString()
            {
                string str = "";
                foreach(Element e in list)
                {
                    str += e.ToString();
                    str += "\n";
                }
                return str;
            }          
        }
        public static int Compare(Element first, Element second)
        {
            for (int i = 0; i < first.list.Count; i++)
            {
                if (i == second.list.Count)
                    return 0;
                if (i == first.list.Count)
                    return 1;
                if (!first.list[i].isList && !second.list[i].isList)
                {
                    if (first.list[i].number == second.list[i].number)
                        continue;
                    if (first.list[i].number < second.list[i].number)
                        return 1;
                    if (first.list[i].number > second.list[i].number)
                        return 0;
                }
                

                if (!first.list[i].isList && second.list[i].isList)
                {
                    first.list[i].isList = true;
                    first.list[i].list = new List<Element>();
                    int n = first.list[i].number;
                    first.list[i].list.Add(new Element(n));
                }
                if (first.list[i].isList && !second.list[i].isList)
                {
                    second.list[i].isList = true;
                    second.list[i].list = new List<Element>();
                    int n = second.list[i].number;
                    second.list[i].list.Add(new Element(n));
                }


                if (first.list[i].isList && second.list[i].isList)
                {
                    if (Compare(first.list[i], second.list[i]) == 1)
                        return 1;
                    if (Compare(first.list[i], second.list[i]) == 0)
                        return 0;
                }
            }
            if (first.list.Count < second.list.Count)
                return 1;
            return -1;
        }
       
        public static List<Element> ParseElements(string[] input)
        {
            Element currentElement;
            List<Element> elements = new List<Element>();

            for (int i = 0; i < input.Length; i++)
            {
                string currentLine = input[i];
                currentElement = new Element(new List<Element>());

                for (int charIndex = 1; charIndex < currentLine.Length; charIndex++)
                {
                    if (currentLine[charIndex] == '[')
                    {
                        currentElement.list.Add(new Element(new List<Element>(), currentElement));
                        currentElement = currentElement.list[currentElement.list.Count - 1];
                        continue;
                    }

                    if (currentLine[charIndex] == ']')
                    {
                        if (charIndex == currentLine.Length - 1)
                        {
                            elements.Add(currentElement);
                            break;
                        }
                        currentElement = currentElement.parent;
                        continue;
                    }
                    if (currentLine[charIndex] == ',')
                        continue;
                    if (currentLine[charIndex] > 47 && currentLine[charIndex] < 58)
                    {
                        if (currentLine[charIndex] == 49 && currentLine[charIndex + 1] == 48)
                        {
                            charIndex++;
                            currentElement.list.Add(new Element(58));
                            continue;
                        }
                        currentElement.list.Add(new Element(currentLine[charIndex]));
                        continue;
                    }
                }
            }
            return elements;
        }
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Element> elements = ParseElements(input);
            bool changed = true;
            Element temp;
            List<string> packetList = new List<string>();
            int firstPacketIndex = 0;
            int secondPacketIndex = 0;

            elements.Add(new Element(new List<Element>()));
            elements.Add(new Element(new List<Element>()));
            elements[elements.Count - 2].list.Add(new Element(new List<Element>()));
            elements[elements.Count - 1].list.Add(new Element(new List<Element>()));
            elements[elements.Count - 2].list[0].list.Add(new Element(2 + 48));
            elements[elements.Count - 1].list[0].list.Add(new Element(6 + 48));

            while(changed)
            {
                changed = false;
                for (int i = 0; i <= elements.Count - 2; i++)
                {
                    if (Compare(elements[i], elements[i + 1]) == 0)
                    {
                        temp = elements[i];
                        elements[i] = elements[i + 1];
                        elements[i + 1] = temp;
                        changed = true;
                    }
                }
            }
            foreach (var element in elements)
                packetList.Add(element.ToString());

            for (int i = 0; i < packetList.Count; i++)
            {
                int numberCounter = 0;
                string packet = packetList[i];
                if (packet.Contains("[[2,],]") && !packet.Contains("[]"))
                {
                    foreach (char c in packet)
                        if (c > 47 && c < 58)
                            numberCounter++;
                    if (numberCounter == 1)
                        firstPacketIndex = i + 1;
                }
                numberCounter = 0;
                if (packet.Contains("[[6,],]") && !packet.Contains("[]"))
                {
                    foreach (char c in packet)
                        if (c > 47 && c < 58)
                            numberCounter++;
                    if (numberCounter == 1)
                        secondPacketIndex = i + 1;
                }
            }
            Console.WriteLine(firstPacketIndex * secondPacketIndex);
            Console.ReadKey();
        }
    }
}