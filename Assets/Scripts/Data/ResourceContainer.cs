using System.Collections.Generic;
using System.Linq;
using Data.Loader;
using Data.Objects.Items;
using Data.Objects.Ranks;
using UnityEngine;

namespace Data
{
    public static class ResourceContainer
    {
        private static List<Item> _items;
        private static List<Rank> _ranks;
     
        public static IReadOnlyList<Item> Items => _items;
        public static IReadOnlyList<Rank> Ranks => _ranks;
        
        public static void Load()
        {
            _items = ItemLoader.Load();
            _ranks = RankLoader.Load();
        }

        public static List<ItemEntry> GetItemsAsEntries()
        {
            return Items.Select(item => new ItemEntry(item)).ToList();
        }
        
        public static Item PickItems()
        {
            return Items[Random.Range(0, Items.Count)];
        }
    }
}