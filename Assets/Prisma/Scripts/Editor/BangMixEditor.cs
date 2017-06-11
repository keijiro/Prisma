using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BangMix))]
    public class BangMixEditor : Editor
    {
        SerializedProperty _modulationType;
        SerializedProperty _onEvent;
        SerializedProperty _offEvent;

        void OnEnable()
        {
            _modulationType = serializedObject.FindProperty("_modulationType");
            _onEvent = serializedObject.FindProperty("_onEvent");
            _offEvent = serializedObject.FindProperty("_offEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_modulationType);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_onEvent);
            EditorGUILayout.PropertyField(_offEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
