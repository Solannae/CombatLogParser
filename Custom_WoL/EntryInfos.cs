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
        public MissType TypeMiss { get; set; }
        public bool IsOffHand { get; set; }
        public int AmountMissed { get; set; }

        public MissedInfo(MissType _type)
        {
            TypeMiss = _type;
        }

        public MissedInfo(int _amount)
        {
            AmountMissed = _amount;
        }

        public MissedInfo(MissType _type, int _amount)
        {
            TypeMiss = _type;
            AmountMissed = _amount;
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
