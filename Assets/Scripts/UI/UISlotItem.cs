using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class UISlotItem : MonoBehaviour
    {
        [field: Header("Links")]
        [field: SerializeField] public RectTransform Transform { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        
        [Header("Configuration")]
        [SerializeField] private Vector2 _offset = Vector2.zero;
        
        public float Height => Transform.rect.height + _offset.y;
    }
}