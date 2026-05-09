using System;

namespace Data.Objects.Items
{
    [Serializable]
    public sealed class ItemEntry
    {
        public Item Item;
        public int Pity;

        public ItemEntry(Item item)
        {
            Item = item;
            Pity = item.Pity;
        }
    }
}