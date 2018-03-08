﻿using System;
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
        public Dictionary<Entity, CombatInfo> Enemies { get; set; }
        public Dictionary<Entity, CombatInfo> Pets { get; set; }

        public Encounter(Queue<Entry> entries)
        {
            Players = new Dictionary<Entity, CombatInfo>();
            Enemies = new Dictionary<Entity, CombatInfo>();
            Pets = new Dictionary<Entity, CombatInfo>();
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
                    if (!Enemies.ContainsKey(entity))
                        Enemies.Add(entity, new CombatInfo());
                    return Enemies[entity];
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

        public void CheckCombatEnd(bool is_player)
        {
            if (is_player)
            {
                if (Players.All(u => u.Value.IsDead == true))
                    IsOver = true;
            }
            else
            {
                if (Enemies.All(u => u.Value.IsDead == true))
                    IsOver = true;
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
                        source.DamageDone += entry.Damage.Amount + entry.Damage.Overkill;
                    dest.DamageTaken += entry.Damage.Amount;
                    break;
                case Entry.EventSuffix.HEAL:
                    //Do not take into account Overhealing and Absorbs for now
                    source.HealingDone += entry.Healing.Amount;
                    dest.HealingTaken += entry.Healing.Amount;
                    break;
                case Entry.EventSuffix.DIED:
                    dest.IsDead = true;
                    CheckCombatEnd(entry.DestEntity.Guid.IsPlayer());
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
