using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task task = Task.Run((() =>
            {
                while ( true)
                {
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }));
            task.Wait();
        }
    }
}