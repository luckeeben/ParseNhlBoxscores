using System;
using System.Collections.Generic;
using static ParseNhlBoxscores.ParseEventSummary;
using static ParseNhlBoxscores.ParseSchedule;
using static ParseNhlBoxscores.ParsePlayerStats;

namespace ParseNhlBoxscores
{
    class Program
    {
        static void Main()
        {
            GetPlayers();
            //GetGames();

            //ProcessAllEventSummary();
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
