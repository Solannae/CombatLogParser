using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    class CombatInfo
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
    }
}
