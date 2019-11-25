using System;
using System.Configuration;
using System.Linq;

namespace Custom_WoL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser(ConfigurationManager.AppSettings["logPath"]);
            parser.Parse();
            parser.BuildEncountersList();
            PrettyPrinter.PrintEncountersInfos(parser.Encounters);
            Console.ReadKey();
        }
    }
}
