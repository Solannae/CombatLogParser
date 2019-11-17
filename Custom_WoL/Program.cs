using System;
using System.Configuration;
using System.Linq;

namespace Custom_WoL
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser(ConfigurationManager.AppSettings["logPath"]);
            parser.Parse();
            parser.BuildEncountersList();            

            foreach (var fight in parser.Encounters)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Begin : " + fight.Start + ", End : " + fight.End);
                Console.WriteLine("Fight : " + fight.NPC.OrderByDescending  (u => u.Value.DamageTaken).First().Key.Name);
                Console.WriteLine("---------------------------------------------------\nDamage Done :");
                foreach (var enemy in fight.Players.Where(u => u.Value.DamageDone != 0))
                {
                    Console.WriteLine(enemy.Key.Name + " Damage Done : " + enemy.Value.DamageDone);
                }

            }

            Console.ReadKey();
        }
    }
}
