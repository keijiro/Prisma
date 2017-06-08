//
// Kvant/SprayMV - Particle system with motion vectors support
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SprayMV))]
    public class SprayMVEditor : Editor
    {
        SerializedProperty _emitterCenter;
        SerializedProperty _emitterSize;
        SerializedProperty _throttle;

        SerializedProperty _life;
        SerializedProperty _lifeRandomness;

        SerializedProperty _initialVelocity;
        SerializedProperty _directionSpread;
        SerializedProperty _speedRandomness;

        SerializedProperty _acceleration;
        SerializedProperty _drag;

        SerializedProperty _spin;
        SerializedProperty _speedToSpin;
        SerializedProperty _spinRandomness;

        SerializedProperty _noiseAmplitude;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;

        SerializedProperty _template;
        SerializedProperty _scale;
        SerializedProperty _scaleRandomness;
        SerializedProperty _randomSeed;

        static GUIContent _textCenter    = new GUIContent("Center");
        static GUIContent _textSize      = new GUIContent("Size");
        static GUIContent _textMotion    = new GUIContent("Motion");
        static GUIContent _textAmplitude = new GUIContent("Amplitude");
        static GUIContent _textFrequency = new GUIContent("Frequency");

        void OnEnable()
        {
            _emitterCenter = serializedObject.FindProperty("_emitterCenter");
            _emitterSize   = serializedObject.FindProperty("_emitterSize");
            _throttle      = serializedObject.FindProperty("_throttle");

            _life           = serializedObject.FindProperty("_life");
            _lifeRandomness = serializedObject.FindProperty("_lifeRandomness");

            _initialVelocity = serializedObject.FindProperty("_initialVelocity");
            _directionSpread = serializedObject.FindProperty("_directionSpread");
            _speedRandomness = serializedObject.FindProperty("_speedRandomness");

            _acceleration = serializedObject.FindProperty("_acceleration");
            _drag         = serializedObject.FindProperty("_drag");

            _spin           = serializedObject.FindProperty("_spin");
            _speedToSpin    = serializedObject.FindProperty("_speedToSpin");
            _spinRandomness = serializedObject.FindProperty("_spinRandomness");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion    = serializedObject.FindProperty("_noiseMotion");

            _template        = serializedObject.FindProperty("_template");
            _scale           = serializedObject.FindProperty("_scale");
            _scaleRandomness = serializedObject.FindProperty("_scaleRandomness");
            _randomSeed      = serializedObject.FindProperty("_randomSeed");
        }

        public override void OnInspectorGUI()
        {
            var targetSpray = target as SprayMV;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_template);

            if (EditorGUI.EndChangeCheck())
                targetSpray.RequestReconfigurationFromEditor();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_scale);
            EditorGUILayout.PropertyField(_scaleRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_randomSeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Emitter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_emitterCenter, _textCenter);
            EditorGUILayout.PropertyField(_emitterSize, _textSize);
            EditorGUILayout.PropertyField(_throttle);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_life);
            EditorGUILayout.PropertyField(_lifeRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Velocity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_initialVelocity);
            EditorGUILayout.PropertyField(_directionSpread);
            EditorGUILayout.PropertyField(_speedRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_acceleration);
            EditorGUILayout.PropertyField(_drag);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spin);
            EditorGUILayout.PropertyField(_speedToSpin);
            EditorGUILayout.PropertyField(_spinRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Turbulent Noise", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_noiseAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_noiseMotion, _textMotion);

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
