using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Grid
    {
        private List<int[]> data;
        
        private int offset;

        private int? height;

        private int? width;

        private TextReader reader;

        /// <summary>
        /// Load a grid from file reader. 
        /// </summary>
        /// <param name="reader">must be a valid CSV file, and number of rows and columns must be the same.</param>
        public Grid(TextReader reader)
        {
            this.reader = reader;
            data = new List<int[]>();
            ReadNextLine();
        }


        private bool ReadNextLine()
        {
            string line;
            if ((line = reader.ReadLine()) != null)
            {
                var cells = ParseLine(line);
                if (width != null && cells.Length != width)
                {
                    throw new Exception(string.Format("Invalid csv line length: row {0}", offset + data.Count));
                }

                width = cells.Length;
                
                for (var col = 0; col < cells.Length; col++)
                {
                    if (cells[col] > 256 || cells[col] < 0)
                    {
                        throw new Exception(string.Format("Invalid cell value in row {0} column {1}", offset + data.Count + 1, col + 1));
                    }
                }
                data.Add(cells);
                if (data.Count > Snake.MAX_LENGTH * 2 - 1)
                {
                    data.RemoveAt(0);
                    offset++;
                }
                return true;
            }

            height = offset + data.Count;
            return false;
        }

        /// <summary>
        /// Find out if x and y are both inside the grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if x and y are within the limits of the grid, false otherwise</returns>
        public bool isPointInside(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width)
                return false;
            while (y - offset >=  data.Count && ReadNextLine())
            {
            }

            return y - offset < data.Count;
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
            return data[y - offset][x];
        }

        /// <summary>
        /// Search for the first pair of similar (same weight) snakes 
        /// </summary>
        /// <returns>Tuple containing two similar snakes if found</returns>
        public Tuple<Snake, Snake> SearchSimilarPair()
        {
            var sums = new List<Snake>[1793];

            var y = 0;
            while (height == null || y < height)
            {
                for (var x = 0; x < width; x++)
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

                        var maxOverlappedSize = 0;
                        foreach (var oldSnake in sums[snake.Weight])
                        {
                            var overlapped = oldSnake.OverlappedSize(snake);
                            if (overlapped == 0)
                            {
                                return new Tuple<Snake, Snake>(oldSnake, snake);
                            } 
                            if(overlapped > maxOverlappedSize)
                            {
                                maxOverlappedSize = overlapped;
                            }
                                
                        }

                        if (maxOverlappedSize < Snake.MAX_LENGTH)
                        {
                            sums[snake.Weight].Add(snake);
                        }
                    }
                }
                y++;
            }

            return null;
        }
        
    }
}