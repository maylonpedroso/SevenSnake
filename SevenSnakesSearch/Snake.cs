using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenSnakesSearch
{
    public class Snake
    {
        public const int MAX_LENGTH = 7; 
        private int[] MX = {0, 1, 0, -1};
        private int[] MY = {1, 0, -1, 0};
        
        public int Weight { get; }
        
        public Tuple<int, int> Head { get; }

        private HashSet<Tuple<int,int>> Body { get; }

        public Snake(Tuple<int, int> head, HashSet<Tuple<int,int>> body, int weight)
        {
            Head = head;
            Weight = weight;
            Body = new HashSet<Tuple<int, int>>(body) {head};
        }

        
        public IEnumerable<Snake> GrownList(int pX, int pY, Grid grid)
        {
            var snakes = new List<Snake>();
            for (var i = 0; i < 4; i++)
            {
                var head = new Tuple<int, int>(Head.Item1 + MX[i], Head.Item2 + MY[i]);
                if (grid.isPointInside(head.Item1, head.Item2)
                    && !Body.Contains(head) 
                    && MAX_LENGTH - Body.Count - (head.Item1 > pX ? 2:1) > pY - head.Item2)
                {
                    snakes.Add(new Snake(head, Body, Weight + grid.GetCell(head.Item1, head.Item2)));
                }
            }
            return snakes;
        }

        public bool NotOverlappedWith(Snake snake)
        {
            return snake.Body.All(cell => !Body.Contains(cell));
        }

        public string ToString()
        {
            return string.Join(" ", from cell in Body select string.Format("({0},{1})", cell.Item1+1, cell.Item2+1));
        }
    }
}