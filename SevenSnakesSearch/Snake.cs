using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenSnakesSearch
{
    public class Snake
    {
        public const int MAX_LENGTH = 7; 
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
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="grid"></param>
        /// <returns>List of snakes</returns>
        public IEnumerable<Snake> GrownList(int pX, int pY, Grid grid)
        {
            var snakes = new List<Snake>();
            for (var i = 0; i < 4; i++)
            {
                var head = new Tuple<int, int>(Head.Item1 + MX[i], Head.Item2 + MY[i]);
                if (grid.isPointInside(head.Item1, head.Item2)
                    && !Body.Contains(head) 
                    && MAX_LENGTH - Body.Count - 1 - (head.Item1 > pX ? 0:1) >= pY - head.Item2
                    )
                {
                    snakes.Add(new Snake(head, Body, Weight + grid.GetCell(head.Item1, head.Item2)));
                }
            }
            return snakes;
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

        public string ToString()
        {
            return string.Join(" ", from cell in Body select string.Format("({0},{1})", cell.Item1+1, cell.Item2+1));
        }
    }
}