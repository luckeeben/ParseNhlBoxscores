using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace ParseNhlBoxscores
{
    public class ParseSchedule
    {
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
    }
}
