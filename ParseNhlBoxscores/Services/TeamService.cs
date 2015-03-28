using ParseNhlBoxscores.Models;
using System.Linq;

namespace ParseNhlBoxscores.Services
{
    public class TeamService
    {
        private ParseNhlBoxscoresContext db = new ParseNhlBoxscoresContext();

        public Team GetTeam(int id)
        {
            return db.Teams.Find(id);
        }

        public Team GetTeam(string s)
        {
            var L2EQuery = from t in db.Teams
                           where t.LongName.Contains(s) || t.ShortName.Contains(s) || t.Abbreviation.Contains(s)
                           select t;

            return L2EQuery.FirstOrDefault<Team>();
        }
    }
}
