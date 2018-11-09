namespace Nokia3310.Applications.Common
{
    public class Boundaries
    {
        public Boundaries(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }

        public bool Contains(Coordinate coordinate)
        {
            return coordinate.X >= MinX && coordinate.X < MaxX && coordinate.Y >= MinY && coordinate.Y < MaxY;
        }
    }
}
