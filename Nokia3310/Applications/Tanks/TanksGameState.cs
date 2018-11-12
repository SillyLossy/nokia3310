using System.Collections.Generic;
using System.Linq;
using Nokia3310.Applications.Common;
using Nokia3310.Applications.Extensions;
using Nokia3310.Applications.Games;
using OpenTK.Input;
using RLNET;

namespace Nokia3310.Applications.Tanks
{
    public class TanksGameState : AbstractGameState
    {
        private readonly List<Tank> tanks;
        private readonly List<Obstacle> obstacles;
        private readonly List<Projectile> projectiles;
        private readonly Boundaries boundaries;

        private Tank playerTank;

        public IEnumerable<Tank> Tanks => tanks;
        public IEnumerable<Obstacle> Obstacles => obstacles;
        public IEnumerable<Projectile> Projectiles => projectiles;

        public TanksGameState() : base(GameState.Running)
        {
            tanks = new List<Tank>();
            obstacles = new List<Obstacle>();
            projectiles = new List<Projectile>();
            boundaries = new Boundaries(0, NokiaApp.ScreenWidth, 0, NokiaApp.ScreenHeight);
            Initialize();
        }

        public override void Update(KeyboardState keyboard, RLKeyPress keyPress)
        {
            if (keyPress?.Key == RLKey.Escape)
            {
                StateManager.ChangeState(GameState.Destroyed);
            }

            switch (StateManager.CurrentState)
            {
                case GameState.Running:
                    Update_Running(keyboard);
                    break;
            }
        }

        private void Update_Running(KeyboardState key)
        {
            HandleInput(key);
            ProjectilesTick();
        }

        private void HandleInput(KeyboardState key)
        {
            if (key[Key.Space])
            {
                projectiles.AddNotNull(playerTank.Shoot(playerTank));
            }
            if (key[Key.Up])
            {
                playerTank.MoveOrTurn(Direction.Up, GetImpassable(), boundaries);
            }
            else if (key[Key.Down])
            {
                playerTank.MoveOrTurn(Direction.Down, GetImpassable(), boundaries);
            }
            else if (key[Key.Left])
            {
                playerTank.MoveOrTurn(Direction.Left, GetImpassable(), boundaries);
            }
            else if (key[Key.Right])
            {
                playerTank.MoveOrTurn(Direction.Right, GetImpassable(), boundaries);
            }
        }

        private void ProjectilesTick()
        {
            var toRemove = new List<Projectile>();

            foreach (var projectile in projectiles)
            {
                projectile.Tick();

                var obstacle = obstacles.Find(o => o.Location.Equals(projectile.Location));

                if (obstacle != null)
                {
                    toRemove.Add(projectile);
                    obstacle.Hit();
                    if (obstacle.Destroyed)
                    {
                        obstacles.Remove(obstacle);
                    }
                }

                var tank = tanks.Find(t => t.Location.Equals(projectile.Location));

                if (tank != null && !ReferenceEquals(tank, projectile.Owner))
                {
                    toRemove.Add(projectile);
                    tank.Hit();
                    if (tank.Destroyed)
                    {
                        tanks.Remove(tank);
                    }
                }

                if (!boundaries.Contains(projectile.Location))
                {
                    toRemove.Add(projectile);
                }
            }

            projectiles.RemoveAll(i => toRemove.Contains(i));
        }

        private void SpawnPlayer()
        {
            playerTank = Tank.Player(new Coordinate(9, 10));
            tanks.Add(playerTank);
            tanks.Add(Tank.Player(new Coordinate(0, 0)));
            obstacles.Add(Obstacle.Heavy(new Coordinate(11, 11)));
            obstacles.Add(Obstacle.Light(new Coordinate(11, 10)));
            obstacles.Add(Obstacle.Medium(new Coordinate(10, 11)));
            obstacles.Add(Obstacle.Indestructible(new Coordinate(10, 10)));
        }

        private IEnumerable<Coordinate> GetImpassable()
        {
            return obstacles.Select(o => o.Location).Concat(tanks.Select(t => t.Location));
        }

        public void Initialize()
        {
            tanks.Clear();
            obstacles.Clear();
            projectiles.Clear();
            SpawnPlayer();
        }
    }
}
