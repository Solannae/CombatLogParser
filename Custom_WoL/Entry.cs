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

    public enum MissType
    {
        ABSORB,
        BLOCK,
        DEFLECT,
        DODGE,
        EVADE,
        IMMUNE,
        MISS,
        PARRY,
        REFLECT,
        RESIST
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

    public class DamageInfo
    {
        public int Amount { get; set; }
        public int Overkill { get; set; }
        public int School { get; set; }
        public int Resisted { get; set; }
        public int Blocked { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }
        public bool Glancing { get; set; }
        public bool Crushing { get; set; }
    }

    public class HealingInfo
    {
        public int Amount { get; set; }
        public int OverHealing { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }
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
        public DamageInfo Damage { get; set; }
        public HealingInfo Healing { get; set; }
        public EnvironmentalType Environment { get; set; }
        public AuraType Aura { get; set; }
        public int Amount { get; set; }

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

        public void FillSuffixFlags(string[] tokens)
        {
            AuraType tmp_aura;
            switch (Suffix)
            {
                case EventSuffix.DAMAGE:
                    Damage = new DamageInfo();   //TODO
                    break;

                case EventSuffix.MISSED:         //TODO
                    break;

                case EventSuffix.HEAL:
                    Healing = new HealingInfo(); //TODO
                    break;

                case EventSuffix.DRAIN:
                case EventSuffix.LEECH:
                    break;                       //TODO

                case EventSuffix.INTERRUPT:
                case EventSuffix.DISPEL_FAILED:
                    ExtraSpellCast = new Spell(int.Parse(tokens[12]), tokens[13], int.Parse(tokens[14]));
                    break;

                case EventSuffix.EXTRA_ATTACKS:
                    Amount = int.Parse(tokens[12]);
                    break;

                case EventSuffix.DISPEL:
                case EventSuffix.STOLEN:
                case EventSuffix.AURA_BROKEN_SPELL:
                    ExtraSpellCast = new Spell(int.Parse(tokens[12]), tokens[13], int.Parse(tokens[14]));
                    Enum.TryParse(tokens[15], out tmp_aura);
                    Aura = tmp_aura;
                    break;

                case EventSuffix.AURA_APPLIED:
                case EventSuffix.AURA_REMOVED:
                case EventSuffix.AURA_REFRESH:
                case EventSuffix.AURA_BROKEN:
                    Enum.TryParse(tokens[12], out tmp_aura);
                    Aura = tmp_aura;
                    break;

                case EventSuffix.AURA_APPLIED_DOSE:
                case EventSuffix.AURA_REMOVED_DOSE:
                    Enum.TryParse(tokens[12], out tmp_aura);
                    Aura = tmp_aura;
                    Amount = int.Parse(tokens[13]);
                    break;

                default:
                    break;
            }
        }
    }
}
