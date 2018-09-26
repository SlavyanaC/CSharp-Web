namespace _01EvenNumbersThread
{
    using System;
    using System.Linq;
    using System.Threading;

    class StartUp
    {
        static void Main(string[] args)
        {
            int[] range = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int start = range[0];
            int end = range[1];

            Thread evens = new Thread(() => PrintEvenNumbers(start, end));
            evens.Start();
            evens.Join();
            Console.WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbers(int start, int end)
        {
            int firstEven = start % 2 == 0 ? start : ++start;
            for (int even = firstEven; even <= end; even += 2)
            {
                Console.WriteLine(even);
            }
        }
    }
}
