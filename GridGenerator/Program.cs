using System;
using System.Linq;

namespace GridGenerator
{
    internal class Program
    {
        private const string USAGE = "GridGenerator.exe size";

        /// <summary>
        /// Generate a grid (comma-separated) with the specified size and random values 
        /// </summary>
        /// <param name="args">single element array containing size of grid</param>
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine(USAGE);
                return;
            }

            int size;
            if (!int.TryParse(args[0], out size))
            {
                Console.Error.WriteLine("Invalid size number");
                return;
            }

            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(",", Enumerable.Range(0, size).Select(r => random.Next(256)+1).ToArray()));                
            }
        }
    }
}