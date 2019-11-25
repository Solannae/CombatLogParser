using System;

namespace Custom_WoL
{
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

        public Entry(DateTime time)
        {
            Timestamp = time;
        }

        public int HexaToInt(string nb)
        {
            if (nb.Contains("0x"))
                nb = nb.Replace("0x", string.Empty);

            return int.Parse(nb, System.Globalization.NumberStyles.HexNumber);
        }

        public ulong HexaToLong(string nb)
        {
            if (nb.Contains("0x"))
                nb = nb.Replace("0x", string.Empty);

            return ulong.Parse(nb, System.Globalization.NumberStyles.HexNumber);
        }

        public bool ToBool(string token)
        {
            return token == "nil";
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
            SourceEntity = new Entity(HexaToLong(tokens[1]), tokens[2], HexaToInt(tokens[3]), HexaToInt(tokens[4]));
            DestEntity = new Entity(HexaToLong(tokens[5]), tokens[6], HexaToInt(tokens[7]), HexaToInt(tokens[8]));
        }

        public void FindPrefix(string eventType)
        {
            //Three specific cases where an event type must be considered as another one
            if (eventType == "DAMAGE_SHIELD" || eventType == "DAMAGE_SPLIT" || eventType == "DAMAGE_SHIELD_MISSED")
                Prefix = EventPrefix.SPELL;
            else if (eventType.Contains("SWING"))
                Prefix = EventPrefix.SWING;
            else if (eventType.Contains("RANGE"))
                Prefix = EventPrefix.RANGE;
            else if (eventType.Contains("ENVIRONMENTAL"))
                Prefix = EventPrefix.ENVIRONMENTAL;
            else if (eventType.Contains("SPELL_PERIODIC"))
                Prefix = EventPrefix.SPELL_PERIODIC;
            else if (eventType.Contains("SPELL_BUILDING"))
                Prefix = EventPrefix.SPELL_BUILDING;
            else if (eventType.Contains("SPELL"))
                Prefix = EventPrefix.SPELL;
            else if (eventType.Contains("ENCHANT"))
                Prefix = EventPrefix.ENCHANT;
            else if (eventType.Contains("PARTY"))
                Prefix = EventPrefix.PARTY;
            else if (eventType.Contains("UNIT"))
                Prefix = EventPrefix.UNIT;
        }

        public void FindSuffix(string eventType)
        {
            //Three specific cases where an event type must be considered as another one
            if (eventType == "DAMAGE_TYPE" || eventType == "DAMAGE_SPLIT")
                Suffix = EventSuffix.DAMAGE;
            else if (eventType == "DAMAGE_SHIELD_MISSED")
                Suffix = EventSuffix.MISSED;
            else
            {
                var suffixStr = eventType.Split(new[] { Prefix.ToString() }, StringSplitOptions.RemoveEmptyEntries)[0].Substring(1);
                Enum.TryParse(suffixStr, out EventSuffix suff);
                Suffix = suff;
            }
        }

        public void FillPrefixFlags(string[] tokens)
        {
            switch (Prefix)
            {
                case EventPrefix.RANGE:
                case EventPrefix.SPELL:
                case EventPrefix.SPELL_PERIODIC:
                case EventPrefix.SPELL_BUILDING:
                    SpellCast = new Spell(HexaToInt(tokens[9]), tokens[10], HexaToInt(tokens[11]));
                    break;

                case EventPrefix.ENVIRONMENTAL:
                    Enum.TryParse(tokens[9], out EnvironmentalType env);
                    Environment = env;
                    break;
            }
        }

