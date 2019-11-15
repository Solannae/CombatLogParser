using System.Collections.Generic;

namespace Custom_WoL.Libs
{
    public class ArrayManipulation
    {
        public static void MergeEncounterDictionaries(Dictionary<Entity, CombatInfo> dicA, Dictionary<Entity, CombatInfo> dicB)
        {
            foreach (var item in dicB)
            {
                if (!dicA.ContainsKey(item.Key))
                    dicA.Add(item.Key, item.Value);
                else
                    dicA[item.Key] += item.Value;
            }
        }
    }
}
