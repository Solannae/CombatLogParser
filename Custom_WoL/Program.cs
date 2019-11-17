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
            PrettyPrinter.PrintEncountersInfos(parser.Encounters);
            Console.ReadKey();
        }
    }
}
