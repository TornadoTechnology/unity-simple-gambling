using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Objects.Items;
using UnityEngine;

namespace Data.Loader
{
    public static class ItemLoader
    {
        public static List<Item> Load()
        {
            var dataList = LoadItemData();
            return CreateItems(dataList);
        }

        private static ItemDataList LoadItemData()
        {
            var path = GetStreamingPath("items.json");
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<ItemDataList>(json);
        }

        private static List<Item> CreateItems(ItemDataList dataList) =>
            dataList.Items.Select(CreateItem).ToList();

        private static Item CreateItem(ItemData data)
        {
            var sprite = LoadSprite(data.SpritePath);

            return new Item(
                data.Name,
                data.Id,
                sprite,
                data.Weight,
                data.RankWeights,
                data.Pity
            );
        }

        private static Sprite LoadSprite(string relativePath)
        {
            var path = GetStreamingPath(relativePath);
            var bytes = File.ReadAllBytes(path);

            var texture = LoadTexture(bytes);

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            
            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                Vector2.one * 0.5f,
                64
            );

            return sprite;
        }

        private static Texture2D LoadTexture(byte[] bytes)
        {
            var texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);
            return texture;
        }

        private static string GetStreamingPath(string relativePath) => Path.Combine(Root.Path, relativePath);
    }
}