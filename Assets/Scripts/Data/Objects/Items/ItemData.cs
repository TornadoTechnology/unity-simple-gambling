using System;
using System.Collections.Generic;

namespace Data.Objects.Items
{
    /// <summary>
    /// Represents item data used in loot/gacha systems.
    /// </summary>
    [Serializable]
    public sealed class ItemData
    {
        public string Name;
        
        /// <summary>
        /// Unique identifier of the item.
        /// </summary>
        public string Id;

        /// <summary>
        /// Path to the item sprite used for visual representation.
        /// </summary>
        public string SpritePath;

        /// <summary>
        /// Item weight used for probability calculations or balancing systems.
        /// Default value is 1.
        /// </summary>
        public float Weight = 1f;

        /// <summary>
        /// Dictionary of rank weights.
        /// Key represents rank/category name, value represents its weight affecting drop probability.
        /// </summary>
        public Dictionary<string, float> RankWeights = new();

        /// <summary>
        /// Pity counter — the number of spins required to guarantee obtaining this item.
        /// If the item is not obtained within this number of attempts, it is guaranteed to drop.
        /// Default value is 200 spins.
        /// </summary>
        public int Pity = 200;
    }
}