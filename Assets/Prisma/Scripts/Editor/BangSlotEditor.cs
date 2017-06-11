using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BangSlot))]
    public class BangSlotEditor : Editor
    {
        SerializedProperty _bangEvent;

        void OnEnable()
        {
            _bangEvent = serializedObject.FindProperty("_bangEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_bangEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
