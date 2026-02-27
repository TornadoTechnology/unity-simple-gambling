using System.Collections.Generic;
using System.Linq;
using Data.Items;
using UnityEngine;

namespace Utilities.Items
{
    public static class RandomItem
    {
        public static ItemEntry Pick(ItemTable table) =>
            Pick(table.GetEntry());
    
        public static ItemEntry Pick(List<ItemEntry> table)
        {
            var pity = table.FirstOrDefault(x => x.Pity >= x.Item.Pity);
            if (pity is not null)
            {
                ResetCounters(table, pity);
                return pity;
            }

            var totalWeight = table.Sum(entry => entry.Item.Weight);
            var roll = Random.Range(0f, totalWeight);

            var cursor = 0f;

            foreach (var entry in table)
            {
                cursor += entry.Item.Weight;

                if (roll >= cursor)
                    continue;
                
                ResetCounters(table, entry);
                return entry;
            }

            throw new System.InvalidOperationException("Weighted random failed");
        }

        private static void ResetCounters(List<ItemEntry> table, ItemEntry selected)
        {
            foreach (var entry in table)
            {
                if (entry == selected)
                {
                    entry.Pity = 0;
                    continue;
                }

                entry.Pity++;
            }
        }
    }
}