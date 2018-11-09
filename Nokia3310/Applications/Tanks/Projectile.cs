using System;
using Nokia3310.Applications.Common;

namespace Nokia3310.Applications.Tanks
{
    public class Projectile
    {
        public double MovementSpeed { get; set; }
        public DateTime LastMovement { get; set; } = DateTime.Now;
        public Tank Owner { get; set; }
        public Direction Direction { get; set; }
        public Coordinate Location { get; set; }

        public void Tick()
        {
            if ((DateTime.Now - LastMovement).TotalMilliseconds > MovementSpeed)
            {
                Location = Location.Shifted(Direction);
                LastMovement = DateTime.Now;
            }
        }
    }
}
