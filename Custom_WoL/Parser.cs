using System;
using System.Collections.Generic;
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
        public Queue<Entry> Entries { get; set; }
        public List<Encounter> Encounters { get; set; }

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

        public void Parse()
        {
            Entries = new Queue<Entry>();
            using (var file = File.OpenRead(Path))
            using (var reader = new StreamReader(file))
            {
                string token;
                while ((token = reader.ReadLine()) != null)
                {
                    var timestamp = token.Split(new string[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                    //Split CSV without taking into account quoted commas
                    var arguments = Regex.Split(token.Split(new string[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[1], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    Entry tmp_entry = new Entry(FormatDateTime(timestamp));
                    tmp_entry.Fill(arguments);
                    Entries.Enqueue(tmp_entry);
                }
            }
        }

        public void BuildEncountersList()
        {
            Encounters = new List<Encounter>();
            while (Entries.Count != 0)
                Encounters.Add(new Encounter(Entries));

            //Remove encounters shorter than 5 seconds
            TimeSpan five_sec = new TimeSpan(0, 0, 5);
            Encounters.RemoveAll(u => u.End - u.Start < five_sec);
        }
    }
}
