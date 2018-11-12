using Newtonsoft.Json;
using Nokia3310.Applications.Common;
using Nokia3310.Applications.Extensions;
using Nokia3310.Applications.Games;
using RLNET;

namespace Nokia3310.Applications.Snake
{
    public class SnakeGame : AbstractGame<SnakeGameState>
    {
        private int blinkTick;
        private const int BlinkThreshold = FPS / 2;

        public SnakeGame(NokiaApp parent, RLRootConsole console) : base(parent, console)
        {
        }

        public override void Render(object sender, UpdateEventArgs e)
        {
            blinkTick = blinkTick + 1 < BlinkThreshold * 2 ? blinkTick + 1 : 0;
            Console.Clear(' ', BackgroundColor, BackgroundColor);

            switch (State.StateManager.CurrentState)
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

            if (State.StateManager.PreviousState == GameState.GameOver)
            {
                Render_GameOver();
                line1 = "GAME OVER!";
                line2 = $"Score: {State.Score}";
            }
            else
            {
                line1 = $"Level {State.Level + 1}";
                line2 = $"Goal: {State.GetLevelCap(State.Level)} points";
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

            foreach (var item in State.HighScore)
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
                foreach (var obstacle in State.Obstacles)
                {
                    Console.SetBackColor(obstacle.X, obstacle.Y, ForegroundColor);
                }

                foreach (var node in State.Nodes)
                {
                    Console.SetColor(node.X, node.Y, ForegroundColor);
                    Console.SetChar(node.X, node.Y, Glyph.Block);
                }

                foreach (var treat in State.Treats)
                {
                    Console.SetColor(treat.Location.X, treat.Location.Y, ForegroundColor);
                    Console.SetChar(treat.Location.X, treat.Location.Y, treat.Character);
                }
            }
        }

        private bool DisplayBlink => blinkTick < BlinkThreshold;

        private void Render_Title()
        {
            string[] title = JsonConvert.DeserializeObject<string[]>(Resources.Levels.SnakeTitle);

            for (int i = 0; i < title.Length; i++)
            {
                Console.Print(0, i, title[i], ForegroundColor, BackgroundColor);
            }
        }

        private void Render_Running()
        {
            foreach (var obstacle in State.Obstacles)
            {
                Console.SetBackColor(obstacle.X, obstacle.Y, ForegroundColor);
            }

            foreach (var node in State.Nodes)
            {
                Console.SetColor(node.X, node.Y, ForegroundColor);
                Console.SetChar(node.X, node.Y, Glyph.Block);
            }

            foreach (var treat in State.Treats)
            {
                Console.SetColor(treat.Location.X, treat.Location.Y, ForegroundColor);
                Console.SetChar(treat.Location.X, treat.Location.Y, treat.Character);
            }
        }

        public override string Name => "Snake";
    }
}
