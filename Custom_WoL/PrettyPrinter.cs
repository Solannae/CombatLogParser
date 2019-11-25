using System;
using System.Collections.Generic;
using System.Linq;

namespace Custom_WoL
{
    class PrettyPrinter
    {
        public static void PrintEncountersInfos(List<Encounter> encounters)
        {
            foreach (var fight in encounters)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Begin : " + fight.Start + ", End : " + fight.End);
                Console.WriteLine("Fight : " + fight.NPC.OrderByDescending(u => u.Value.DamageTaken).First().Key.Name);
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Damage Done :");

                foreach (var enemy in fight.Players.Where(u => u.Value.DamageDone != 0))
                {
                    Console.WriteLine(enemy.Key.Name + " Damage Done : " + enemy.Value.DamageDone);
                }
            }
        }
    }
}
