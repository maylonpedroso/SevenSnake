using System;
using System.IO;
using System.Linq;

namespace GridGenerator
{
    internal class Program
    {
        private const string Usage = "GridGenerator.exe size path/to/file.csv";

        /// <summary>
        /// Generate a grid (comma-separated) with the specified size and random values 
        /// </summary>
        /// <param name="args">two elements array containing size of grid and output file path</param>
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine(Usage);
                return;
            }

            int size;
            if (!int.TryParse(args[0], out size))
            {
                Console.Error.WriteLine("Invalid size number");
                return;
            }

            var file = new StreamWriter(args[1]);

            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                file.WriteLine(string.Join(",", Enumerable.Range(0, size).Select(r => random.Next(256)+1).ToArray()));                
            }
            file.Close();
        }
    }
}