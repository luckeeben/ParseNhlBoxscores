﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Linq;
using ParseNhlBoxscores.Models;
using static ParseNhlBoxscores.ParseNhlBoxscoresHelpers;
using static ParseNhlBoxscores.ParseSchedule;
using ParseNhlBoxscores.Services;
using NLog;

namespace ParseNhlBoxscores
{
    public class ParseEventSummary
    {
        private static ParseNhlBoxscoresContext db = new ParseNhlBoxscoresContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ParseEventSummary()
        {

        }

        /// <summary>
        /// Save player game logs of all game in db
        /// </summary>
        static public void ProcessAllEventSummary()
        {
            IEnumerable<Game> gameList = db.Games;

            foreach(var game in gameList)
            {
                // Get Event Summary page
                var document = GetGameDocument(game.NhlGameId);

                // Save all player logs
                var gamelogList = GetPlayerGameLogs(game.Id, document);
                foreach (var playerGameLog in gamelogList)
                {
                    db.PlayerGameLogs.Add(playerGameLog);
                    logger.Info("Saving Game log:" + playerGameLog.Print());
                }
            }
            db.SaveChanges();
        }

        static public void ProcessEventSummary(Game game)
        {

            // Get Event Summary page
            var document = GetGameDocument(game.NhlGameId);

            // Save all player logs
            var gamelogList = GetPlayerGameLogs(game.Id, document);
            foreach (var playerGameLog in gamelogList)
            {
                db.PlayerGameLogs.Add(playerGameLog);
                logger.Info("Saving Game log:" + playerGameLog.Print());
            }

            db.SaveChanges();
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

        static Game GetGameInfo(string gameId, HtmlDocument document)
        {
            var game = new Game();
            var TS = new TeamService();

            //var document = GetGameDocument(gameId);

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
            game.VisitorTeamId = TS.GetTeam(visitorTdText[0]).Id;

            // Set visitor score
            game.VisitorScore = Convert.ToInt32(document.DocumentNode.QuerySelector("#Visitor > tr:nth-child(2) > td > table > tr > td:nth-child(2)").InnerText);

            // Set home team
            var homeTd = document.DocumentNode.QuerySelector("#Home > tr:nth-child(3) > td").InnerText;
            string[] homeTdText = Regex.Split(homeTd, "Game");
            game.HomeTeam = homeTdText[0];
            game.HomeTeamId = TS.GetTeam(homeTdText[0]).Id;

            // Set home team score
            game.HomeScore = Convert.ToInt32(document.DocumentNode.QuerySelector("#Home > tr:nth-child(2) > td > table > tr > td:nth-child(2)").InnerText);

            return game;
        }



        static List<PlayerGameLog> GetPlayerGameLogs(int gameId, HtmlDocument document)
        {
            var playerGameLogList = new List<PlayerGameLog>();
            

            String team = null;

            //var document = GetGameDocument(gameId);
            var allGridNodes = document.DocumentNode.QuerySelectorAll("body > table > tr:nth-child(8) > td > table > tr");

            foreach (HtmlNode tr in allGridNodes)
            {
                var tds = tr.QuerySelectorAll("td");
                var firstTd = tds.First();

                if (firstTd.InnerText.Contains("&nbsp")) // Not a valid TR line
                {
                    continue;
                }

                if (firstTd.Attributes["class"] != null)
                {
                    String tdClass = firstTd.Attributes["class"].Value;

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
                    String trClass = tr.Attributes["class"].Value;
                    if ((trClass.Contains("evenColor") || trClass.Contains("oddColor")) && !(firstTd.InnerText.Contains("TEAM TOTALS"))) // This is a player log line
                    {
                        if (!tds.ElementAt(1).InnerText.Contains("G")) // Not importing goalies
                        {
                            var playerGameLog = new PlayerGameLog { GameId = gameId };

                            // Set Player Number
                            playerGameLog.PlayerNumber = ConvertStringToInt(tds.ElementAt(0).InnerText);

                            // Set Player Name
                            playerGameLog.PlayerName = tds.ElementAt(2).InnerText;

                            playerGameLog.Goals = ConvertStringToInt(tds.ElementAt(3).InnerText);
                            playerGameLog.Assists = ConvertStringToInt(tds.ElementAt(4).InnerText);
                            playerGameLog.Points = ConvertStringToInt(tds.ElementAt(5).InnerText);
                            playerGameLog.PlusMinus = ConvertStringToInt(tds.ElementAt(6).InnerText);
                            playerGameLog.Penalty = ConvertStringToInt(tds.ElementAt(7).InnerText);
                            playerGameLog.PenaltyMinutes = ConvertStringToInt(tds.ElementAt(8).InnerText);
                            playerGameLog.TotalToi = ConvertToi(tds.ElementAt(9).InnerText);
                            playerGameLog.PpToi = ConvertToi(tds.ElementAt(12).InnerText);
                            playerGameLog.ShToi = ConvertToi(tds.ElementAt(13).InnerText);
                            playerGameLog.EvToi = ConvertToi(tds.ElementAt(14).InnerText);
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
