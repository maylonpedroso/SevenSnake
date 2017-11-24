using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Grid
    {
        private int[][] data;

        /// <summary>
        /// Load a grid from file reader. 
        /// </summary>
        /// <param name="reader">must be a valid CSV file, and number of rows and columns must be the same.</param>
        public Grid(StreamReader reader)
        {
            var index = 0;
            while (!reader.EndOfStream)
            {
                var line = ParseLine(reader.ReadLine());
                
                if (data == null)
                {
                    data =  new int[line.Length][];
                }
                data[index] = line;
                
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return data.Length;
        }

        /// <summary>
        /// Find out if x and y are both inside the grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if x and y are within the limits of the grid, false otherwise</returns>
        public bool isPointInside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < data.Length && y < data.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static int[] ParseLine(string line)
        {
            return (from number in line.Split(',') select int.Parse(number)).ToArray();
        }

        /// <summary>
        /// Will get the weight of the cell in (x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>int value in position x,y of the grid</returns>
        public int GetCell(int x, int y)
        {
            return data[y][x];
        }

        /// <summary>
        /// Search for the first pair of similar (same weight) snakes 
        /// </summary>
        /// <returns>Tuple containing two similar snakes if found</returns>
        public Tuple<Snake, Snake> SearchSimilarPair()
        {
            var sums = new List<Snake>[1785];

            for (var y = 0; y < data.Length; y++)
            {
                for (var x = 0; x < data.Length; x++)
                {
                    var snakes = new List<Snake>
                    {
                        new Snake(new Tuple<int, int>(x,y), new HashSet<Tuple<int, int>>(), GetCell(x, y))
                    };
                    for (var l = 1; l < Snake.MAX_LENGTH; l++)
                    {
                        var next = new List<Snake>();
                        foreach (var snake in snakes)
                        {
                            next.AddRange(snake.GrownList(x, y, this));
                        }
                        snakes.Clear();
                        snakes = next;
                    }

                    foreach (var snake in snakes)
                    {
                        if (sums[snake.Weight] == null)
                        {
                            sums[snake.Weight] = new List<Snake>();
                        }

                        foreach (var oldSnake in sums[snake.Weight])
                        {
                            if (oldSnake.NotOverlappedWith(snake))
                            {
                                return new Tuple<Snake, Snake>(oldSnake, snake);
                            }
                        }
                        
                        sums[snake.Weight].Add(snake);
                    }
                }
            }

            return null;
        }
        
    }
}