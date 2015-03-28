using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseNhlBoxscores.Models
{
    public class Team
    {
        public Team() { }

        public int Id { get; set; }
        public string LongName { get; set; }

        public string ShortName { get; set; }

        public string Abbreviation { get; set; }

    }
}
