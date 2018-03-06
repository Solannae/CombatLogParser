using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    class Entity
    {
        public int Guid { get; set; }
        public string Name { get; set; }
        public int Flags { get; set; }
        public int Flags2 { get; set; }
    }

    class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int School { get; set; }
    }

    class Entry
    {
        public DateTime Timestamp { get; set; }
        public Entity SourceEntity { get; set; }
        public Entity DestEntity { get; set; }
    }
}
