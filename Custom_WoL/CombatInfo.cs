using System;

namespace Custom_WoL
{
    public class CombatInfo
    {
        public long DamageDone { get; set; }
        public long DamageTaken { get; set; }
        public long HealingDone { get; set; }
        public long HealingTaken { get; set; }
        public bool IsDead { get; set; }
        public DateTime LastActive { get; set; }

        public CombatInfo()
        {
            IsDead = true;
        }

        public CombatInfo(long _dd, long _dt, long _hd, long _ht, bool _is_dead, DateTime _last_active)
        {
            DamageDone = _dd;
            DamageTaken = _dt;
            HealingDone = _hd;
            HealingTaken = _ht;
            IsDead = _is_dead;
            LastActive = _last_active;
        }

        public static CombatInfo operator +(CombatInfo a, CombatInfo b)
        {
            return new CombatInfo(a.DamageDone + b.DamageDone, a.DamageTaken + b.DamageTaken,
                                  a.HealingDone + b.HealingDone, a.HealingTaken + b.HealingTaken,
                                  b.IsDead, b.LastActive);
        }
    }
}
