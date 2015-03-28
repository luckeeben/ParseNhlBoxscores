using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Linq;
using ParseNhlBoxscores.Models;
using NLog;
using ParseNhlBoxscores.Services;

namespace ParseNhlBoxscores
{
    public class ParseSchedule
    {
        private static ParseNhlBoxscoresContext db = new ParseNhlBoxscoresContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<string> GetLastNightGameIds()
        {
            var web = new HtmlWeb();
            var document = web.Load("http://www.nhl.com/ice/gamestats.htm?season=20142015&gameType=2&team=&viewName=summary&pg=1");
            var trlist = document.DocumentNode.QuerySelectorAll("table.data.stats>tbody>tr");
            List<string> gameList = new List<string>();

            foreach (var tr in trlist)
            {
                DateTime thisDay = DateTime.Today.AddDays(-1);
                var currentDay = thisDay.ToString("dd");

                var link = tr.QuerySelector("td>a");
                var datetext = link.InnerText;
                string[] datesplit = datetext.Split(' ');

                if (datesplit[1].Equals(currentDay))
                {
                    var linkText = link.GetAttributeValue("href", "ERROR");
                    string[] lines = Regex.Split(linkText, "http://www.nhl.com/scores/htmlreports/20142015/GS0");
                    var gameid = lines[1].TrimEnd('.', 'H', 'T', 'M');
                    gameList.Add(gameid);

                }

            }
            return gameList;
        }

        public static void GetGames(bool allseason = true)
        {
            var teamService = new TeamService();
            var maxPage = 2;

            // if allseason is true, get all season games.  Else get only the first 2 pages of the schedule.
            if (allseason == true)
            {
                var web = new HtmlWeb();
                var document = web.Load("http://www.nhl.com/stats/game");
                maxPage = ParseNhlBoxscoresHelpers.GetMaxPage(document);
            }

            for (int i = 1; i <= maxPage; i++)
            {
                var w = new HtmlWeb();
                var url = "http://www.nhl.com/ice/gamestats.htm?fetchKey=20152ALLSATALL&viewName=summary&sort=gameDate&pg=" + i;
                var schedulePage = w.Load(url);
                var trlist = schedulePage.DocumentNode.QuerySelectorAll("table.data.stats>tbody>tr");

                foreach (var tr in trlist)
                {
                    String winningGoalie = tr.QuerySelector("td:nth-child(7)").InnerText;

                    if (winningGoalie != "") // no wining goalie -> game in progress
                    {
                        try
                        {
                            Game game = new Game();

                            var link = tr.QuerySelector("td>a");
                            String [] datetext = link.InnerText.Replace("'", "").Split(' ');

                            var linkText = link.GetAttributeValue("href", "ERROR");
                            string[] lines = Regex.Split(linkText, "http://www.nhl.com/scores/htmlreports/20142015/GS0");
                            String gameid = lines[1].TrimEnd('.', 'H', 'T', 'M');

                            // Set game id
                            game.NhlGameId = gameid;

                            // Set game date

                            String stringToParse = datetext[1] + " " + datetext[0] + " 20" + datetext[2];
                            game.Date = DateTime.Parse(stringToParse);

                            // Set visitor team
                            var visitorText = tr.QuerySelector("td:nth-child(2)").InnerText;
                            game.VisitorTeam = visitorText;
                            game.VisitorTeamId = teamService.GetTeam(visitorText).Id;

                            // Set visitor score
                            game.VisitorScore = Convert.ToInt32(tr.QuerySelector("td:nth-child(3)").InnerText);

                            // Set home team
                            var homeText = tr.QuerySelector("td:nth-child(4)").InnerText;
                            game.HomeTeam = homeText;
                            game.HomeTeamId = teamService.GetTeam(homeText).Id;

                            // Set home team score
                            game.HomeScore = Convert.ToInt32(tr.QuerySelector("td:nth-child(5)").InnerText);

                            db.Games.Add(game);
                            logger.Info("Saving game # " + gameid);

                            //ParseEventSummary.ProcessEventSummary(game);
                        }
                        catch (Exception e)
                        {
                            logger.Error("Cannot get game information for schedule row");
                        }


                    }

                }
            }
            db.SaveChanges();
        }


    }
}
