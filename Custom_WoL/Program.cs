using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Custom_WoL
{
    public class Parser
    {
        public string Path { get; set; }

        public Parser(string path)
        {
            Path = path;
        }

        public DateTime FormatDateTime(string time)
        {
            string date_part = time.Split(' ')[0];
            string hour_part = time.Split(' ')[1];
            return new DateTime(DateTime.Now.Year, int.Parse(date_part.Split('/')[0]), int.Parse(date_part.Split('/')[1]),
                                int.Parse(hour_part.Split(':')[0]), int.Parse(hour_part.Split(':')[1]), int.Parse(hour_part.Split(':')[2].Split('.')[0]),
                                int.Parse(hour_part.Split(':')[2].Split('.')[1]));
        }

        public Queue<Entry> Parse()
        {
            Queue<Entry> entries = new Queue<Entry>();
            using (var file = File.OpenRead(Path))  
            using (var reader = new StreamReader(file))
            {
                string token;
                while ((token = reader.ReadLine()) != null)
                {
                    var timestamp = token.Split(new string[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                    var arguments = Regex.Split(token.Split(new string[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[1], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    Entry tmp_entry = new Entry(FormatDateTime(timestamp));
                    tmp_entry.Fill(arguments);
                    entries.Enqueue(tmp_entry);
                }
            }
            return entries;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = "Tests\\WoWCombatLog.txt";
            Parser wol = new Parser(path);
            Queue<Entry> entries = wol.Parse();
            List<Encounter> encounters = new List<Encounter>();
            
            while (entries.Count != 0)  
                encounters.Add(new Encounter(entries));

            foreach (var fight in encounters)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Begin : " + fight.Start);
                Console.WriteLine("---------------------------------------------------\nDamage Taken :");
                foreach (var enemy in fight.Enemies.Where(u => u.Value.DamageTaken != 0))
                {
                    Console.WriteLine(enemy.Key.Name + " Dakage Taken : " + enemy.Value.DamageTaken);
                }

            }

            Console.ReadKey();
        }
    }
}
