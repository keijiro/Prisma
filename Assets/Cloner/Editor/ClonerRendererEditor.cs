// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using UnityEngine;
using UnityEditor;

namespace Cloner
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ClonerRenderer))]
    public sealed class ClonerRendererEditor : Editor
    {
        SerializedProperty _pointSource;

        SerializedProperty _template;
        SerializedProperty _templateScale;
        SerializedProperty _scaleByNoise;

        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;
        SerializedProperty _normalModifier;

        SerializedProperty _material;
        SerializedProperty _gradient;

        SerializedProperty _randomSeed;

        static class Labels
        {
            public static GUIContent frequency = new GUIContent("Frequency");
            public static GUIContent motion = new GUIContent("Motion");
            public static GUIContent scale = new GUIContent("Scale");
        }

        void OnEnable()
        {
            _pointSource = serializedObject.FindProperty("_pointSource");

            _template = serializedObject.FindProperty("_template");
            _templateScale = serializedObject.FindProperty("_templateScale");
            _scaleByNoise = serializedObject.FindProperty("_scaleByNoise");

            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion = serializedObject.FindProperty("_noiseMotion");
            _normalModifier = serializedObject.FindProperty("_normalModifier");

            _material = serializedObject.FindProperty("_material");
            _gradient = serializedObject.FindProperty("_gradient");

            _randomSeed = serializedObject.FindProperty("_randomSeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_pointSource);

            EditorGUILayout.PropertyField(_template);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_templateScale, Labels.scale);
            EditorGUILayout.PropertyField(_scaleByNoise);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Noise Field");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_noiseFrequency, Labels.frequency);
            EditorGUILayout.PropertyField(_noiseMotion, Labels.motion);
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(_normalModifier);

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_gradient);
            EditorGUILayout.PropertyField(_randomSeed);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
