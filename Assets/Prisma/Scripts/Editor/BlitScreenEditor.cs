using UnityEngine;
using UnityEditor;

namespace Prisma
{
    [CustomEditor(typeof(BlitScreen)), CanEditMultipleObjects]
    public class CrawlingSwarmEditor : Editor
    {
        SerializedProperty _sourceTexture;
        SerializedProperty _tintColor;
        SerializedProperty _scale;
        SerializedProperty _offset;
        SerializedProperty _ditherAmount;

        void OnEnable()
        {
            _sourceTexture = serializedObject.FindProperty("_sourceTexture");
            _tintColor = serializedObject.FindProperty("_tintColor");
            _scale = serializedObject.FindProperty("_scale");
            _offset = serializedObject.FindProperty("_offset");
            _ditherAmount = serializedObject.FindProperty("_ditherAmount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_sourceTexture);
            EditorGUILayout.PropertyField(_tintColor);
            EditorGUILayout.PropertyField(_scale);
            EditorGUILayout.PropertyField(_offset);
            EditorGUILayout.PropertyField(_ditherAmount);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
