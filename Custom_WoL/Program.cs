﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Custom_WoL
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "Tests\\WoWCombatLog.txt";
            Parser parser = new Parser(path);
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
