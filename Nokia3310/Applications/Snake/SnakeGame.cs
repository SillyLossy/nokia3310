using Nokia3310.Applications.Common;
using Nokia3310.Applications.Extensions;
using RLNET;

namespace Nokia3310.Applications.Snake
{
    public class SnakeGame : NokiaApp
    {
        private SnakeGameState gameState;
        private int blinkTick;
        private const int BlinkThreshold = FPS / 2;

        public override void Run()
        {
            base.Run();
            gameState = new SnakeGameState();
        }

        public SnakeGame(NokiaApp parent, RLRootConsole console) : base(parent)
        {
            Console = console;
        }

        public override void Update(object sender, UpdateEventArgs e)
        {
            gameState.Update(Console.Keyboard.GetKeyPress());

            if (gameState.StateManager.CurrentState == GameState.Destroyed)
            {
                Destroy();
            }
        }

        public override void Render(object sender, UpdateEventArgs e)
        {
            blinkTick = blinkTick + 1 < BlinkThreshold * 2 ? blinkTick + 1 : 0;
            Console.Clear(' ', BackgroundColor, BackgroundColor);

            switch (gameState.StateManager.CurrentState)
            {
                case GameState.Title:
                    Render_Title();
                    break;
                case GameState.Running:
                    Render_Running();
                    break;
                case GameState.GameOver:
                    Render_GameOver();
                    break;
                case GameState.HighScore:
                    Render_HighScore();
                    break;
                case GameState.Splash:
                    Render_Splash();
                    break;
            }

            Console.Draw();
        }

        private void Render_Splash()
        {
            string line1, line2;

            if (gameState.StateManager.PreviousState == GameState.GameOver)
            {
                Render_GameOver();
                line1 = "GAME OVER!";
                line2 = $"Score: {gameState.Score}";
            }
            else
            {
                line1 = $"Level {gameState.Level + 1}";
                line2 = $"Goal: {gameState.GetLevelCap(gameState.Level)} points";
            }

            const int boxWidth = 21, boxHeight = 11, textMargin = 3;
            const int boxX = ScreenWidth / 2 - boxWidth / 2 - 1, boxY = ScreenHeight / 2 - boxHeight / 2 - 1;
            Console.DrawBox(boxX, boxY, boxWidth, boxHeight, ForegroundColor, BackgroundColor);
            Console.Print(1 + boxX + (boxWidth - line1.Length) / 2, boxY + textMargin, line1, ForegroundColor);
            Console.Print(1 + boxX + (boxWidth - line2.Length) / 2, boxY + textMargin * 2, line2, ForegroundColor);
        }

        private void Render_HighScore()
        {
            Console.Print(0, 0, "           B E S T           ", BackgroundColor, ForegroundColor);

            const int marginX = 2;

            int offsetY = 2, i = 0;

            foreach (var item in gameState.HighScore)
            {
                if (item.Editable)
                {
                    bool appendCursor = DisplayBlink && item.Name.Length < HighScoreEntry.MaxNameLength;
                    Console.Print(marginX, offsetY, ($"{++i}. " + item.Name + (appendCursor ? "_" : "")).PadRight(20) + item.Score.ToString().PadLeft(4), BackgroundColor, ForegroundColor);
                }
                else
                {
                    Console.Print(marginX, offsetY, ($"{++i}. " + item.Name).PadRight(20) + item.Score.ToString().PadLeft(4), ForegroundColor, BackgroundColor);
                }
                offsetY += 2;
            }
        }

        private void Render_GameOver()
        {
            if (DisplayBlink)
            {
                foreach (var obstacle in gameState.Obstacles)
                {
                    Console.SetBackColor(obstacle.X, obstacle.Y, ForegroundColor);
                }

                foreach (var node in gameState.Nodes)
                {
                    Console.SetColor(node.X, node.Y, ForegroundColor);
                    Console.SetChar(node.X, node.Y, Glyph.Block);
                }

                foreach (var treat in gameState.Treats)
                {
                    Console.SetColor(treat.Location.X, treat.Location.Y, ForegroundColor);
                    Console.SetChar(treat.Location.X, treat.Location.Y, treat.Character);
                }
            }
        }

        private bool DisplayBlink => blinkTick < BlinkThreshold;

        private void Render_Title()
        {
            string[] title =
            {
                "                            ",
                " ÛÛÛÛ Û   Û  ÛÛÛ  Û  Û ÛÛÛÛ ",
                " Û    ÛÛ  Û Û   Û Û Û  Û    ",
                " ÛÛÛÛ Û Û Û ÛÛÛÛÛ ÛÛ   ÛÛÛÛ ",
                "    Û Û  ÛÛ Û   Û Û Û  Û    ",
                " ÛÛÛÛ Û   Û Û   Û Û  Û ÛÛÛÛ ",
                "                            ",
                "                            ",
                "   ÛÛÛÛÛÛÛÛÛÛÛ     ÛÛÛÛÛ  * ",
                "   Û         Û     Û        ",
                "   Û         Û     Û        ",
                "   Û         ÛÛÛÛÛÛÛ        ",
                "   Û                        ",
                "                            ",
                "   Press any key to begin   "
            };

            for (int i = 0; i < title.Length; i++)
            {
                Console.Print(0, i, title[i], ForegroundColor, BackgroundColor);
            }
        }

        private void Render_Running()
        {
            foreach (var obstacle in gameState.Obstacles)
            {
                Console.SetBackColor(obstacle.X, obstacle.Y, ForegroundColor);
            }

            foreach (var node in gameState.Nodes)
            {
                Console.SetColor(node.X, node.Y, ForegroundColor);
                Console.SetChar(node.X, node.Y, Glyph.Block);
            }

            foreach (var treat in gameState.Treats)
            {
                Console.SetColor(treat.Location.X, treat.Location.Y, ForegroundColor);
                Console.SetChar(treat.Location.X, treat.Location.Y, treat.Character);
            }
        }

        public override string Name => "Snake";
    }
}
