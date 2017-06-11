using UnityEngine;
using System.Collections.Generic;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Signaling/Bang Signal")]
    public class BangSignal : NodeBase
    {
        #region Node I/O

        [Inlet]
        public void Bang()
        {
            if (!enabled) return;
            foreach (var slot in SlotList) slot.Signal();
        }

        #endregion

        #region Private members

        List<BangSlot> _slotList;

        List<BangSlot> SlotList {
            get {
                if (_slotList == null) _slotList = ScanSlots(this.name);
                return _slotList;
            }
        }

        static List<BangSlot> ScanSlots(string slotName)
        {
            var list = new List<BangSlot>();
            foreach (var slot in FindObjectsOfType<BangSlot>())
                if (slot.name == slotName) list.Add(slot);
            return list;
        }

        #endregion
    }
}
