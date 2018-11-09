using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nokia3310.Applications.Common;
using Nokia3310.Applications.Extensions;
using RLNET;

namespace Nokia3310.Applications.Snake
{
    public class SnakeGameState
    {
        private static readonly string[][] LevelPresets = {
            new[]
            {
                ""
            },
            new[]
            {
                "############################",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "#                          #",
                "############################",
            },
            new[]
            {
                "############################",
                "#                          #",
                "#                          #",
                "#                          #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#    ##################    #",
                "#                          #",
                "#                          #",
                "#                          #",
                "############################",
            },
            new[]
            {
                "############################",
                "#                          #",
                "#                          #",
                "#                          #",
                "#    #######    #######    #",
                "#    #                #    #",
                "#    #                #    #",
                "#           ####           #",
                "#           ####           #",
                "#    #                #    #",
                "#    #                #    #",
                "#    #######    #######    #",
                "#                          #",
                "#                          #",
                "#                          #",
                "############################",
            }, new []
            {
                "############    ############",
                "#                          #",
                "#   ########    ########   #",
                "#   #                  #   #",
                "#   #    ##########    #   #",
                "#   #    #        #    #   #",
                "    #    #        #    #   ",
                "    #        ##        #   ",
                "    #        ##        #   ",
                "    #    #        #    #   ",
                "#   #    #        #    #   #",
                "#   #    ##########    #   #",
                "#   #                  #   #",
                "#   ########    ########   #",
                "#                          #",
                "############    ############",
            },
        };

        private static List<Coordinate> GetLevel(int i)
        {
            var level = new List<Coordinate>();
            string[] preset = LevelPresets.HasIndex(i) ? LevelPresets[i] : LevelPresets.Random();

            for (int y = 0; y < preset.Length; y++)
            {
                for (int x = 0; x < preset[y].Length; x++)
                {
                    char character = preset[y][x];

                    if (char.IsWhiteSpace(character))
                    {
                        continue;
                    }

                    level.Add(new Coordinate(x, y));
                }
            }

            return level;
        }

        public int GetLevelCap(int level)
        {
            return (level + 1) * 20 + (level / 2) * 20 + (level / 5) * 50;
        }

        private static readonly int[] TickRates = { 200, 175, 150, 125, 100 };

        private const int InitialNodes = 3;

        private readonly LinkedList<Coordinate> nodes;
        private readonly List<Treat> treats;

        private List<Coordinate> obstacles;
        private Direction? direction;
        private DateTime lastUpdate;
        private readonly StringBuilder nameBuffer;

        public int Score { get; private set; }
        public int Level { get; private set; }

        public IEnumerable<Coordinate> Obstacles => obstacles;
        public IEnumerable<Treat> Treats => treats;
        public IEnumerable<Coordinate> Nodes => nodes;

        public StateManager StateManager { get; }

        private static SnakeGameSettings Settings => SettingsManager.Read<SnakeGameSettings>();

        public IEnumerable<HighScoreEntry> HighScore => Settings.HighScore;

        public SnakeGameState()
        {
            nodes = new LinkedList<Coordinate>();
            treats = new List<Treat>();
            lastUpdate = DateTime.Now;
            nameBuffer = new StringBuilder(HighScoreEntry.MaxNameLength, HighScoreEntry.MaxNameLength);
            StateManager = new StateManager(GameState.Title);
        }

        private void Spawn()
        {
            var firstSnakeNode = GetFreePoint();

            for (int i = 0; i < InitialNodes; i++)
            {
                if (firstSnakeNode != null)
                {
                    nodes.AddFirst(firstSnakeNode.Value);
                }
            }

            SpawnTreat();
        }

        public void Update(RLKeyPress keyPress)
        {
            if (keyPress?.Key == RLKey.Escape)
            {
                StateManager.ChangeState(GameState.Destroyed);
            }

            switch (StateManager.CurrentState)
            {
                case GameState.Title:
                    Update_Title(keyPress);
                    break;
                case GameState.Running:
                    Update_Running(keyPress);
                    break;
                case GameState.GameOver:
                    Update_GameOver(keyPress);
                    break;
                case GameState.HighScore:
                    Update_HighScore(keyPress);
                    break;
                case GameState.Splash:
                    Update_Splash(keyPress);
                    break;
            }
        }

        private void Update_Splash(RLKeyPress keyPress)
        {
            if (keyPress != null)
            {
                if (StateManager.PreviousState == GameState.GameOver)
                {
                    Settings.PutScore(Score);
                    StateManager.ChangeState(GameState.HighScore);
                }
                else
                {
                    StateManager.ChangeState(GameState.Running);
                }
            }
        }

