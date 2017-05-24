//
// KlakUI - Custom UI controls for Klak
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
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Klak.UI
{
    [CustomEditor(typeof(Knob))]
    [CanEditMultipleObjects]
    public class KnobEditor : SelectableEditor
    {
        SerializedProperty _minValue;
        SerializedProperty _maxValue;
        SerializedProperty _value;
        SerializedProperty _graphic;
        SerializedProperty _target;

        protected override void OnEnable()
        {
            base.OnEnable();
            _minValue = serializedObject.FindProperty("_minValue");
            _maxValue = serializedObject.FindProperty("_maxValue");
            _value = serializedObject.FindProperty("_value");
            _graphic = serializedObject.FindProperty("_graphic");
            _target = serializedObject.FindProperty("_target");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_minValue);
            EditorGUILayout.PropertyField(_maxValue);
            EditorGUILayout.Slider(_value, _minValue.floatValue, _maxValue.floatValue);
            EditorGUILayout.PropertyField(_graphic);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_target);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
