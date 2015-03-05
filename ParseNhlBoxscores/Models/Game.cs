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
        public Nullable<int> VisitorScore { get; set; }
        public string HomeTeam { get; set; }
        public Nullable<int> HomeScore { get; set; }

        public string PrintGame()
        {
            return "NhlGameId: " + NhlGameId + ", Date: " + Date + ", " + VisitorTeam + " " + VisitorScore.ToString() +
                   " vs " + HomeTeam + " " + HomeScore.ToString();
        }
    }
}
