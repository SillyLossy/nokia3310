using System;
using Nokia3310.Applications.Common;

namespace Nokia3310.Applications.Snake
{
    public class Treat : IEquatable<Treat>
    {
        public int Character { get; }
        public int ScoreIncrement { get; }

        public Coordinate Location { get; set; }

        private Treat(int character, int scoreIncrement)
        {
            Character = character;
            ScoreIncrement = scoreIncrement;
        }

        private static Treat Bug => new Treat('*', 1);
        private static Treat Bagel => new Treat('@', 3);
        private static Treat Guy => new Treat(Glyph.BlackFace, 5);

        public static Treat GetRandom(Coordinate location)
        {
            Treat treat;
            int value = NokiaApp.Random.Next(20);

            if (value == 0)
            {
                treat = Guy;
            }
            else if (value < 5)
            {
                treat = Bagel;
            }
            else
            {
                treat = Bug;
            }

            treat.Location = location;
            return treat;
        }

        public bool Equals(Treat other)
        {
            return Character == other?.Character;
        }
    }
}