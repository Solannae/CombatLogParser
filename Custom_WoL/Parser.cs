using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Custom_WoL.Libs;

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

        public void Parse()
        {
            Entries = new Queue<Entry>();
            using (var file = File.OpenRead(Path))
            using (var reader = new StreamReader(file))
            {
                string token;
                while ((token = reader.ReadLine()) != null)
                {
                    var timestamp = token.Split(new[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                    //Split CSV without taking into account quoted commas
                    var arguments = Regex.Split(token.Split(new[] { "  " }, 2, StringSplitOptions.RemoveEmptyEntries)[1], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    var tmpEntry = new Entry(DateTime.ParseExact(timestamp, "MM/dd hh:mm:ss.fff", CultureInfo.InvariantCulture));
                    tmpEntry.Fill(arguments);
                    Entries.Enqueue(tmpEntry);
                }
            }
        }

        public void BuildEncountersList()
        {
            Encounters = new List<Encounter>();
            while (Entries.Count != 0)
                Encounters.Add(new Encounter(Entries));

            //Remove encounters shorter than 5 seconds
            var fiveSec = new TimeSpan(0, 0, 5);
            Encounters.RemoveAll(u => u.End - u.Start < fiveSec);
            Encounters.RemoveAll(u => u.NPC.Count == 0);
            Encounters.RemoveAll(u => u.Players.All(v => v.Value.DamageDone == 0));
            MergeCloseEncounters();
        }

        public void MergeCloseEncounters()
        {
            var i = 0;
            while (i < Encounters.Count - 1)
            {
                var fifteenSec = new TimeSpan(0, 0, 0, 15);

                if (Encounters[i + 1].Start - Encounters[i].End < fifteenSec)
                {
                    //Merge both encounters
                    Encounters[i].End = Encounters[i + 1].End;

                    ArrayManipulation.MergeEncounterDictionaries(Encounters[i].Players, Encounters[i + 1].Players);
                    ArrayManipulation.MergeEncounterDictionaries(Encounters[i].NPC, Encounters[i + 1].NPC);
                    ArrayManipulation.MergeEncounterDictionaries(Encounters[i].Pets, Encounters[i + 1].Pets);

                    Encounters.RemoveAt(i + 1);
                    --i;
                }

                ++i;
            }
        }
    }
}
