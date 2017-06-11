using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BangSignal))]
    public class BangSignalEditor : Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }
}
