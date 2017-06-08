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
    // Custom material editor for SprayMV materials
    public class SprayMVMaterialEditor : ShaderGUI
    {
        static GUIContent _textAlbedoMap = new GUIContent("Albedo Map");
        static GUIContent _textNormalMap = new GUIContent("Normal Map");
        static GUIContent _textOcclusionMap = new GUIContent("Occlusion Map");

        bool _initial = true;

        public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
        {
            if (ShaderPropertiesGUI(editor, props) || _initial)
                foreach (Material m in editor.targets)
                    SetMaterialKeywords(m);
            _initial = false;
        }

        bool ShaderPropertiesGUI(MaterialEditor editor, MaterialProperty[] props)
        {
            EditorGUI.BeginChangeCheck();

            // Colors
            var colorMode = FindProperty("_ColorMode", props);
            var color1 = FindProperty("_Color", props);
            var color2 = FindProperty("_Color2", props);

            editor.ShaderProperty(colorMode, "Color Mode");

            if (colorMode.floatValue > 0)
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.x += EditorGUIUtility.labelWidth;
                rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2 - 2;
                editor.ShaderProperty(rect, color1, "");
                rect.x += rect.width + 4;
                editor.ShaderProperty(rect, color2, "");
            }
            else
            {
                editor.ShaderProperty(color1, " ");
            }

            EditorGUILayout.Space();

            // Metallic/Smoothness
            editor.RangeProperty(FindProperty("_Metallic", props), "Metallic");
            editor.RangeProperty(FindProperty("_Smoothness", props), "Smoothness");

            EditorGUILayout.Space();

            // Albedo map
            var texture = FindProperty("_MainTex", props);
            editor.TexturePropertySingleLine(_textAlbedoMap, texture);

            // Normal map
            texture = FindProperty("_NormalMap", props);
            var option = FindProperty("_NormalScale", props);
            editor.TexturePropertySingleLine(
                _textNormalMap, texture,
                texture.textureValue != null ? option : null
            );

            // Occlusion map
            texture = FindProperty("_OcclusionMap", props);
            option = FindProperty("_OcclusionStrength", props);
            editor.TexturePropertySingleLine(
                _textOcclusionMap, texture,
                texture.textureValue != null ? option : null
            );

            EditorGUILayout.Space();

            // Scale/Tiling
            texture = FindProperty("_MainTex", props);
            editor.TextureScaleOffsetProperty(texture);

            EditorGUILayout.Space();

            // Emission
            editor.ShaderProperty(FindProperty("_Emission", props), "Emission Intensity");

            return EditorGUI.EndChangeCheck();
        }

        static void SetMaterialKeywords(Material material)
        {
            SetKeyword(material, "_ALBEDOMAP", material.GetTexture("_MainTex"));
            SetKeyword(material, "_NORMALMAP", material.GetTexture("_NormalMap"));
            SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
            SetKeyword(material, "_EMISSION", material.GetFloat("_Emission") > 0);
        }

        static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }
    }
}
