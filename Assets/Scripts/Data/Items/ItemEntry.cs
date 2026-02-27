using System;

namespace Data.Items
{
    [Serializable]
    public sealed class ItemEntry
    {
        public Item Item;
        public int Pity;

        public ItemEntry(Item item, int pity = 0)
        {
            Item = item;
            Pity = pity;
        }
    }
}