namespace AdventOfCode_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var activeDirectories = new Stack<int>();
            int result = 0;
            foreach(var line in input) {
                if (line[2] == 'c') {
                    if (line[5] == '.') {
                        int a = activeDirectories.Pop();
                        if (a <= 100000)
                            result += a;
                        activeDirectories.Push(activeDirectories.Pop() + a);
                    }
                    else
                        activeDirectories.Push(0);
                    continue;
                }
                if(!line.Contains("dir") && !line.Contains("$ ls"))
                    activeDirectories.Push(activeDirectories.Pop() + int.Parse(line.Split(' ')[0]));
            }
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}