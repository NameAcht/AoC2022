using System;
using System.IO;

namespace AdventOfCode_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int sum = 0;
            string[] stringIDs;
            int[] ids = new int[4];
            foreach(string Line in input)
            {
                stringIDs = Line.Split(',', '-');
                for (int i = 0; i < stringIDs.Length; i++)
                    ids[i] = Convert.ToInt32(stringIDs[i]);
                if (ids[0] >= ids[2] && ids[1] <= ids[3] || ids[2] >= ids[0] && ids[1] >= ids[3])
                    sum++;
            }
            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
