using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseNhlBoxscores.Models
{
    public class Player
    {
        public Player() { }

        public int Id { get; set; }
        public int NhlId { get; set; }
        public string Name { get; set; }
        public string EsName { get; set; }
        public DateTime Birthdate { get; set; }
        public int TeamId { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
        public int PlusMinus { get; set; }
        public int Pim { get; set; }
    }
}
