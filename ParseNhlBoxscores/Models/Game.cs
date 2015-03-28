using System;

namespace ParseNhlBoxscores.Models
{
    public class Game
    {
        public Game () {}
            
        public int Id { get; set; }
        public string NhlGameId { get; set; }
        public DateTime Date { get; set; }
        public string VisitorTeam { get; set; }
        public int VisitorTeamId { get; set; }
        public Nullable<int> VisitorScore { get; set; }
        public string HomeTeam { get; set; }
        public int HomeTeamId { get; set; }
        public Nullable<int> HomeScore { get; set; }

        public string Print()
        {
            return "NhlGameId: " + NhlGameId + ", Date: " + Date.ToString("d") + ", " + VisitorTeam + " " + VisitorScore.ToString() +
                   " vs " + HomeTeam + " " + HomeScore.ToString();
        }
    }
}
