using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Data.Items
{
    [CreateAssetMenu(fileName = "ItemTable", menuName = "Items/Table", order = 10)]
    public sealed class ItemTable : ScriptableObject
    {
        [SerializeField] private List<Item> _items;
        
        [PublicAPI]
        public IReadOnlyList<Item> Items => _items;

        public Item GetRandom() => _items[Random.Range(0, Items.Count)];
        public List<ItemEntry> GetEntry() => _items.Select(e => new ItemEntry(e)).ToList();
    }
}