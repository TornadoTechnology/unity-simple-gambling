using System.Collections.Generic;
using UnityEngine;

namespace Data.Objects.Items
{
    public sealed class Item
    {
        public string Name;
        
        /// <summary>
        /// Unique identifier of the item.
        /// Used for referencing and saving item data.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Visual representation of the item.
        /// Used in UI and inventory display.
        /// </summary>
        public Sprite Sprite { get; }

        /// <summary>
        /// Item weight used for probability calculations or balancing systems.
        /// Default value is 1.
        /// </summary>
        public float Weight { get; }

        /// <summary>
        /// Dictionary of rank weights.
        /// Key represents rank/category name, value represents its weight affecting drop probability.
        /// </summary>
        public Dictionary<string, float> RankWeights { get; }

        /// <summary>
        /// Pity counter — the number of spins required to guarantee obtaining this item.
        /// If the item is not obtained within this number of attempts, it is guaranteed to drop.
        /// Default value is 200 spins.
        /// </summary>
        public int Pity { get; }

        public Item(string name, string id, Sprite sprite, float weight = 1f, Dictionary<string, float> rankWeights = null, int pity = 200)
        {
            Name = name;
            Id = id;
            Sprite = sprite;
            Weight = weight;
            RankWeights = rankWeights ?? new Dictionary<string, float>();
            Pity = pity;
        }
    }
}
