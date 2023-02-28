namespace AdventOfCode_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("inputTest.txt");
            var activeDirectories = new Stack<int>();
            int total = 0;
            foreach(var line in input)
                if (int.TryParse(line.Split(' ')[0], out int add))
                    total += add;
            int minSize = total - 40000000;
            int toDelete = int.MaxValue;
            foreach (var line in input) {
                if (line[2] == 'c') {
                    if (line[5] == '.') {
                        int a = activeDirectories.Pop();
                        if (a >= minSize && a < toDelete)
                            toDelete = a;
                        activeDirectories.Push(activeDirectories.Pop() + a);
                    }
                    else
                        activeDirectories.Push(0);
                    continue;
                }
                if (!line.Contains("dir") && !line.Contains("$ ls"))
                    activeDirectories.Push(activeDirectories.Pop() + int.Parse(line.Split(' ')[0]));
            }
            if (activeDirectories.Peek() >= minSize && activeDirectories.Peek() < toDelete)
                toDelete = activeDirectories.Peek();
            Console.WriteLine(toDelete);
            Console.ReadLine();
        }
    }
}