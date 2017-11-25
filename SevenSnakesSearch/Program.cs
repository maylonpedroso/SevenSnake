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
                Console.Error.WriteLine(USAGE);
                return;
            }
            
            var filePath = args[0];
            if(!File.Exists(filePath)) {
                Console.Error.WriteLine("Can not find the specified file");
                return;
            }

            Grid grid;
            Tuple<Snake, Snake> result;
            try
            {
                grid = new Grid(new StreamReader(filePath));
                result = grid.SearchSimilarPair();
            }
            catch (FormatException e)
            {
                Console.Error.WriteLine("Invalid cell value found, not a number");
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }


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