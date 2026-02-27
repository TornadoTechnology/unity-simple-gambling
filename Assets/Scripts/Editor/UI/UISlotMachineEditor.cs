using UI;
using UnityEditor;
using UnityEngine;

namespace Editor.UI
{
    [CustomEditor(typeof(UISlotMachine))]
    public sealed class UISlotMachineEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var slot = (UISlotMachine) target;

            GUILayout.Space(10);
            GUILayout.Label("Debug Controls", EditorStyles.boldLabel);

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("▶ Spin Random"))
                slot.Spin();

            if (GUILayout.Button("🎯 Spin Forced"))
                slot.Spin(slot.DebugForceSpinSprite);

            if (GUILayout.Button("🔄 Reset"))
                slot.ResetSlots();

            GUI.enabled = true;

            if (!Application.isPlaying)
                EditorGUILayout.HelpBox("Buttons work in Play Mode only.", MessageType.Info);
        }
    }
}