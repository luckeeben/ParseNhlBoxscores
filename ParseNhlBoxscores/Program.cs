using System;
using System.Collections.Generic;
using static ParseNhlBoxscores.ParseEventSummary;

namespace ParseNhlBoxscores
{
    class Program
    {
        static void Main()
        {
            SaveGameLogs();

            //using (var ctx = new ParseNhlBoxscoresContext())
            //{
            //    PlayerGameLog log = new PlayerGameLog() { PlayerName = "Benoit Cantin" };

            //    ctx.PlayerGameLogs.Add(log);
            //    ctx.SaveChanges();
            //}


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





        


    }
}
