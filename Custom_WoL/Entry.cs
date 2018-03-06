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

    public enum EnvironmentalType
    {
        DROWNING,
        FALLING,
        FATIGUE,
        FIRE,
        LAVA,
        SLIME
    };

    public class Entity
    {
        public long Guid { get; set; }
        public string Name { get; set; }
        public int Flags { get; set; }
        public int Flags2 { get; set; }

        public Entity(long _guid, string _name, int _flags, int _flags2)
        {
            Guid = _guid;
            Name = _name;
            Flags = _flags;
            Flags2 = _flags2;
        }
    }

    public class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int School { get; set; }

        public Spell(int _id, string _name, int _school)
        {
            ID = _id;
            Name = _name;
            School = _school;
        }
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
        public EnvironmentalType Environment { get; set; }
        public AuraType Aura { get; set; }

        public void FillBasic(string[] tokens)
        {
            SourceEntity = new Entity(long.Parse(tokens[1]), tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
            DestEntity = new Entity(long.Parse(tokens[5]), tokens[6], int.Parse(tokens[7]), int.Parse(tokens[8]));
        }

        public void FillPrefixFlags(string[] tokens)
        {
            switch (Prefix)
            {
                case EventPrefix.RANGE:
                case EventPrefix.SPELL:
                case EventPrefix.SPELL_PERIODIC:
                case EventPrefix.SPELL_BUILDING:
                    SpellCast = new Spell(int.Parse(tokens[9]), tokens[10], int.Parse(tokens[11]));
                    break;
                case EventPrefix.ENVIRONMENTAL:
                    Enum.TryParse(tokens[9], out EnvironmentalType Environment);
                    break;
                default:
                    break;
            }
        }
    }
}
