using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam
{
    public class SnakeBody
    {
        private int direction;
        private Point location;
        
        public int Direction { get => direction; set => direction = value; }
        public Point Location { get => location; }
        public void setX(int x)
        {
            location.X = x;
        }
        public void setY(int y)
        {
            location.Y = y;
        }
        public int getX()
        {
            return location.X;
        }
        public int getY()
        {
            return location.Y;
        }
        public void draw(Graphics g, SolidBrush brush, Color color)
        {
            brush.Color = color;
            g.FillEllipse(brush, Location.X, Location.Y, 17, 17);
        }
    }
}
