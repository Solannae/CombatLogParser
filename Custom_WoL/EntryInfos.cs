namespace Custom_WoL
{
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

        public DamageInfo(int amount, int overkill, int school, int res,
                          int block, int abs, bool crit, bool glance, bool crush)
        {
            Amount = amount;
            Overkill = overkill;
            School = school;
            Resisted = res;
            Blocked = block;
            Absorbed = abs;
            Critical = crit;
            Glancing = glance;
            Crushing = crush;
        }
    }

    public class HealingInfo
    {
        public int Amount { get; set; }
        public int OverHealing { get; set; }
        public int Absorbed { get; set; }
        public bool Critical { get; set; }

        public HealingInfo(int amount, int over, int abs, bool crit)
        {
            Amount = amount;
            OverHealing = over;
            Absorbed = abs;
            Critical = crit;
        }
    }

    public class MissedInfo
    {
        public MissType TypeMiss { get; set; }
        public bool IsOffHand { get; set; }
        public int AmountMissed { get; set; }

        public MissedInfo(MissType type)
        {
            TypeMiss = type;
        }

        public MissedInfo(int amount)
        {
            AmountMissed = amount;
        }

        public MissedInfo(MissType type, int amount)
        {
            TypeMiss = type;
            AmountMissed = amount;
        }

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
    }
}
