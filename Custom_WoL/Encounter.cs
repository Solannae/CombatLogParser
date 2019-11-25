using System;
using System.Collections.Generic;
using System.Linq;

namespace Custom_WoL
{
    public class Encounter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime? LastDamage { get; set; }
        public bool IsOver { get; set; }
        public TimeSpan Inactivity { get; set; }
        public TimeSpan DmgInactivity { get; set; }

        public Dictionary<Entity, CombatInfo> Players { get; set; }
        public Dictionary<Entity, CombatInfo> NPC { get; set; }
        public Dictionary<Entity, CombatInfo> Pets { get; set; }

        public Encounter(Queue<Entry> entries)
        {
            Players = new Dictionary<Entity, CombatInfo>();
            NPC = new Dictionary<Entity, CombatInfo>();
            Pets = new Dictionary<Entity, CombatInfo>();
            LastDamage = null;
            Inactivity = new TimeSpan(0, 0, 0, 5);
            DmgInactivity = new TimeSpan(0, 0, 0, 10);
            Fill(entries);
        }

        public CombatInfo AddEntity(Entity entity)
        {
            if (entity.Guid != 0)
            {
                Dictionary<Entity, CombatInfo> correspondingContainer;

                if (entity.Guid.IsPlayer())
                    correspondingContainer = Players;
                else if (entity.Guid.IsCreatureOrVehicle())
                    correspondingContainer = NPC;
                else if (entity.Guid.IsPet())
                    correspondingContainer = Pets;
                else
                    return null;

                if (!correspondingContainer.ContainsKey(entity))
                    correspondingContainer.Add(entity, new CombatInfo());
                return correspondingContainer[entity];
            }

            return null;
        }

        public void CheckCombatEnd(bool isPlayer, DateTime currTime)
        {
            var data = (isPlayer) ? Players : NPC;

            //Inactivity timer
            foreach (var entity in data.Where(u => u.Value.IsDead == false))
            {
                if (currTime - entity.Value.LastActive > Inactivity)
                    entity.Value.IsDead = true;
            }

            if (currTime - LastDamage > DmgInactivity || data.All(u => u.Value.IsDead == true))
            {
                IsOver = true;
                End = LastDamage ?? currTime;
            }
        }

        public void AddEntry(Entry entry)
        {
            var source = AddEntity(entry.SourceEntity);
            var dest = AddEntity(entry.DestEntity);

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
                    
                    if (!LastDamage.HasValue)
                        Start = entry.Timestamp;
                    
                    LastDamage = entry.Timestamp;
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

            do
            {
                if (LastDamage.HasValue && entries.First().Timestamp - LastDamage > DmgInactivity)
                    IsOver = true;
                else
                {
                    var current = entries.Dequeue();
                    AddEntry(current);
                }
            } while (!IsOver && entries.Count > 0);

            if (entries.Count == 0 && !IsOver)
                IsOver = true;
        }
    }
}
