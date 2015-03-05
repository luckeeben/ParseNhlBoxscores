using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Convert.ToInt16(s);
        }

        public static TimeSpan ConvertToi(string s)
        {
            string[] sa = s.Split(':');
            return new TimeSpan(0, Convert.ToInt16(sa[0]), Convert.ToInt16(sa[1]));
        }
    }
}
