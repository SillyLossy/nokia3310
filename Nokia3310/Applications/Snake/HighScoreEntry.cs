namespace Nokia3310.Applications.Snake
{
    public class HighScoreEntry
    {
        public const int MaxNameLength = 10;

        public string Name { get; set; }
        public int Score { get; set; }

        internal bool Editable { get; set; }
    }
}
