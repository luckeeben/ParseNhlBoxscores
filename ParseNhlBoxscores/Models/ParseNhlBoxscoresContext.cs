using System.Data.Entity;

namespace ParseNhlBoxscores.Models
{
    public class ParseNhlBoxscoresContext : DbContext
    {
        public ParseNhlBoxscoresContext() : base("ParseNhlBoxscoresDB")
        {
            Database.SetInitializer<ParseNhlBoxscoresContext>(new ParseNhlBoxscoresDBInitializer());
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGameLog> PlayerGameLogs { get; set; }

    }
}
