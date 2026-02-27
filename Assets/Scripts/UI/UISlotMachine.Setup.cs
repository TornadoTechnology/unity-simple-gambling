using System;
using System.Collections.Generic;
using Data.Items;
using UnityEngine;

using Random = UnityEngine.Random;

namespace UI
{
    public partial class UISlotMachine
    {
        private readonly Dictionary<RectTransform, List<UISlotItem>> _itemInstances = new();
        
        private void SetupContainers(IReadOnlyList<ItemEntry> entries)
        {
            foreach (var container in _containers)
            {
                _itemInstances[container] = SetupContainer(container, entries, out var height);
                
                container.anchorMin = new Vector2(0.5f, 0.5f);
                container.anchorMax = new Vector2(0.5f, 0.5f);
                container.pivot     = new Vector2(0.5f, 0.5f);
                container.sizeDelta = new Vector2(container.sizeDelta.x, height);
            }
        }
        
        private List<UISlotItem> SetupContainer(RectTransform container, IReadOnlyList<ItemEntry> entries, out float height)
        {
            height = 0f;
            
            var result = new List<UISlotItem>();
            var count = 0;

            var entriesCopy = new List<ItemEntry>(entries);
            Shuffle(entriesCopy);
            
            // Add all type items
            foreach (var entry in entriesCopy)
            {
                CreatePrefab(entry, ref height);
                count++;
            }
            
            for (; count < _containerItems; count++)
            {
                CreatePrefab(entries[Random.Range(0, entries.Count)], ref height);
            }

            return result;

            void CreatePrefab(ItemEntry entry, ref float height)
            {
                var itemPrefab = SetupItemPrefab(container);

                itemPrefab.Init(entry.Item);
                
                itemPrefab.Image.rectTransform.anchoredPosition = Vector2.down * count * itemPrefab.Height;
                itemPrefab.Image.sprite = entry.Item.Sprite;
                
                height += itemPrefab.Height;
                result.Add(itemPrefab);
            }
        }
        
        private UISlotItem SetupItemPrefab(RectTransform content)
        {
            var instance = Instantiate(_itemPrefab, content);
            return instance.TryGetComponent<UISlotItem>(out var component)
                ? component
                : throw new InvalidOperationException($"Excepted {nameof(UISlotItem)} on {nameof(_itemPrefab)} in {nameof(UISlotMachine)}");
        }
        
        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}