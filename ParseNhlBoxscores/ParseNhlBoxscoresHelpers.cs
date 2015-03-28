using HtmlAgilityPack;
using System;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;

namespace ParseNhlBoxscores
{
    public static class ParseNhlBoxscoresHelpers
    {
        public static int ConvertStringToInt(string s)
        {
            if (s.Contains("&nbsp") || s.Contains(" "))
            {
                return 0;
            }
            return Convert.ToInt32(s);
        }

        public static TimeSpan ConvertToi(string s)
        {
            if (s.Contains("&nbsp") || s.Contains(" "))
            {
                return new TimeSpan(0, 0, 0);
            }
            string[] sa = s.Split(':');
            return new TimeSpan(0, ConvertStringToInt(sa[0]), ConvertStringToInt(sa[1]));
        }

        public static int GetMaxPage(HtmlDocument document)
        {
            var aList = document.DocumentNode.QuerySelectorAll("#statsPage > div.paging > div.pages > a");
            var href = aList.Last().GetAttributeValue("href", "ERROR");
            if (!href.Contains("ERROR"))
            {
                return Convert.ToInt16(href.Split('=').Last());
            }
            return 1;
        }
    }
}
