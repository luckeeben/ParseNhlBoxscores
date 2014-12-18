using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseNhlBoxscores
{
    class PlayerGameLog
    {
        private int NhlGameId { get; set; }
        private int PlayerNumber { get; set; }
        private string PlayerName { get; set; }
        private int Goals { get; set; }
        private int Assists { get; set; }
        private int Points { get; set; }
        private int PlusMinus { get; set; }
        private int Penalty { get; set; }
        private int PenaltyMinutes { get; set; }
        private int TotalToi { get; set; }
        private int PpToi { get; set; }
        private int ShToi { get; set; }
        private int EvToi { get; set; }
        private int Shots { get; set; }
        private int AttemptsBlocked { get; set; }
        private int MissedShots { get; set; }
        private int Hits { get; set; }
        private int Giveaways { get; set; }
        private int Takeaways { get; set; }
        private int BlockedShots { get; set; }
        private int FaceoffsWon { get; set; }
        private int FaceoffsLost { get; set; }
    }
}
