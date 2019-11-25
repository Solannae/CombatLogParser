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

        public CombatInfo(long dd, long dt, long hd, long ht, bool isDead, DateTime lastActive)
        {
            DamageDone = dd;
            DamageTaken = dt;
            HealingDone = hd;
            HealingTaken = ht;
            IsDead = isDead;
            LastActive = lastActive;
        }

        public static CombatInfo operator +(CombatInfo a, CombatInfo b)
        {
            return new CombatInfo(a.DamageDone + b.DamageDone, a.DamageTaken + b.DamageTaken,
                                  a.HealingDone + b.HealingDone, a.HealingTaken + b.HealingTaken,
                                  b.IsDead, b.LastActive);
        }
    }
}
