using System;
using System.Collections.Generic;
using System.Linq;
using Nokia3310.Applications.Common;
using Nokia3310.Applications.Extensions;

namespace Nokia3310.Applications.Tanks
{
    public class Tank
    {
        public static Tank Player(Coordinate location)
        {
            return new Tank
            {
                Location = location,
                LastMovement = DateTime.Now,
                LastShot = DateTime.Now,
                ShootCooldown = 1000,
                MovementSpeed = 200,
                Facing = EnumHelper.Random<Direction>()
            };
        }

        private double ShootCooldown { get; set; }
        private DateTime LastShot { get; set; }
        private double MovementSpeed { get; set; }
        private DateTime LastMovement { get; set; }
        public Coordinate Location { get; set; }
        public Direction Facing { get; set; }

        public int GetGlyph()
        {
            switch (Facing)
            {
                case Direction.Up:
                    return Glyph.TriangleUp;
                case Direction.Down:
                    return Glyph.TriangleDown;
                case Direction.Left:
                    return Glyph.TriangleLeft;
                case Direction.Right:
                    return Glyph.TriangleRight;
            }

            return 0;
        }

        public void MoveOrTurn(Direction direction, IEnumerable<Coordinate> impassable, Boundaries boundaries)
        {
            if ((DateTime.Now - LastMovement).TotalMilliseconds > MovementSpeed)
            {
                if (Facing == direction)
                {
                    var location = Location.Shifted(direction);

                    if (!impassable.Any(l => l.Equals(location)) && boundaries.Contains(location))
                    {
                        Location = location;
                    }
                }
                else
                {
                    Facing = direction;
                }
                LastMovement = DateTime.Now;
            }
        }

        public Projectile Shoot(Tank playerTank)
        {
            if ((DateTime.Now - LastShot).TotalMilliseconds > ShootCooldown)
            {
                var projectile = new Projectile { Direction = Facing, Location = Location, Owner = this, MovementSpeed = 50 };
                LastShot = DateTime.Now;
                return projectile;
            }

            return null;
        }
    }
}