namespace AdventOfCodeDay1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int max = 0;
            int curr = 0;
            foreach(var line in input)
                if (int.TryParse(line, out int cal))
                    curr += cal;
                else {
                    max = Math.Max(max, curr);
                    curr = 0;
                }
            Console.WriteLine(max);
            Console.ReadLine();
        }
    }
}