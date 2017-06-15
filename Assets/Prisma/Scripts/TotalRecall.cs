using UnityEngine;
using System.Collections.Generic;

namespace Prisma
{
    public class TotalRecall : MonoBehaviour
    {
        [SerializeField] Klak.UI.Button [] _buttonsOnReset;

        Dictionary<Klak.UI.Knob, float> _knobs;
        Dictionary<Klak.UI.Toggle, bool> _toggles;

        void Start()
        {
            _knobs = new Dictionary<Klak.UI.Knob, float>();

            foreach (var knob in FindObjectsOfType<Klak.UI.Knob>())
                _knobs.Add(knob, knob.value);

            _toggles = new Dictionary<Klak.UI.Toggle, bool>();

            foreach (var toggle in FindObjectsOfType<Klak.UI.Toggle>())
                _toggles.Add(toggle, toggle.isOn);
        }

        public void ResetToDefault()
        {
            foreach (var pair in _knobs) pair.Key.value = pair.Value;
            foreach (var pair in _toggles) pair.Key.isOn = pair.Value;
            foreach (var button in _buttonsOnReset) button.target.ButtonDown();
        }

        public void KillAll()
        {
            foreach (var knob in _knobs.Keys) knob.value = 0;
            foreach (var toggle in _toggles.Keys) toggle.isOn = false;
        }
    }
}
