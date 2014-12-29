using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Linq;

namespace ParseNhlBoxscores
{
    class Program
    {
        static void Main()
        {
            List<string> gameIdList = GetLastNightGameIds();

            foreach (var gameId in gameIdList)
            {
                var a = GetPlayerGameLogs(gameId);
                a[0].Print();
                continue;
            }




        }

        /* temp */
        static void PrintList(IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        static List<string> GetLastNightGameIds()
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

        static string GetEventSummaryUrl(string gameId)
        {
            return "http://www.nhl.com/scores/htmlreports/20142015/ES0" + gameId + ".HTM";
        }

        static HtmlDocument GetGameDocument(string gameId)
        {
            string gameUrl = GetEventSummaryUrl(gameId);
            var web = new HtmlWeb();
            return web.Load(gameUrl);
        }

        static Game GetGameInfo(string gameId)
        {
            var game = new Game();

            var document = GetGameDocument(gameId);
            
            // Set game id
            game.NhlGameId = gameId;

            // Set game date
            var datetext = document.DocumentNode.QuerySelector("#GameInfo > tr:nth-child(4) > td").InnerText;
            var found = datetext.IndexOf(", ");
            game.Date = DateTime.Parse(datetext.Substring(found + 2));

            // Set visitor team
            var visitorTd = document.DocumentNode.QuerySelector("#Visitor > tr:nth-child(3) > td").InnerText;
            string[] visitorTdText = Regex.Split(visitorTd, "Game");
            game.VisitorTeam = visitorTdText[0];

            // Set visitor score
            game.VisitorScore = Convert.ToInt32(document.DocumentNode.QuerySelector("#Visitor > tr:nth-child(2) > td > table > tr > td:nth-child(2)").InnerText);

            // Set home team
            var homeTd = document.DocumentNode.QuerySelector("#Home > tr:nth-child(3) > td").InnerText;
            string[] homeTdText = Regex.Split(homeTd, "Game");
            game.HomeTeam = homeTdText[0];

            // Set home team score
            game.HomeScore = Convert.ToInt32(document.DocumentNode.QuerySelector("#Home > tr:nth-child(2) > td > table > tr > td:nth-child(2)").InnerText);

            return game;
        }

        static int ConvertStringToInt(string s)
        {
            if (s.Contains("&nbsp") || s.Contains(" "))
            {
                return 0;
            }
            else
                return Convert.ToInt16(s);
        }

        static List<PlayerGameLog> GetPlayerGameLogs(string gameId)
        {
            var playerGameLogList = new List<PlayerGameLog>();
            var playerGameLog = new PlayerGameLog();
            playerGameLog.NhlGameId = gameId;
            
            String team = null;

            var document = GetGameDocument(gameId);
            var allGridNodes = document.DocumentNode.QuerySelectorAll("body > table > tr:nth-child(8) > td > table > tr");

            foreach (HtmlNode tr in allGridNodes)
            {
                var tds = tr.QuerySelectorAll("td");
                var firstTd = tds.First();

                String tdClass;
                String trClass;

                if (firstTd.InnerText.Contains("&nbsp")) // Not a valid TR line
                {
                    continue;
                }

                if (firstTd.Attributes["class"] != null)
                {
                    tdClass = firstTd.Attributes["class"].Value;

                    if ((tdClass.Contains("visitorsectionheading") && !(firstTd.InnerText.Contains("TOT")) && !(firstTd.InnerText.Contains("TEAM TOTALS")))) // This is a team header line
                    {
                        team = firstTd.InnerText;
                        continue;
                    }

                    if ((tdClass.Contains("homesectionheading") && !(firstTd.InnerText.Contains("TOT")) && !(firstTd.InnerText.Contains("TEAM TOTALS")))) // This is a team header line
                    {
                        team = firstTd.InnerText;
                        continue;
                    }
                }

                if (tr.Attributes["class"] != null)
                {
                    trClass = tr.Attributes["class"].Value;
                    if ((trClass.Contains("evenColor") || trClass.Contains("oddColor")) && !(firstTd.InnerText.Contains("TEAM TOTALS"))) // This is a player log line
                    {
                        if (!tds.ElementAt(1).InnerText.Contains("G")) // Not importing goalies
                        {
                            // Set Player Number
                            playerGameLog.PlayerNumber = ConvertStringToInt(tds.ElementAt(0).InnerText);
                            
                            // Set Player Name
                            playerGameLog.PlayerName = tds.ElementAt(2).InnerText;
                            
                            // Set Goals
                            playerGameLog.Goals = ConvertStringToInt(tds.ElementAt(3).InnerText);
                            playerGameLog.Assists = ConvertStringToInt(tds.ElementAt(4).InnerText);
                            playerGameLog.Points = ConvertStringToInt(tds.ElementAt(5).InnerText);
                            playerGameLog.PlusMinus = ConvertStringToInt(tds.ElementAt(6).InnerText);
                            playerGameLog.Penalty = ConvertStringToInt(tds.ElementAt(7).InnerText);
                            playerGameLog.PenaltyMinutes = ConvertStringToInt(tds.ElementAt(8).InnerText);
                            playerGameLog.TotalToi = ConvertStringToInt(tds.ElementAt(9).InnerText);        // Must convert to 100 sec
                            playerGameLog.PpToi = ConvertStringToInt(tds.ElementAt(12).InnerText);          // Must convert to 100 sec
                            playerGameLog.ShToi = ConvertStringToInt(tds.ElementAt(13).InnerText);          // Must convert to 100 sec
                            playerGameLog.EvToi = ConvertStringToInt(tds.ElementAt(14).InnerText);          // Must convert to 100 sec
                            playerGameLog.Shots = ConvertStringToInt(tds.ElementAt(15).InnerText);
                            playerGameLog.AttemptsBlocked = ConvertStringToInt(tds.ElementAt(16).InnerText);
                            playerGameLog.MissedShots = ConvertStringToInt(tds.ElementAt(17).InnerText);
                            playerGameLog.Hits = ConvertStringToInt(tds.ElementAt(18).InnerText);
                            playerGameLog.Giveaways = ConvertStringToInt(tds.ElementAt(19).InnerText);
                            playerGameLog.Takeaways = ConvertStringToInt(tds.ElementAt(20).InnerText);
                            playerGameLog.BlockedShots = ConvertStringToInt(tds.ElementAt(21).InnerText);
                            playerGameLog.FaceoffsWon = ConvertStringToInt(tds.ElementAt(22).InnerText);
                            playerGameLog.FaceoffsLost = ConvertStringToInt(tds.ElementAt(23).InnerText);

                            playerGameLog.Team = team;
                            playerGameLogList.Add(playerGameLog);
                        }
                    }
                }

            }

            return playerGameLogList;

        }

        


    }
}
