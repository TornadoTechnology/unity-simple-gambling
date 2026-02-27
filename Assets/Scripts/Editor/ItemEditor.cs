using Data.Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Item))]
    public sealed class ItemEditor : UnityEditor.Editor
    {
        private const float PreviewSize = 100f;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var item = (Item) target;
            if (!item.Sprite)
                return;

            GUILayout.Space(10);
            GUILayout.Label("Sprite Preview", EditorStyles.boldLabel);

            var texture = item.Sprite.texture;
            var rect = GUILayoutUtility.GetRect(PreviewSize, PreviewSize, GUILayout.ExpandWidth(false));

            if (!texture)
                return;
            
            var spriteRect = item.Sprite.textureRect;
            var uv = new Rect(
                spriteRect.x / texture.width,
                spriteRect.y / texture.height,
                spriteRect.width / texture.width,
                spriteRect.height / texture.height
            );

            GUI.DrawTextureWithTexCoords(rect, texture, uv, true);
        }
    }
}