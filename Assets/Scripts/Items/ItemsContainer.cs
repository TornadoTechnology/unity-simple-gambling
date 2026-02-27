using System.Collections.Generic;
using Data.Items;
using UnityEngine;
using Utilities.Behaviour;
using Utilities.Items;

namespace Items
{
    public sealed class ItemsContainer : MonoSingleton<ItemsContainer>
    {
        [SerializeField] private ItemTable _itemTable;
        [SerializeField] private List<ItemEntry> _items = new();

        public IReadOnlyList<ItemEntry> Items => _items;
        
        protected override void Awake()
        {
            base.Awake();
            
            _items = _itemTable.GetEntry();
        }

        public Item Pick() => RandomItem.Pick(_items).Item;
    }
}