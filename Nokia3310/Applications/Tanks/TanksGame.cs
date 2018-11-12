using Nokia3310.Applications.Common;
using Nokia3310.Applications.Games;
using RLNET;

namespace Nokia3310.Applications.Tanks
{
    public class TanksGame : AbstractGame<TanksGameState>
    {
        public TanksGame(NokiaApp parent, RLRootConsole console) : base(parent, console)
        {
        }
        
        public override void Render(object sender, UpdateEventArgs e)
        {
            Console.Clear(' ', BackgroundColor, BackgroundColor);

            switch (State.StateManager.CurrentState)
            {
                case GameState.Running:
                    Render_Running();
                    break;
            }

            Console.Draw();
        }

        private void Render_Running()
        {
            foreach (var obstacle in State.Obstacles)
            {
                Console.SetColor(obstacle.Location.X, obstacle.Location.Y, ForegroundColor);
                Console.SetChar(obstacle.Location.X, obstacle.Location.Y, obstacle.Glyph);
            }

            foreach (var projectile in State.Projectiles)
            {
                Console.SetColor(projectile.Location.X, projectile.Location.Y, ForegroundColor);
                Console.SetChar(projectile.Location.X, projectile.Location.Y, Glyph.Circle);
            }

            foreach (var tank in State.Tanks)
            {
                Console.SetColor(tank.Location.X, tank.Location.Y, ForegroundColor);
                Console.SetChar(tank.Location.X, tank.Location.Y, tank.GetGlyph());
            }
        }

        public override string Name => "Tanks";
    }
}
