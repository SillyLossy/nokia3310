using Nokia3310.Applications.Common;

namespace Nokia3310.Applications.Tanks
{
    public class Obstacle
    {
        public static Obstacle Heavy(Coordinate location)
        {
            return new Obstacle
            {
                Health = 3,
                Location = location
            };
        }

        public static Obstacle Medium(Coordinate location)
        {
            return new Obstacle
            {
                Health = 2,
                Location = location
            };

        }

        public static Obstacle Light(Coordinate location)
        {
            return new Obstacle
            {
                Health = 1,
                Location = location
            };
        }
        public static Obstacle Indestructible(Coordinate location)
        {
            return new Obstacle
            {
                Health = -1,
                Location = location
            };
        }

        public Coordinate Location { get; private set; }

        public int Glyph
        {
            get
            {
                switch (Health)
                {
                    case 3:
                        return Common.Glyph.HeavyShade;
                    case 2:
                        return Common.Glyph.MediumShade;
                    case 1:
                        return Common.Glyph.LightShade;
                    default:
                        return Common.Glyph.Block;
                }
            }
        }

        public int Health { get; private set; }

        public void Hit()
        {
            --Health;
        }

        public bool Destroyed => Health == 0;
    }
}