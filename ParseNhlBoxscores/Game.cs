using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseNhlBoxscores
{
    class Game
    {
        public string NhlGameId { get; set; }
        public DateTime Date { get; set; }
        public string VisitorTeam { get; set; }
        public int VisitorScore { get; set; }
        public string HomeTeam { get; set; }
        public int HomeScore { get; set; }

        public string PrintGame()
        {
            return "NhlGameId: " + NhlGameId + ", Date: " + Date + ", " + VisitorTeam + " " + VisitorScore.ToString() +
                   " vs " + HomeTeam + " " + HomeScore.ToString();
        }
    }
}
