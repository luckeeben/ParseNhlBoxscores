using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseNhlBoxscores
{
    class PlayerGameLog
    {
        public string NhlGameId { get; set; }
        public string Team { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int PlusMinus { get; set; }
        public int Penalty { get; set; }
        public int PenaltyMinutes { get; set; }
        public int TotalToi { get; set; }
        public int PpToi { get; set; }
        public int ShToi { get; set; }
        public int EvToi { get; set; }
        public int Shots { get; set; }
        public int AttemptsBlocked { get; set; }
        public int MissedShots { get; set; }
        public int Hits { get; set; }
        public int Giveaways { get; set; }
        public int Takeaways { get; set; }
        public int BlockedShots { get; set; }
        public int FaceoffsWon { get; set; }
        public int FaceoffsLost { get; set; }

        public void Print()
        {
            Console.WriteLine(NhlGameId);
            Console.WriteLine(PlayerNumber);
            Console.WriteLine(PlayerName);
            Console.WriteLine(Goals);

            Console.ReadLine();
        }
    }
}
