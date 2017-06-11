using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Signaling/Bang Slot")]
    public class BangSlot : NodeBase
    {
        #region Node I/O

        [SerializeField, Outlet]
        VoidEvent _bangEvent = new VoidEvent();

        #endregion

        #region Slot/Signal functions

        public void Signal()
        {
            if (enabled) _bangEvent.Invoke();
        }

        #endregion
    }
}
