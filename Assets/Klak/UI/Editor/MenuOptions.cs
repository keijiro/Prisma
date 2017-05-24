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
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace Klak.UI
{
    static class MenuOptions
    {
        static T LoadResource<T>(string filename) where T : UnityEngine.Object
        {
            var path = System.IO.Path.Combine("Assets/Klak/UI", filename);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        static void PlaceUIElementRoot(GameObject go, MenuCommand menuCommand)
        {
            // Retrieve an internal method "MenuOptions.PlaceUIElementRoot".
            var type = Type.GetType("UnityEditor.UI.MenuOptions,UnityEditor.UI");
            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var method = type.GetMethod("PlaceUIElementRoot", flags);

            // PlaceUIElementRoot(go, menuCommand)
            method.Invoke(null, new System.Object[]{ go, menuCommand });
        }

        [MenuItem("GameObject/UI/Klak/Button", false, 10000)]
        static void AddButton(MenuCommand menuCommand)
        {
            var go = DefaultControls.CreateButton(
                LoadResource<Sprite>("Texture/Button.png"),
                LoadResource<Font>("Font/DejaVuSans-ExtraLight.ttf")
            );
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Klak/Toggle", false, 10001)]
        static void AddToggle(MenuCommand menuCommand)
        {
            var go = DefaultControls.CreateToggle(
                LoadResource<Sprite>("Texture/Toggle.png"),
                LoadResource<Sprite>("Texture/Toggle Fill.png"),
                LoadResource<Font>("Font/DejaVuSans-ExtraLight.ttf")
            );
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Klak/Knob", false, 10002)]
        static void AddKnob(MenuCommand menuCommand)
        {
            var go = DefaultControls.CreateKnob(
                LoadResource<Material>("Shader/Knob.mat"),
                LoadResource<Sprite>("Texture/Knob.png"),
                LoadResource<Font>("Font/DejaVuSans-ExtraLight.ttf")
            );
            PlaceUIElementRoot(go, menuCommand);
        }
    }
}
