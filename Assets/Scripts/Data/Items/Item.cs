using System;
using UnityEngine;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Data.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
    public sealed class Item : ScriptableObject, IEquatable<Item>
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public float Weight  { get; private set; } = 1f;
        [field: SerializeField] public int Pity  { get; private set; } = 200;
        
        public bool Equals(Item other)
        {
            if (other is null)
                return false;
            
            if (ReferenceEquals(this, other))
                return true;
            
            return Equals(Sprite, other.Sprite) &&
                   Weight.Equals(other.Weight) &&
                   Pity == other.Pity;
        }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is Item other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), Sprite, Weight, Pity);
        
        public static bool operator ==(Item left, Item right) => 
            Equals(left, right);

        public static bool operator !=(Item left, Item right) => 
            !(left == right);
    }
}