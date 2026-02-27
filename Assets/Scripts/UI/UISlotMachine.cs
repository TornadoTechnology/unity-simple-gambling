using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// I hate mono
namespace UI
{
    public sealed class UISlotMachine : MonoBehaviour
    {
        [Header("References")]
        
        [SerializeField] private RectTransform[] _slotContents; // 3 Content
        [SerializeField] private GameObject _itemPrefab;
        
        [Header("Config")]
        
        [SerializeField] private List<Sprite> _possibleItems;
        [SerializeField] private float _spinDuration = 2f;
        [SerializeField] private float _spins = 10f;
        [SerializeField] private float _delay = 5f;
        [SerializeField] private int _itemsInColumn = 20;
        
        [FormerlySerializedAs("_decelerationCurveф")]
        [Header("Animation")]
        [SerializeField] private AnimationCurve _decelerationCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [field: Header("Debug")]
        [field: SerializeField] public Sprite DebugForceSpinSprite { get; private set; }
        
        [Header("State (Do not change)")]
        [SerializeField] private bool _isSpinning;

        /**
         * Events
         */
        
        public event Action OnSpinStart;
        public event Action<int> OnColumnStop;
        public event Action OnSpinEnd;
        
        private void Awake()
        {
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            foreach (var content in _slotContents)
            {
                if (content.childCount > 0)
                    continue;

                for (var i = 0; i < _itemsInColumn; i++)
                    SetupItem(InstantiateItem(content), i);
                
                var totalHeight = 0f;
                foreach (Transform child in content)
                    totalHeight += child.GetComponent<UISlotItem>().Height;

                content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
                content.anchoredPosition = Vector2.zero;
            }
        }
        
        public void Spin(Sprite forcedItem = null)
        {
            if (_isSpinning)
                return;

            StartCoroutine(SpinRoutine(forcedItem));
        }
        
        public void ResetSlots()
        {
            StopAllCoroutines();
            _isSpinning = false;

            foreach (var content in _slotContents)
            {
                foreach (Transform child in content)
                    DestroyImmediate(child.gameObject);
                content.anchoredPosition = Vector2.zero;
            }
        }
        
        #region Item
        
        private UISlotItem InstantiateItem(RectTransform content)
        {
            var instance = Instantiate(_itemPrefab, content);
            return !instance.TryGetComponent<UISlotItem>(out var component)
                ? throw new InvalidOperationException($"Excepted {nameof(UISlotItem)} on {nameof(_itemPrefab)} in {nameof(UISlotMachine)}")
                : component;
        }

        private UISlotItem SetupItem(UISlotItem item, int index)
        {
            item.Image.rectTransform.anchoredPosition = new Vector2(0, -index * item.Transform.rect.height);
            item.Image.sprite = _possibleItems[Random.Range(0, _possibleItems.Count)];
            
            return item;
        }
        
        #endregion

        #region Animation (Spin)

        private int _stoppedColumns;

        private IEnumerator SpinRoutine(Sprite forcedItem)
        {
            if (_isSpinning) yield break;

            _isSpinning = true;
            _stoppedColumns = 0;

            OnSpinStart?.Invoke();

            for (var i = 0; i < _slotContents.Length; i++)
            {
                var resultIndex = !forcedItem
                    ? Random.Range(0, _itemsInColumn)
                    : _itemsInColumn - 1;
                
                StartCoroutine(SpinColumn(_slotContents[i], resultIndex, i * _delay, i));
            }

            // Ждем реально завершения всех колонок
            while (_stoppedColumns < _slotContents.Length)
                yield return null;

            _isSpinning = false;
            OnSpinEnd?.Invoke();
        }

        private IEnumerator SpinColumn(RectTransform content, int resultIndex, float delay, int columnIndex)
        {
            yield return new WaitForSeconds(delay);

            var totalHeight = content.sizeDelta.y;
            var firstItem = content.GetComponentInChildren<UISlotItem>();
            var itemHeight = firstItem.Height;

            float startY = content.anchoredPosition.y;
            float targetY = resultIndex * itemHeight;
            float spinDistance = totalHeight * _spins + Mathf.Repeat(targetY - startY + totalHeight, totalHeight);
            float endY = startY + spinDistance;

            float time = 0f;

            while (time < _spinDuration)
            {
                time += Time.deltaTime;
                float t = time / _spinDuration;

                // eased = 0..1, плавное замедление по кривой
                float eased = _decelerationCurve.Evaluate(t);

                float currentY = Mathf.Repeat(Mathf.Lerp(startY, endY, eased), totalHeight);
                content.anchoredPosition = new Vector2(0, currentY);

                yield return null;
            }

            content.anchoredPosition = new Vector2(0, targetY);

            OnColumnStop?.Invoke(columnIndex);
            _stoppedColumns++;
        }
        
        #endregion
    }
}