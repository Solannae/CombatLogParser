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

    public enum PowerType
    {
        HEALTH = -2,
        MANA = 0,
        RAGE = 1,
        FOCUS = 2,
        ENERGY = 3,
        COMBO_POINTS = 4,
        RUNES = 5,
        RUNIC_POWER = 6,
        SOUL_SHARDS = 7,
        LUNAR_POWER = 8,
        HOLY_POWER = 9,
        ALTERNATE_POWER = 10,
        MAELSTROM = 11,
        CHI = 12,
        INSANITY = 13,
        OBSOLETE = 14,
        OBSOLETE_2 = 15,
        ARCANE_CHARGES = 16,
        FURY = 17,
        PAIN = 18
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

        public DamageInfo(int _amount, int _overkill, int _school, int _res,
                          int _block, int _abs, bool _crit, bool _glance, bool _crush)
        {
            Amount = _amount;
            Overkill = _overkill;
            School = _school;
            Resisted = _res;
            Blocked = _block;
            Absorbed = _abs;
            Critical = _crit;
            Glancing = _glance;
            Crushing = _crush;
        }
    }

    public class HealingInfo
    {
        public int Amount { get; set; }
        public int OverHealing { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }

        public HealingInfo(int _amount, int _over, int _abs, bool _crit)
        {
            Amount = _amount;
            OverHealing = _over;
            Absorbed = _abs;
            Critical = _crit;
        }
    }

    public class MissedInfo
    {
        public bool IsOffHand { get; set; }
        public int AmountMissed { get; set; }

        public MissedInfo(bool _oh, int _amount)
        {
            IsOffHand = _oh;
            AmountMissed = _amount;
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
        public DamageInfo Damage { get; set; }
        public HealingInfo Healing { get; set; }
        public MissedInfo Missed { get; set; }
        public EnvironmentalType Environment { get; set; }
        public AuraType Aura { get; set; }
        public PowerType Power { get; set; }
        public int Amount { get; set; }
        public int ExtraAmount { get; set; }

        public Entry(DateTime _time)
        {
            Timestamp = _time;
        }

        public void Fill(string[] tokens)
        {
            FillBasic(tokens);
            FindPrefix(tokens[0]);
            FindSuffix(tokens[0]);
            FillPrefixFlags(tokens);
            FillSuffixFlags(tokens);
        }

        public void FillBasic(string[] tokens)
        {
            SourceEntity = new Entity(long.Parse(tokens[1]), tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
            DestEntity = new Entity(long.Parse(tokens[5]), tokens[6], int.Parse(tokens[7]), int.Parse(tokens[8]));
        }

        public void FindPrefix(string event_type)
        {
            if (event_type.Contains("SWING"))
                Prefix = EventPrefix.SWING;
            else if (event_type.Contains("RANGE"))
                Prefix = EventPrefix.RANGE;
            else if (event_type.Contains("ENVIRONMENTAL"))
                Prefix = EventPrefix.ENVIRONMENTAL;
            else if (event_type.Contains("SPELL_PERIODIC"))
                Prefix = EventPrefix.SPELL_PERIODIC;
            else if (event_type.Contains("SPELL_BUILDING"))
                Prefix = EventPrefix.SPELL_BUILDING;
            else if (event_type.Contains("SPELL"))
                Prefix = EventPrefix.SPELL;
        }

        public void FindSuffix(string event_type)
        {
            Enum.TryParse(event_type.Split(new string[] { Prefix.ToString() }, StringSplitOptions.RemoveEmptyEntries)[0], out EventSuffix suff);
            Suffix = suff;
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
                    Damage = new DamageInfo(int.Parse(tokens[12]), int.Parse(tokens[13]), int.Parse(tokens[14]),
                                            int.Parse(tokens[15]), int.Parse(tokens[16]), int.Parse(tokens[17]),
                                            bool.Parse(tokens[18]), bool.Parse(tokens[19]), bool.Parse(tokens[20]));
                    break;

                case EventSuffix.MISSED:
                    Missed = new MissedInfo(bool.Parse(tokens[13]), int.Parse(tokens[15]));
                    break;

                case EventSuffix.HEAL:
                    Healing = new HealingInfo(int.Parse(tokens[12]), int.Parse(tokens[13]), int.Parse(tokens[14]), bool.Parse(tokens[15]));
                    break;

                case EventSuffix.DRAIN:
                case EventSuffix.LEECH:
                    Amount = int.Parse(tokens[12]);
                    Power = (PowerType)int.Parse(tokens[13]);
                    ExtraAmount = int.Parse(tokens[14]);
                    break;

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
