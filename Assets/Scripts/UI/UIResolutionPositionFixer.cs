using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIResolutionPositionFixer : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Canvas canvas;

        [Header("Reference Resolution (where you placed the UI)")]
        [SerializeField] private Vector2 referenceResolution = new Vector2(1024, 768);

        [Header("Position on reference resolution")]
        [SerializeField] private Vector2 referencePosition;

        private Vector2 lastCanvasSize;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        private void Start()
        {
            ApplyPosition();
            lastCanvasSize = GetCanvasSize();
        }

        private void Update()
        {
            Vector2 currentCanvasSize = GetCanvasSize();

            if (currentCanvasSize != lastCanvasSize)
            {
                ApplyPosition();
                lastCanvasSize = currentCanvasSize;
            }
        }

        [ContextMenu("Capture Current Position")]
        public void CaptureCurrentPosition()
        {
            referencePosition = rectTransform.anchoredPosition;
        }

        public void ApplyPosition()
        {
            Vector2 currentResolution = GetCanvasSize();

            float scaleX = currentResolution.x / referenceResolution.x;
            float scaleY = currentResolution.y / referenceResolution.y;

            rectTransform.anchoredPosition = new Vector2(
                referencePosition.x * scaleX,
                referencePosition.y * scaleY
            );
        }

        private Vector2 GetCanvasSize()
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            return canvasRect.rect.size;
        }
    }
}