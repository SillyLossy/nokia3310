using System.Collections.Generic;
using System.Linq;

namespace Nokia3310.Applications.Snake
{
    public class SnakeGameSettings
    {
        private const int MaxScores = 7;

        public List<HighScoreEntry> HighScore { get; set; }

        public SnakeGameSettings()
        {
            HighScore = new List<HighScoreEntry>();
        }

        public void PutScore(int score)
        {
            if (HighScore.Count < MaxScores || score <= HighScore.Select(x => x.Score).Min())
            {
                HighScore.Add(new HighScoreEntry { Editable = true, Name = "", Score = score });
                HighScore = HighScore.OrderByDescending(x => x.Score).Take(MaxScores).ToList();
            }
        }
    }
}
