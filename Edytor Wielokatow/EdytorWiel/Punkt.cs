using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdytorWiel
{
    public enum ContingencyType { G0, G1, C1 }
    public class Punkt
    {
        public Point Position { get; set; }
        private int defaultRadius = 5;
        private int highlightRadius = 8;
        private int toleranceRadius = 10;
        public bool IsHighlighted { get; set; }
        public bool isMoving{ get; set; }
        public bool isControl{ get; set; }
        public ContingencyType contingency { get; set; }

        public Punkt(int x, int y)
        {
            Position = new Point(x, y);
            IsHighlighted = false;
            contingency = ContingencyType.C1;
            isControl = false;
        }

        public void Draw(Graphics g)
        {
            int currentRadius = IsHighlighted ? highlightRadius : defaultRadius;
            Brush brush = IsHighlighted ? Brushes.Blue : Brushes.Black;
            g.FillEllipse(brush, Position.X - currentRadius, Position.Y - currentRadius, 2 * currentRadius, 2 * currentRadius);
        }

        public bool IsMouseOver(Point mousePosition)
        {
            return Math.Sqrt(Math.Pow(mousePosition.X - Position.X, 2) + Math.Pow(mousePosition.Y - Position.Y, 2)) <= toleranceRadius;
        }
    }


}
