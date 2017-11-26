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
            var sums = new List<Snake>[Grid.MAX_CELL_VALUE * Snake.MAX_LENGTH + 1];

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