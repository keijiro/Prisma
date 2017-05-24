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

namespace Klak.UI
{
    [AddComponentMenu("UI/Klak/Configuration")]
    public class Configuration : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] float _knobSensitivity = 1;

        public float knobSensitivity {
            get { return _knobSensitivity; }
        }

        #endregion

        #region Static members


        // Search a given hierarchy for a configuration.
        static public Configuration Search(GameObject go)
        {
            // Return the default configuration if there is no
            // configuration instance in the parent hierarchy.
            var candid = go.GetComponentInParent<Configuration>();
            return candid != null ? candid : defaultInstance;
        }

        // Default configuration instance
        static Configuration s_defaultInstance;

        static Configuration defaultInstance {
            get {
                if (s_defaultInstance == null)
                {
                    // Create a hidden game object to store the configuration.
                    var go = new GameObject("VJUI Configuration");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    s_defaultInstance = go.AddComponent<Configuration>();
                }
                return s_defaultInstance;
            }
        }

        #endregion
    }
}
