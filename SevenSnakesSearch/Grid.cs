using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Grid
    {
        public const int MaxCellValue = 255;
        
        private List<int[]> data;
        
        private int offset;

        private readonly TextReader reader;

        public int? Height { get; private set; }

        public int? Width { get; private set; }

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
                if (Width != null && cells.Length != Width)
                {
                    throw new Exception($"Invalid csv line length: row {offset + data.Count}");
                }

                Width = cells.Length;
                
                for (var col = 0; col < cells.Length; col++)
                {
                    if (cells[col] > MaxCellValue || cells[col] < 0)
                    {
                        throw new Exception($"Invalid cell value in row {offset + data.Count + 1} column {col + 1}");
                    }
                }
                data.Add(cells);
                if (data.Count > 3 * Snake.MaxLength / 2 - 1)
                {
                    data.RemoveAt(0);
                    offset++;
                }
                return true;
            }

            Height = offset + data.Count;
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
            if (col < 0 || row < 0 || col >= Width)
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
            return (from number in line.Split(',',';') select int.Parse(number) - 1).ToArray();
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
        
    }
}