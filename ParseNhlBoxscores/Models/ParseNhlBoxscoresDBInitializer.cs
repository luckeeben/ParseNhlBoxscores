using System.Collections.Generic;
using System.Data.Entity;

namespace ParseNhlBoxscores.Models
{
    public class ParseNhlBoxscoresDBInitializer : DropCreateDatabaseAlways<ParseNhlBoxscoresContext>
    {
        protected override void Seed(ParseNhlBoxscoresContext context)
        {
            IList<Team> teams = new List<Team>();

            // ###### EASTERN CONFERENCE ######

            // ATLANTIC DIVISION
            teams.Add(new Team() { LongName = "BOSTON BRUINS", ShortName = "BOSTON", Abbreviation = "BOS" });
            teams.Add(new Team() { LongName = "BUFFALO SABRES", ShortName = "BUFFALO", Abbreviation = "BUF" });
            teams.Add(new Team() { LongName = "DETROIT RED WINGS", ShortName = "DETROIT", Abbreviation = "DET" });
            teams.Add(new Team() { LongName = "FLORIDA PANTHERS", ShortName = "FLORIDA", Abbreviation = "FLA" });
            teams.Add(new Team() { LongName = "MONTREAL CANADIENS", ShortName = "MONTREAL", Abbreviation = "MTL" });
            teams.Add(new Team() { LongName = "OTTAWA SENATORS", ShortName = "OTTAWA", Abbreviation = "OTT" });
            teams.Add(new Team() { LongName = "TAMPA BAY LIGHTNING", ShortName = "TAMPA BAY", Abbreviation = "TBL" });
            teams.Add(new Team() { LongName = "TORONTO MAPLE LEAFS", ShortName = "TORONTO", Abbreviation = "TOR" });

            // METROPOLITAN DIVISION
            teams.Add(new Team() { LongName = "CAROLINA HURRICANES", ShortName = "CAROLINA", Abbreviation = "CAR" });
            teams.Add(new Team() { LongName = "COLUMBUS BLUE JACKETS", ShortName = "COLUMBUS", Abbreviation = "CBJ" });
            teams.Add(new Team() { LongName = "NEW JERSEY DEVILS", ShortName = "NEW JERSEY", Abbreviation = "NJD" });
            teams.Add(new Team() { LongName = "NEW YORK RANGERS", ShortName = "NY RANGERS", Abbreviation = "NYR" });
            teams.Add(new Team() { LongName = "NEW YORK ISLANDERS", ShortName = "NY ISLANDERS", Abbreviation = "NYI" });
            teams.Add(new Team() { LongName = "PHILADELPHIA FLYERS", ShortName = "PHILADELPHIA", Abbreviation = "PHI" });
            teams.Add(new Team() { LongName = "PITTSBURGH PENGUINS", ShortName = "PITTSBURGH", Abbreviation = "PIT" });
            teams.Add(new Team() { LongName = "WASHINGTON CAPITALS", ShortName = "WASHINGTON", Abbreviation = "WSH" });

            // ###### WESTERN CONFERENCE ######

            // CENTRAL DIVISION
            teams.Add(new Team() { LongName = "CHICAGO BLACKHAWKS", ShortName = "CHICAGO", Abbreviation = "CHI" });
            teams.Add(new Team() { LongName = "COLORADO AVALANCHE", ShortName = "COLORADO", Abbreviation = "COL" });
            teams.Add(new Team() { LongName = "DALLAS STARS", ShortName = "DALLAS", Abbreviation = "DAL" });
            teams.Add(new Team() { LongName = "MINNESOTA WILD", ShortName = "MINNESOTA", Abbreviation = "MIN" });
            teams.Add(new Team() { LongName = "NASHVILLE PREDATORS", ShortName = "NASHVILLE", Abbreviation = "NSH" });
            teams.Add(new Team() { LongName = "ST. LOUIS BLUES", ShortName = "ST LOUIS", Abbreviation = "STL" });
            teams.Add(new Team() { LongName = "WINNIPEG JETS", ShortName = "WINNIPEG", Abbreviation = "WPG" });

            // PACIFIC DIVISION
            teams.Add(new Team() { LongName = "ANAHEIM DUCKS", ShortName = "ANAHEIM", Abbreviation = "ANA" });
            teams.Add(new Team() { LongName = "ARIZONA COYOTES", ShortName = "ARIZONA", Abbreviation = "ARI" });
            teams.Add(new Team() { LongName = "CALGARY FLAMES", ShortName = "CALGARY", Abbreviation = "CGY" });
            teams.Add(new Team() { LongName = "EDMONTON OILERS", ShortName = "EDMONTON", Abbreviation = "EDM" });
            teams.Add(new Team() { LongName = "LOS ANGELES KINGS", ShortName = "LOS ANGELES", Abbreviation = "LAK" });
            teams.Add(new Team() { LongName = "SAN JOSE SHARKS", ShortName = "SAN JOSE", Abbreviation = "SJS" });
            teams.Add(new Team() { LongName = "VANCOUVER CANUCKS", ShortName = "VANCOUVER", Abbreviation = "VAN" });

            foreach (Team team in teams)
                context.Teams.Add(team);

            base.Seed(context);
        }
    }
}
