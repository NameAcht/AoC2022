using System;
using System.Collections.Generic;
namespace AdventOfCode_6
{
    class Program
    {
        static void Main(string[] args)
        {
            string signal = System.IO.File.ReadAllText("input.txt");
            List<char> message = new List<char>();
            int i;
            for (i = 0; i < signal.Length; i++)
            {
                int index = message.FindIndex(c => c == signal[i]);
                if (index != -1) message.RemoveRange(0, index + 1);
                message.Add(signal[i]);
                if (message.Count == 14) break;
            }
            Console.WriteLine(i + 1);
            Console.ReadKey();
        }
    }
}
