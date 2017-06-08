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
using System.IO;
using System.Collections.Generic;

namespace Kvant
{
    [CustomEditor(typeof(SprayMVTemplate))]
    public class SprayMVTemplateEditor : Editor
    {
        #region Editor functions

        SerializedProperty _shapes;
        SerializedProperty _maxInstanceCount;

        void OnEnable()
        {
            _shapes = serializedObject.FindProperty("_shapes");
            _maxInstanceCount = serializedObject.FindProperty("_maxInstanceCount");
        }

        public override void OnInspectorGUI()
        {
            var template = (SprayMVTemplate)target;

            // Editable properties
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_shapes, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_maxInstanceCount);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            // Readonly members
            EditorGUILayout.LabelField("Instance Count", template.instanceCount.ToString());

            EditorGUILayout.Space();

            // Rebuild button
            rebuild |= GUILayout.Button("Rebuild");

            // Rebuild the template mesh when the properties are changed.
            if (rebuild) template.RebuildMesh();
        }

        #endregion

        #region Create menu item functions

        [MenuItem("Assets/Create/Kvant/SprayMV Template")]
        public static void CreateTemplateAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(path), "");
            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/Template.asset");

            // Create a template asset.
            var asset = ScriptableObject.CreateInstance<SprayMVTemplate>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.mesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        #endregion
    }
}
