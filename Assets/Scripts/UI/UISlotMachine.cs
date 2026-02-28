using System;
using System.Collections.Generic;
using System.Linq;
using Data.Items;
using Items;
using JetBrains.Annotations;
using UnityEngine;
using Utilities;

using Random = UnityEngine.Random;

// I hate mono
namespace UI
{
    public sealed partial class UISlotMachine : MonoBehaviour
    {
        #region Events

        public event Action OnSpinStart;
        public event Action OnSpinEnd;
        public event Action OnColumnStart;
        public event Action OnColumnStop;

        #endregion

        #region Fields (Configuration)
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _itemPrefab;

        [Header("Container")]
        [SerializeField] private RectTransform[] _containers;
        [SerializeField] private int _containerItems = 20;
  
        [Header("Animation")]
        [SerializeField] private AnimationCurve _decelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _spinDuration = 2f;
        [SerializeField] private float _spins = 10f;
        [SerializeField] private float _delay = 5f;
  
        [field: Header("State (Do not change)")]
        [field: SerializeField] public bool Initialized { get; private set; }
        [field: SerializeField] public bool Spun { get; private set; }
        [field: SerializeField] public Dictionary<RectTransform, bool> ContainerSpinning { get; private set; } = new(); // ONLY FOR DEBUG DON'T CHANGE

        #endregion
        
        [PublicAPI]
        public bool SpunAny => ContainerSpinning.Values.Any(e => e);
        public bool SpunAll => ContainerSpinning.Values.All(e => e);

        private void Awake()
        {
            // TODO: Separate module
            Initialize(ItemsContainer.Instance.Items);
        }
        
        public void Initialize(IReadOnlyList<ItemEntry> entries)
        {
            if (Initialized)
                return;
            
            SetupContainers(entries);
            Initialized = true;
        }

        public void Spin(Item result)
        {
            if (!Initialized)
            {
                FastLog.Error("Failed to spin, not initialized", this);
                return;
            }
            
            if (SpunAny)
            {
                FastLog.Error("Failed to spin", this);
                return;
            }
            
            SpinRoutine(result);
        }
        
        private IReadOnlyDictionary<RectTransform, SpinData> PrepareSpin(Item selected)
        {
            var result = new Dictionary<RectTransform, SpinData>();
            foreach (var container in _containers)
            {
                var items = _itemInstances[container];
                var itemsResulted = items.Where(e => e.Item == selected).ToList();

                var item = itemsResulted[Random.Range(0, itemsResulted.Count)];
                var itemIndex = items.IndexOf(item);

                var target = 0f;
                for (var i = 0; i < itemIndex; i++)
                    target += items[i].Height;

                var totalHeight = container.sizeDelta.y;
                var start = container.anchoredPosition.y;
                var distance = totalHeight * _spins + Mathf.Repeat(target - start + totalHeight, totalHeight);

                var end = start + distance;

                result[container] = new SpinData(start, target, end, totalHeight);
            }

            return result;
        }
        
        [Serializable]
        private readonly struct SpinData
        {
            public readonly float Start;
            public readonly float Target;
            public readonly float End;
            public readonly float Height;

            public SpinData(float start, float target, float end, float height)
            {
                Start = start;
                Target = target;
                End = end;
                Height = height;
            }
        }
    }
}