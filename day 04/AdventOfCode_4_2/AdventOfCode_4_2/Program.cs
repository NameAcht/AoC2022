namespace AdventOfCode_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int sum = 0;
            string[] stringIDs;
            int[] IDs = new int[4];
            foreach (var line in input)
            {
                stringIDs = line.Split(',', '-');
                for (int i = 0; i < stringIDs.Length; i++)
                    IDs[i] = Convert.ToInt32(stringIDs[i]);
                if (IDs[0] <= IDs[2] && IDs[1] >= IDs[2] || IDs[2] <= IDs[0] && IDs[3] >= IDs[0])
                    sum++;
            }
            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