        public void FillSuffixFlags(string[] tokens)
        {
            AuraType tmpAura;
            switch (Suffix)
            {
                case EventSuffix.DAMAGE:
                    if (Prefix == EventPrefix.SWING)
                        Damage = new DamageInfo(HexaToInt(tokens[9]), HexaToInt(tokens[10]), HexaToInt(tokens[11]),
                                                HexaToInt(tokens[12]), HexaToInt(tokens[13]), HexaToInt(tokens[14]),
                                                ToBool(tokens[15]), ToBool(tokens[16]), ToBool(tokens[17]));
                    else if (Prefix == EventPrefix.ENVIRONMENTAL)
                        Damage = new DamageInfo(HexaToInt(tokens[10]), HexaToInt(tokens[11]), HexaToInt(tokens[12]),
                                                HexaToInt(tokens[13]), HexaToInt(tokens[14]), HexaToInt(tokens[15]),
                                                ToBool(tokens[16]), ToBool(tokens[17]), ToBool(tokens[18]));
                    else
                        Damage = new DamageInfo(HexaToInt(tokens[12]), HexaToInt(tokens[13]), HexaToInt(tokens[14]),
                                                HexaToInt(tokens[15]), HexaToInt(tokens[16]), HexaToInt(tokens[17]),
                                                ToBool(tokens[18]), ToBool(tokens[19]), ToBool(tokens[20]));
                    break;

                case EventSuffix.MISSED:
                    if (Prefix == EventPrefix.SWING)
                    {
                        Enum.TryParse(tokens[9], out MissedInfo.MissType miss);
                        switch (miss)
                        {
                            case MissedInfo.MissType.DODGE:
                            case MissedInfo.MissType.IMMUNE:
                            case MissedInfo.MissType.MISS:
                            case MissedInfo.MissType.PARRY:
                                Missed = new MissedInfo(miss);
                                break;
                            default:
                                Missed = new MissedInfo(HexaToInt(tokens[10]));
                                break;
                        }
                    }
                    else
                    {
                        Enum.TryParse(tokens[12], out MissedInfo.MissType miss);
                        switch (miss)
                        {
                            case MissedInfo.MissType.DEFLECT:
                            case MissedInfo.MissType.DODGE:
                            case MissedInfo.MissType.EVADE:
                            case MissedInfo.MissType.IMMUNE:
                            case MissedInfo.MissType.MISS:
                            case MissedInfo.MissType.PARRY:
                            case MissedInfo.MissType.REFLECT:
                                Missed = new MissedInfo(miss);
                                break;
                            default:
                                Missed = new MissedInfo(HexaToInt(tokens[13]));
                                break;
                        }
                    }
                    break;

                case EventSuffix.HEAL:
                    Healing = new HealingInfo(HexaToInt(tokens[12]), HexaToInt(tokens[13]), HexaToInt(tokens[14]), ToBool(tokens[15]));
                    break;

                case EventSuffix.DRAIN:
                case EventSuffix.LEECH:
                    Amount = HexaToInt(tokens[12]);
                    Power = (PowerType)HexaToInt(tokens[13]);
                    ExtraAmount = HexaToInt(tokens[14]);
                    break;

                case EventSuffix.DISPEL_FAILED:
                case EventSuffix.INTERRUPT:
                    ExtraSpellCast = new Spell(HexaToInt(tokens[12]), tokens[13], HexaToInt(tokens[14]));
                    break;

                case EventSuffix.EXTRA_ATTACKS:
                    Amount = int.Parse(tokens[12]);
                    break;

                case EventSuffix.AURA_BROKEN_SPELL:
                case EventSuffix.DISPEL:
                case EventSuffix.STOLEN:
                    ExtraSpellCast = new Spell(HexaToInt(tokens[12]), tokens[13], HexaToInt(tokens[14]));
                    Enum.TryParse(tokens[15], out tmpAura);
                    Aura = tmpAura;
                    break;

                case EventSuffix.AURA_APPLIED:
                case EventSuffix.AURA_BROKEN:
                case EventSuffix.AURA_REFRESH:
                case EventSuffix.AURA_REMOVED:
                    Enum.TryParse(tokens[12], out tmpAura);
                    Aura = tmpAura;
                    break;

                case EventSuffix.AURA_APPLIED_DOSE:
                case EventSuffix.AURA_REMOVED_DOSE:
                    Enum.TryParse(tokens[12], out tmpAura);
                    Aura = tmpAura;
                    Amount = HexaToInt(tokens[13]);
                    break;
            }
        }

        public enum EventPrefix
        {
            SWING,
            RANGE,
            SPELL,
            SPELL_PERIODIC,
            SPELL_BUILDING,
            ENVIRONMENTAL,
            ENCHANT,
            PARTY,
            UNIT
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
            RESURRECT,
            KILL,
            DIED,
            DESTROYED,
            APPLIED,
            REMOVED
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
    }
}
