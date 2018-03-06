using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    public enum EventPrefix
    {
        SWING,
        RANGE,
        SPELL,
        SPELL_PERIODIC,
        SPELL_BUILDING,
        ENVIRONMENTAL
    };

    public enum EventSuffix
    {
        DAMAGE,
        MISSED,
        HEAL,
        ENERGIZE,
        DRAIN,
        LEECH,
        INTERRUPT,
        DISPEL,
        DISPEL_FAILED,
        STOLEN,
        EXTRA_ATTACKS,
        AURA_APPLIED,
        AURA_REMOVED,
        AURA_APPLIED_DOSE,
        AURA_REMOVED_DOSE,
        AURA_REFRESH,
        AURA_BROKEN,
        AURA_BROKEN_SPELL,
        CAST_START,
        CAST_SUCCESS,
        CAST_FAILED,
        INSTAKILL,
        DURABILITY_DAMAGE,
        DURABILITY_DAMAGE_ALL,
        CREATE,
        SUMMON,
        RESURRECT
    };

    public enum AuraType
    {
        BUFF,
        DEBUFF
    };

    public class Entity
    {
        public long Guid { get; set; }
        public string Name { get; set; }
        public int Flags { get; set; }
        public int Flags2 { get; set; }
    }

    public class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int School { get; set; }
    }

    public class Entry
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public EventPrefix Prefix { get; set; }
        public EventSuffix Suffix { get; set; }
        public Entity SourceEntity { get; set; }
        public Entity DestEntity { get; set; }
        public Spell SpellCast { get; set; }
        public Spell ExtraSpellCast { get; set; }
        public AuraType Aura { get; set; }
    }
}
