using System;

namespace SeaBattle.Common.Types
{
    public class Point: IEquatable<Point>
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            return
                other.X == X &&
                other.Y == Y;
        }
    }
}
