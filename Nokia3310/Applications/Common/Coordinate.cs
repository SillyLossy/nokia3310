using System;

namespace Nokia3310.Applications.Common
{
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate Shifted(Direction? direction)
        {
            int x = X, y = Y;

            switch (direction)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Down:
                    y++;
                    break;
                case Direction.Left:
                    x--;
                    break;
                case Direction.Right:
                    x++;
                    break;
            }

            return new Coordinate(x, y);
        }

        public Coordinate Warp(int minX, int maxX, int minY, int maxY)
        {
            int x = X, y = Y;

            if (x < minX)
            {
                x = maxX;
            }
            else if (x > maxX)

            {
                x = minX;
            }
            else if (y < minY)
            {
                y = maxY;
            }
            else if (y > maxY)
            {
                y = minY;
            }

            return new Coordinate(x, y);
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (X * 397) ^ Y;
        }
    }
}