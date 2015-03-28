using HtmlAgilityPack;
using NLog;
using ParseNhlBoxscores.Models;
using Fizzler.Systems.HtmlAgilityPack;
using System;
using ParseNhlBoxscores.Services;
using static ParseNhlBoxscores.ParseNhlBoxscoresHelpers;
using System.Linq;

namespace ParseNhlBoxscores
{
    public class ParsePlayerStats
    {
        private static ParseNhlBoxscoresContext db = new ParseNhlBoxscoresContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TeamService teamService = new TeamService();

        public static void GetPlayers()
        {
            var web = new HtmlWeb();
            var document = web.Load("http://www.nhl.com/stats/player?fetchKey=20152ALLSASAll&viewName=summary&sort=points&gp=1&pg=1");
            var maxPage = ParseNhlBoxscoresHelpers.GetMaxPage(document);

            for (int i = 1; i <= maxPage; i++)
            {
                var w = new HtmlWeb();
                var url = "http://www.nhl.com/stats/player?fetchKey=20152ALLSASAll&viewName=summary&sort=points&gp=1&pg=" + i;
                var schedulePage = w.Load(url);
                var trlist = schedulePage.DocumentNode.QuerySelectorAll("table.data.stats>tbody>tr");

                foreach (var tr in trlist)
                {
                    try
                    {
                        

                        // Get Nhl.com ID
                        var playerUrl = tr.QuerySelector("td:nth-child(2) > a").GetAttributeValue("href", "ERROR");
                        var splittedPlayerUrl = playerUrl.Split('=');
                        var nhlId = ConvertStringToInt(splittedPlayerUrl[1]);

                        // Get all other values
                        var name = tr.QuerySelector("td:nth-child(2) > a").InnerText;
                        var team = tr.QuerySelector("td:nth-child(3) > a").InnerText;
                        var teamId = teamService.GetTeam(team).Id;
                        var position = tr.QuerySelector("td:nth-child(4)").InnerText;
                        var gamesPlayed = ConvertStringToInt(tr.QuerySelector("td:nth-child(5)").InnerText);
                        var goals = ConvertStringToInt(tr.QuerySelector("td:nth-child(6)").InnerText);
                        var assists = ConvertStringToInt(tr.QuerySelector("td:nth-child(7)").InnerText);
                        var points = ConvertStringToInt(tr.QuerySelector("td:nth-child(8)").InnerText);
                        var plusMinus = ConvertStringToInt(tr.QuerySelector("td:nth-child(9)").InnerText);
                        var pim = ConvertStringToInt(tr.QuerySelector("td:nth-child(10)").InnerText);


                        // Update if player already exists
                        Player player = db.Players.SingleOrDefault(p => p.NhlId == nhlId); //b => b.BookNumber == bookNumber
                        if (player != null)
                        {
                            player.TeamId = teamId;
                            player.Position = position;
                            player.GamesPlayed = gamesPlayed;
                            player.Goals = goals;
                            player.Assists = assists;
                            player.Points = points;
                            player.PlusMinus = plusMinus;
                            player.Pim = pim;

                            //db.SaveChanges();
                            logger.Info("Updated player: " + player.Name);
                        }
                        // Create new player
                        else
                        {
                            Player newplayer = new Player();
                            newplayer.Name = name;
                            newplayer.NhlId = nhlId;
                            newplayer.TeamId = teamId;
                            newplayer.Position = position;
                            newplayer.GamesPlayed = gamesPlayed;
                            newplayer.Goals = goals;
                            newplayer.Assists = assists;
                            newplayer.Points = points;
                            newplayer.PlusMinus = plusMinus;
                            newplayer.Pim = pim;

                            // Date of birth
                            newplayer.Birthdate = GetPlayerBirthdate(Convert.ToInt32(nhlId));

                            db.Players.Add(newplayer);
                            logger.Info("Adding player: " + newplayer.Name);
                        }

                    }
                    catch (Exception e)
                    {
                        logger.Error("Cannot get player information");
                    }


                }
            }
            db.SaveChanges();

        }

        private static DateTime GetPlayerBirthdate(int nhlId)
        {
            var web = new HtmlWeb();
            var url = "http://www.nhl.com/ice/player.htm?id=" + nhlId;
            HtmlDocument document = web.Load(url);
            HtmlNode biotable = document.DocumentNode.QuerySelector("table.bioInfo");
            string birthDateToSplit = biotable.QuerySelector("tr > td:nth-child(4)").InnerText;
            string [] splittedBirthDate = birthDateToSplit.Split('(');
            string birthDate = splittedBirthDate[0].Trim('\n');

            return DateTime.Parse(birthDate);
        }

    }
}