        private void Update_HighScore(RLKeyPress keyPress)
        {
            var editable = Settings.HighScore.Find(h => h.Editable);

            if (editable != null)
            {
                if (keyPress?.Key == RLKey.Enter)
                {
                    editable.Editable = false;
                    SettingsManager.Write(Settings);
                }
                else if (keyPress?.Key == RLKey.BackSpace)
                {
                    if (nameBuffer.Length > 0)
                    {
                        nameBuffer.Length--;
                        editable.Name = nameBuffer.ToString();
                    }
                }
                else if (keyPress?.Char != null && nameBuffer.Length < HighScoreEntry.MaxNameLength)
                {
                    char character = char.ToUpperInvariant(keyPress.Char.Value);
                    nameBuffer.Append(character);
                    editable.Name = nameBuffer.ToString();
                }
            }
            else if (keyPress != null)
            {
                StateManager.ChangeState(GameState.Title);
            }
        }

        private void Update_Running(RLKeyPress keyPress)
        {
            SetDirection(keyPress);

            if (!direction.HasValue || (DateTime.Now - lastUpdate).TotalMilliseconds < TickRates.GetIndexOrLast(Level))
            {
                return;
            }

            var newHead = nodes.First.Value.Shifted(direction).Warp(0, NokiaApp.ScreenWidth, 0, NokiaApp.ScreenHeight);

            if (!HasSelfCollision(newHead) && !HasObstacleCollision(newHead))
            {
                nodes.AddFirst(newHead);
                nodes.RemoveLast();

                CheckForTreatsCollision();
                CheckForLevelUp();

                lastUpdate = DateTime.Now;
            }
            else
            {
                StateManager.ChangeState(GameState.GameOver);
                StateManager.ChangeState(GameState.Splash);
            }
        }

        private void Update_Title(RLKeyPress keyPress)
        {
            if (keyPress != null)
            {
                Initialize();
                StateManager.ChangeState(GameState.Splash);
            }
        }

        private void Update_GameOver(RLKeyPress keyPress)
        {
            if (keyPress != null)
            {
                StateManager.ChangeState(GameState.HighScore);
            }
        }

        private void SetDirection(RLKeyPress keyPress)
        {
            switch (keyPress?.Key)
            {
                case RLKey.Up:
                    direction = direction != Direction.Down ? Direction.Up : Direction.Down;
                    break;
                case RLKey.Down:
                    direction = direction != Direction.Up ? Direction.Down : Direction.Up;
                    break;
                case RLKey.Right:
                    direction = direction != Direction.Left ? Direction.Right : Direction.Left;
                    break;
                case RLKey.Left:
                    direction = direction != Direction.Right ? Direction.Left : Direction.Right;
                    break;
            }
        }

        private Coordinate? GetFreePoint()
        {
            Coordinate point;

            if (nodes.Count + obstacles.Count + treats.Count > NokiaApp.ScreenHeight * NokiaApp.ScreenWidth)
            {
                return null;
            }

            do
            {
                point = new Coordinate(NokiaApp.Random.Next(0, NokiaApp.ScreenWidth), NokiaApp.Random.Next(0, NokiaApp.ScreenHeight));
            } while (nodes.Contains(point) || treats.Any(t => t.Location.Equals(point)) || obstacles.Contains(point));

            return point;
        }

        private void CheckForLevelUp()
        {
            if (Score >= GetLevelCap(Level))
            {
                Initialize(Level + 1, Score);
                StateManager.ChangeState(GameState.Splash);
            }
        }

        private bool HasObstacleCollision(Coordinate newHead)
        {
            return obstacles.Contains(newHead);
        }

        private bool HasSelfCollision(Coordinate target)
        {
            bool first = true;

            foreach (var self in nodes)
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                if (target.X == self.X && target.Y == self.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckForTreatsCollision()
        {
            var head = nodes.First.Value;

            var eatenTreat = treats.Find(t => t.Location.Equals(head));

            if (eatenTreat == null)
            {
                return;
            }

            treats.Remove(eatenTreat);

            for (int i = 0; i < eatenTreat.ScoreIncrement; i++)
            {
                nodes.AddLast(nodes.Last.Value);
            }

            Score += eatenTreat.ScoreIncrement;
            SpawnTreat();
        }

        private void SpawnTreat()
        {
            var coordinate = GetFreePoint();

            if (coordinate == null)
            {
                return;
            }

            treats.Add(Treat.GetRandom(coordinate.Value));
        }

        private void Initialize(int level, int score)
        {
            nodes.Clear();
            treats.Clear();
            Score = score;
            Level = level;
            obstacles = GetLevel(level);
            StateManager.ChangeState(GameState.Running);
            direction = null;
            Spawn();
        }

        public void Initialize()
        {
            Initialize(0, 0);
        }
    }
}
