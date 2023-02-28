namespace AdventOfCodeDay1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            var elves = new List<int>();
            int curr = 0;
            foreach (var line in input)
                if (int.TryParse(line, out int cal))
                    curr += cal;
                else {
                    elves.Add(curr);
                    curr = 0;
                }
            elves.Sort();
            Console.WriteLine(elves[elves.Count - 1] + elves[elves.Count - 2] + elves[elves.Count - 3]);
            Console.ReadLine();
        }
    }
}