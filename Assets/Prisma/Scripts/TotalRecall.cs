using UnityEngine;
using System.Collections.Generic;

namespace Prisma
{
    public class TotalRecall : MonoBehaviour
    {
        Dictionary<Klak.UI.Knob, float> _knobs;
        Klak.UI.Toggle [] _toggles;

        void Start()
        {
            _knobs = new Dictionary<Klak.UI.Knob, float>();

            foreach (var knob in FindObjectsOfType<Klak.UI.Knob>())
                _knobs.Add(knob, knob.value);

            _toggles = FindObjectsOfType<Klak.UI.Toggle>();
        }

        public void ResetToDefault()
        {
            foreach (var pair in _knobs) pair.Key.value = pair.Value;
            foreach (var toggle in _toggles) toggle.isOn = false;
        }

        public void KillAll()
        {
            foreach (var knob in _knobs.Keys) knob.value = 0;
            foreach (var toggle in _toggles) toggle.isOn = false;
        }
    }
}
