using System;
using System.Collections.Generic;

namespace SevenSnakesSearch
{
    public static class Search
    {
        /// <summary>
        /// Search for the first pair of similar (same weight) snakes 
        /// </summary>
        /// <returns>Tuple containing two similar snakes if found</returns>
        public static Tuple<Snake, Snake> FindSimilarPair(Grid grid)
        {
            var sums = new List<Snake>[Grid.MaxCellValue * Snake.MaxLength + 1];

            var row = 0;
            while (grid.Height == null || row < grid.Height)
            {
                for (var col = 0; col < grid.Width; col++)
                {
                    // Find new snakes starting at current cell (col, row)
                    var snakes = new Snake(new Tuple<int, int>(col, row),
                                           new HashSet<Tuple<int, int>>(),
                                           grid.GetCell(col, row))
                                 .GrownList(grid);
              
                    foreach (var snake in snakes)
                    {
                        if (sums[snake.Weight] == null)
                        {
                            sums[snake.Weight] = new List<Snake>();
                        }

                        foreach (var oldSnake in sums[snake.Weight])
                        {
                            if (!oldSnake.IsOverlappedWith(snake))
                            {
                                return new Tuple<Snake, Snake>(oldSnake, snake);
                            } 
                        }

                        sums[snake.Weight].Add(snake);
                    }
                }
                row++;
            }

            return null;
        }
        
    }
}