using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    class Encounter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsOver { get; set; }

        public Dictionary<Entity, CombatInfo> Players { get; set; }
        public Dictionary<Entity, CombatInfo> NPC { get; set; }
        public Dictionary<Entity, CombatInfo> Pets { get; set; }
        public Dictionary<Entity, CombatInfo> Summons { get; set; }

        public Encounter(Queue<Entry> entries)
        {
            Players = new Dictionary<Entity, CombatInfo>();
            NPC = new Dictionary<Entity, CombatInfo>();
            Pets = new Dictionary<Entity, CombatInfo>();
            Summons = new Dictionary<Entity, CombatInfo>();
            Fill(entries);
        }

        public CombatInfo AddEntity(Entity entity)
        {
            if (entity.Guid != 0)
            {
                if (entity.Guid.IsPlayer())
                { 
                    if (!Players.ContainsKey(entity))
                        Players.Add(entity, new CombatInfo());
                    return Players[entity];
                }
                else if (entity.Guid.IsCreatureOrVehicle())
                {
                    if (!NPC.ContainsKey(entity))
                        NPC.Add(entity, new CombatInfo());
                    return NPC[entity];
                }
                else if (entity.Guid.IsPet())
                {
                    if (!Pets.ContainsKey(entity))
                        Pets.Add(entity, new CombatInfo());
                    return Pets[entity];
                }
            }

            return null;
        }

        public void CheckCombatEnd(bool is_player, DateTime curr_time)
        {
            TimeSpan five_secs = new TimeSpan(0, 0, 5);

            if (is_player)
            {
                if (Players.All(u => u.Value.IsDead == true))
                    IsOver = true;
            }
            else
            {
                foreach (var npc in NPC.Where(u => u.Value.IsDead == false))
                {
                    if (curr_time - npc.Value.LastActive > five_secs)
                        npc.Value.IsDead = true;
                }

                if (NPC.All(u => u.Value.IsDead == true))
                {
                    IsOver = true;
                    End = curr_time;
                }
            }
        }

        public void AddEntry(Entry entry)
        {
            CombatInfo source = AddEntity(entry.SourceEntity);
            CombatInfo dest = AddEntity(entry.DestEntity);

            switch (entry.Suffix)
            {
                case Entry.EventSuffix.DAMAGE:
                    if (source != null)
                    {
                        source.DamageDone += entry.Damage.Amount + entry.Damage.Overkill;
                        if (source.IsDead)
                            source.IsDead = false;
                        source.LastActive = entry.Timestamp;
                    }
                    dest.DamageTaken += entry.Damage.Amount;
                    dest.LastActive = entry.Timestamp;
                    break;
                case Entry.EventSuffix.HEAL:
                    //Do not take into account Overhealing and Absorbs for now
                    source.HealingDone += entry.Healing.Amount;
                    if (source.IsDead)
                        source.IsDead = false;
                    source.LastActive = entry.Timestamp;
                    dest.HealingTaken += entry.Healing.Amount;
                    dest.LastActive = entry.Timestamp;
                    break;
                //Flag totems and apparitions (and more...) as summons
                case Entry.EventSuffix.SUMMON:
                    entry.DestEntity.IsSummoned = true;
                    break;
                case Entry.EventSuffix.DIED:
                case Entry.EventSuffix.DESTROYED:
                    dest.IsDead = true;
                    CheckCombatEnd(entry.DestEntity.Guid.IsPlayer(), entry.Timestamp);
                    break;
            }
        }

        public void Fill(Queue<Entry> entries)
        {
            Start = entries.First().Timestamp;
            Entry current;

            while (!IsOver && entries.Count > 0)
            {
                current = entries.Dequeue();
                AddEntry(current);
            }
        }
    }
}
