using System;
using System.Collections.Generic;
using System.Linq;
using Data.Objects.Items;
using Data.Objects.Ranks;
using JetBrains.Annotations;

namespace Data
{
    [PublicAPI]
    public static class ResourceGameContainer
    {
        private static readonly List<ItemEntry> Table = new();

        public static void Load()
        {
            Table.Clear();
            Table.AddRange(ResourceContainer.GetItemsAsEntries());
        }
        
        [CanBeNull]
        public static Rank GetRank(int count) => ResourceContainer.Ranks.First(e => count >= e.Min && count <= e.Max);

        public static Item PickItem(Rank rank) => PickItem(Table, rank);
        
        public static Item PickItem(IReadOnlyList<ItemEntry> table, [CanBeNull] Rank rank)
        {
            if (table.Count == 0)
                throw new ArgumentException("Loot table is empty.");

            // Pity check
            var pityEntry = table.FirstOrDefault(entry => entry.Pity >= entry.Item.Pity);
            if (pityEntry is not null)
            {
                UpdatePityCounters(table, pityEntry);
                return pityEntry.Item;
            }

            // Weight selection (rank weight or default weight)
            var weightedEntries = table
                .Select(entry => new
                {
                    Entry = entry,
                    Weight = rank is null
                            ? entry.Item.Weight
                            : entry.Item.RankWeights.TryGetValue(rank.Id, out var rankWeight)
                                ? rankWeight
                                : entry.Item.Weight
                }).ToList();

            var totalWeight = weightedEntries.Sum(x => x.Weight);
            if (totalWeight <= 0f)
                throw new InvalidOperationException("Total weight must be greater than zero.");

            var roll = UnityEngine.Random.Range(0f, totalWeight);
            var current = 0f;

            foreach (var item in weightedEntries)
            {
                current += item.Weight;

                if (roll >= current)
                    continue;
                
                UpdatePityCounters(table, item.Entry);
                return item.Entry.Item;
            }

            throw new InvalidOperationException("Weighted random selection failed.");
        }

        private static void UpdatePityCounters(IReadOnlyList<ItemEntry> table, ItemEntry selected)
        {
            foreach (var entry in table)
            {
                entry.Pity = entry == selected ? 0 : entry.Pity + 1;
            }
        }
    }
}