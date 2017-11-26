using System;
using System.IO;

namespace SevenSnakesSearch
{
    internal class Program
    {
        private const string Usage = "Usage: SevenSnakesSearch.exe csv_path";
        
        public static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.Error.WriteLine(Usage);
                return;
            }
            
            var filePath = args[0];
            if(!File.Exists(filePath)) {
                Console.Error.WriteLine("Can not find the specified file");
                return;
            }

            Tuple<Snake, Snake> result;
            try
            {
                var reader = new StreamReader(filePath);
                result = Search.FindSimilarPair(new Grid(reader));
                reader.Close();
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
                Console.WriteLine(result.Item1);
                Console.WriteLine(result.Item2);
            }
        }
    }
}