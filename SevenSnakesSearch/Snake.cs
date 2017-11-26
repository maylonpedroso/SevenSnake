using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Snake
    {
        public const int MaxLength = 7;
        private readonly int[] MX = {0, 1, 0, -1};
        private readonly int[] MY = {1, 0, -1, 0};
        
        public int Weight { get; }

        private Tuple<int, int> Head { get; }

        private HashSet<Tuple<int,int>> Body { get; }

        public Snake(Tuple<int, int> head, IEnumerable<Tuple<int, int>> body, int weight)
        {
            Head = head;
            Weight = weight;
            Body = new HashSet<Tuple<int, int>>(body) {head};
        }

        /// <summary>
        /// Create list of snakes generated extending the size from head 
        /// one cell in every permitted direction.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>List of snakes</returns>
        private IEnumerable<Snake> GrownOneCell(Grid grid)
        {
            var snakes = new List<Snake>();
            for (var i = 0; i < 4; i++)
            {
                var head = new Tuple<int, int>(Head.Item1 + MX[i], Head.Item2 + MY[i]);
                if (grid.isValidCellPosition(head.Item1, head.Item2)
                    && !Body.Contains(head) 
                    && NotAdjacentToBody(head)
                    && CanEndInInitialRow(head))
                {
                    snakes.Add(new Snake(head, Body, Weight + grid.GetCell(head.Item1, head.Item2)));
                }
            }
            return snakes;
        }

        /// <summary>
        /// Create new snakes grown from the current to the maximum length 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>List of snakes</returns>
        public IEnumerable<Snake> GrownList(Grid grid)
        {
            var snakes = new List<Snake> { this };
            
            for (var l = Body.Count; l < MaxLength; l++)
            {
                var next = new List<Snake>();
                foreach (var snake in snakes)
                {
                    next.AddRange(snake.GrownOneCell(grid));
                }
                snakes.Clear();
                snakes = next;
            }

            return snakes;
        }

        /// <summary>
        /// Check if cell is not adjacent to the snake body
        /// </summary>
        /// <param name="cell">A tupple with cell position (col, row) to check</param>
        /// <returns>true if not adjascent, otherwise false</returns>
        private bool NotAdjacentToBody(Tuple<int, int> cell)
        {
            for (var i = 0; i < 4; i++)
            {
                var adjacent = new Tuple<int, int>(cell.Item1 + MX[i], cell.Item2 + MY[i]);
                if (!adjacent.Equals(Head) && Body.Contains(adjacent))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validate if 
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private bool CanEndInInitialRow(Tuple<int, int> head)
        {
            return MaxLength - Body.Count - 1 - (head.Item1 > Body.First().Item1 ? 0 : 1) >=
                   Body.First().Item2 - head.Item2;
        }

        /// <summary>
        /// Count the number of cell shared by the snakes
        /// </summary>
        /// <param name="snake"></param>
        /// <returns>int number of shared cells</returns>
        public int OverlappedSize(Snake snake)
        {
            return snake.Body.Count(cell => Body.Contains(cell));
        }

        /// <summary>
        /// Creates a string representation of the snake
        /// </summary>
        /// <returns>Formatted snaked cells</returns>
        public override string ToString()
        {
            return $"[{string.Join(",", from cell in Body select $"({cell.Item2 + 1},{cell.Item1 + 1})")}]";
        }
    }
}