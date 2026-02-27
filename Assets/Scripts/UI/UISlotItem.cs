using Data.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class UISlotItem : MonoBehaviour
    {
        [field: Header("Configuration")]
        [field: SerializeField] public RectTransform Transform { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }

        [field: Header("Setup (Don't change)")]
        [field: SerializeField] public bool Setup { get; private set; }
        [field: SerializeField] public Item Item { get; private set; }
        
        public float Height => Transform.rect.height + Offset.y;

        public void Init(Item item)
        {
            if (Setup)
                return;
            
            Item = item;
            Setup = true;
        }
    }
}