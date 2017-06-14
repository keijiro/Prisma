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
        SerializedProperty _scaleByPulse;

        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;
        SerializedProperty _normalModifier;

        SerializedProperty _pulseProbability;
        SerializedProperty _pulseFrequency;

        SerializedProperty _material;
        SerializedProperty _gradient;

        SerializedProperty _bounds;
        SerializedProperty _randomSeed;

        static class Labels
        {
            public static GUIContent frequency = new GUIContent("Frequency");
            public static GUIContent motion = new GUIContent("Motion");
            public static GUIContent probability = new GUIContent("Probability");
            public static GUIContent scale = new GUIContent("Scale");
        }

        void OnEnable()
        {
            _pointSource = serializedObject.FindProperty("_pointSource");

            _template = serializedObject.FindProperty("_template");
            _templateScale = serializedObject.FindProperty("_templateScale");
            _scaleByNoise = serializedObject.FindProperty("_scaleByNoise");
            _scaleByPulse = serializedObject.FindProperty("_scaleByPulse");

            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion = serializedObject.FindProperty("_noiseMotion");
            _normalModifier = serializedObject.FindProperty("_normalModifier");

            _pulseProbability = serializedObject.FindProperty("_pulseProbability");
            _pulseFrequency = serializedObject.FindProperty("_pulseFrequency");

            _material = serializedObject.FindProperty("_material");
            _gradient = serializedObject.FindProperty("_gradient");

            _bounds = serializedObject.FindProperty("_bounds");
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
            EditorGUILayout.PropertyField(_scaleByPulse);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Noise Field");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_noiseFrequency, Labels.frequency);
            EditorGUILayout.PropertyField(_noiseMotion, Labels.motion);
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(_normalModifier);

            EditorGUILayout.LabelField("Pulse Noise");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_pulseProbability, Labels.probability);
            EditorGUILayout.PropertyField(_pulseFrequency, Labels.frequency);
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_gradient);

            EditorGUILayout.PropertyField(_bounds);
            EditorGUILayout.PropertyField(_randomSeed);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
