using System;

namespace ParseNhlBoxscores.Models
{
    public class PlayerGameLog
    {
        public PlayerGameLog() { }

        public int Id { get; set; }
        public int GameId { get; set; }
        public string Team { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int PlusMinus { get; set; }
        public int Penalty { get; set; }
        public int PenaltyMinutes { get; set; }
        public TimeSpan TotalToi { get; set; }
        public TimeSpan PpToi { get; set; }
        public TimeSpan ShToi { get; set; }
        public TimeSpan EvToi { get; set; }
        public int Shots { get; set; }
        public int AttemptsBlocked { get; set; }
        public int MissedShots { get; set; }
        public int Hits { get; set; }
        public int Giveaways { get; set; }
        public int Takeaways { get; set; }
        public int BlockedShots { get; set; }
        public int FaceoffsWon { get; set; }
        public int FaceoffsLost { get; set; }

        public string Print()
        {
            return "Game #" + GameId + ", " + PlayerName;
        }
    }
}
