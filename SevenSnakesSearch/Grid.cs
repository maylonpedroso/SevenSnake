using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Grid
    {
        private const int MAX_CELL_VALUE = 255;
        
        private List<int[]> data;
        
        private int offset;

        private int? height;

        private int? width;

        private readonly TextReader reader;

        /// <summary>
        /// Load a grid from file reader. 
        /// </summary>
        /// <param name="reader">must be a valid CSV file and cells value should be in the valid range.</param>
        public Grid(TextReader reader)
        {
            this.reader = reader;
            data = new List<int[]>();
            ReadNextLine();
        }

        /// <summary>
        /// Reads and validate next line from the grid csv file
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool ReadNextLine()
        {
            string line;
            if ((line = reader.ReadLine()) != null)
            {
                var cells = ParseLine(line);
                if (width != null && cells.Length != width)
                {
                    throw new Exception($"Invalid csv line length: row {offset + data.Count}");
                }

                width = cells.Length;
                
                for (var col = 0; col < cells.Length; col++)
                {
                    if (cells[col] > MAX_CELL_VALUE || cells[col] < 0)
                    {
                        throw new Exception($"Invalid cell value in row {offset + data.Count + 1} column {col + 1}");
                    }
                }
                data.Add(cells);
                if (data.Count > 3 * Snake.MAX_LENGTH / 2 - 1)
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
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>true if x and y are within the limits of the grid, false otherwise</returns>
        public bool isValidCellPosition(int col, int row)
        {
            if (col < 0 || row < 0 || col >= width)
                return false;
            while (row - offset >=  data.Count && ReadNextLine())
            {
            }

            return row - offset < data.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static int[] ParseLine(string line)
        {
            return (from number in line.Split(',') select int.Parse(number) - 1).ToArray();
        }

        /// <summary>
        /// Will get the weight of the cell in (x,y)
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>int value in position x,y of the grid</returns>
        public int GetCell(int col, int row)
        {
            return data[row - offset][col];
        }

        /// <summary>
        /// Search for the first pair of similar (same weight) snakes 
        /// </summary>
        /// <returns>Tuple containing two similar snakes if found</returns>
        public Tuple<Snake, Snake> SearchSimilarPair()
        {
            var sums = new List<Snake>[MAX_CELL_VALUE * Snake.MAX_LENGTH + 1];

            var row = 0;
            while (height == null || row < height)
            {
                for (var col = 0; col < width; col++)
                {
                    // Find new snakes starting at current cell (col, row)
                    var snakes = new Snake(new Tuple<int, int>(col, row),
                                           new HashSet<Tuple<int, int>>(),
                                           GetCell(col, row))
                                 .GrownList(this);
              
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
                row++;
            }

            return null;
        }
        
    }
}