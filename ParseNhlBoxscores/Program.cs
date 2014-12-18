using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace ParseNhlBoxscores
{
    class Program
    {
        static void Main()
        {
            List<string> gameIdList = GetLastNightGameIds();

            foreach (var gameId in gameIdList)
            {
                Console.WriteLine(GetPlayerGameLogs(gameId));
            }
            Console.ReadLine();



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

        static IEnumerable<PlayerGameLog> GetPlayerGameLogs(string gameId)
        {
            var playerGameLogList = new List<PlayerGameLog>();
            //var playerGameLog = new PlayerGameLog();

            var document = GetGameDocument(gameId);
            var allGridNodes = document.DocumentNode.QuerySelectorAll("body > table > tr:nth-child(8) > td > table > tr");

            foreach (HtmlNode tr in allGridNodes)
            {
                
            }



            Console.WriteLine();


            var playerGameLog = new PlayerGameLog();
            playerGameLogList.Add(playerGameLog);
            return playerGameLogList;


        }

    }
}
