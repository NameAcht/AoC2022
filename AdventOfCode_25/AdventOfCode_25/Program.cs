using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_25
{
    internal class Program
    {
        public static double GetSumOfSNAFUS(string[] input)
        {
            double sumOfSNAFUS = 0;
            for (int lineIndex = 0; lineIndex < input.Length; lineIndex++)
            {
                List<char> line = input[lineIndex].ToList();
                sumOfSNAFUS += SNAFUToDouble(line);
            }
            return sumOfSNAFUS;
        }
        public static double SNAFUToDouble(List<char> SNAFU)
        {
            double power = 0;
            double result = 0;
            for (int charIndex = SNAFU.Count - 1; charIndex >= 0; charIndex--)
            {
                char currChar = SNAFU[charIndex];
                switch (currChar)
                {
                    case '=': result += -2 * Math.Pow(5, power); break;
                    case '-': result += -1 * Math.Pow(5, power); break;
                    case '0': break;
                    case '1': result += Math.Pow(5, power); break;
                    case '2': result += 2 * Math.Pow(5, power); break;
                }
                power++;
            }
            return result;
        }
        public static char SubtractSNAFU(char SNAFUchar)
        {
            switch (SNAFUchar)
            {
                case '-': return '=';
                case '0': return '-';
                case '1': return '0';
                case '2': return '1';
                default: return SNAFUchar;
            }
        }
        public static char AddSNAFU(char SNAFUchar)
        {
            switch (SNAFUchar)
            {
                case '=': return '-';
                case '-': return '0';
                case '0': return '1';
                case '1': return '2';
                default: return SNAFUchar;
            }
        }
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            double sumOfSNAFUS = GetSumOfSNAFUS(input);
            List<char> resultSNAFU = new List<char>();
            while (SNAFUToDouble(resultSNAFU) < sumOfSNAFUS) 
                resultSNAFU.Add('2');
            for (int i = 0; i < resultSNAFU.Count; i++)
            {
                while (SNAFUToDouble(resultSNAFU) > sumOfSNAFUS)
                {
                    resultSNAFU[i] = SubtractSNAFU(resultSNAFU[i]);
                    if (resultSNAFU[i] == '=')
                        break;
                }
                double temp = SNAFUToDouble(resultSNAFU);
                
                if (temp == sumOfSNAFUS)
                    break;

                if (resultSNAFU[i] == '=' && temp > sumOfSNAFUS)
                    continue;
                
                resultSNAFU[i] = AddSNAFU(resultSNAFU[i]);
            }
            foreach(char c in resultSNAFU)
                Console.Write(c);
            Console.ReadKey();
        }
    }
}
