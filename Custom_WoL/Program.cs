using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    public class Parser
    {
        public string Path { get; }

        public Parser(string path)
        {
            Path = path;
        }

        public List<Entry> Parse()
        {
            using (var file = File.OpenRead(Path))  
            using (var reader = new StreamReader(file))
            {
                var token = reader.ReadLine();
            }
            return new List<Entry>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = "D:\\Users\\Solannae\\Desktop\\World of Warcraft - Cataclysm enGB\\Logs\\WoWCombatLog.txt";
            Parser wol = new Parser(path);
            wol.Parse();
        }
    }
}
