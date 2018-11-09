using Nokia3310.Applications.Common;
using OpenTK.Input;
using RLNET;

namespace Nokia3310.Applications.Tanks
{
    public class TanksGame : NokiaApp
    {
        private TanksGameState state;

        public override void Run()
        {
            base.Run();
            state = new TanksGameState();
        }

        public TanksGame(NokiaApp parent, RLRootConsole console) : base(parent)
        {
            Console = console;
        }

        public override void Update(object sender, UpdateEventArgs e)
        {
            state.Update(Keyboard.GetState(), Console.Keyboard.GetKeyPress());

            if (state.StateManager.CurrentState == GameState.Destroyed)
            {
                Destroy();
            }
        }

        public override void Render(object sender, UpdateEventArgs e)
        {
            Console.Clear(' ', BackgroundColor, BackgroundColor);

            switch (state.StateManager.CurrentState)
            {
                case GameState.Running:
                    Render_Running();
                    break;
            }

            Console.Draw();
        }

        private void Render_Running()
        {
            foreach (var obstacle in state.Obstacles)
            {
                Console.SetColor(obstacle.Location.X, obstacle.Location.Y, ForegroundColor);
                Console.SetChar(obstacle.Location.X, obstacle.Location.Y, obstacle.Glyph);
            }

            foreach (var projectile in state.Projectiles)
            {
                Console.SetColor(projectile.Location.X, projectile.Location.Y, ForegroundColor);
                Console.SetChar(projectile.Location.X, projectile.Location.Y, Glyph.Circle);
            }

            foreach (var tank in state.Tanks)
            {
                Console.SetColor(tank.Location.X, tank.Location.Y, ForegroundColor);
                Console.SetChar(tank.Location.X, tank.Location.Y, tank.GetGlyph());
            }
        }

        public override string Name => "Tanks";
    }
}
