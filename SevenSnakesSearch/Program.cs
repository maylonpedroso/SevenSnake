using System;
using System.IO;

namespace SevenSnakesSearch
{
    internal class Program
    {
        private const string USAGE = "Usage: SevenSnakesSearch.exe csv_path";
        
        public static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine(USAGE);
                return;
            }
            
            var filePath = args[0];
            if(!File.Exists(filePath)) {
                Console.WriteLine("Can not find the specified file");
                return;
            }
            
            var grid = new Grid(new StreamReader(filePath));
            
            var result = grid.SearchSimilarPair();

            if (result == null)
            {
                Console.WriteLine("FAIL");
            }
            else
            {
                Console.WriteLine("Snake 1: " + result.Item1.ToString());
                Console.WriteLine("Snake 2: " + result.Item2.ToString());
            }
        }
    }
}