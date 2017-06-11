using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Mixing/Bang Mix")]
    public class BangMix : NodeBase
    {
        #region Editable properties

        public enum ModulationType { Off, Or, And, Xor }

        [SerializeField]
        ModulationType _modulationType = ModulationType.Or;

        #endregion

        #region Node I/O

        [Inlet]
        public void Input1On()
        {
            if (!enabled) return;
            _input1 = true;
            InvokeEvent();
        }

        [Inlet]
        public void Input1Off()
        {
            if (!enabled) return;
            _input1 = false;
            InvokeEvent();
        }

        [Inlet]
        public void Input2On()
        {
            if (!enabled) return;
            _input2 = true;
            InvokeEvent();
        }

        [Inlet]
        public void Input2Off()
        {
            if (!enabled) return;
            _input2 = false;
            InvokeEvent();
        }

        [SerializeField, Outlet]
        VoidEvent _onEvent = new VoidEvent();

        [SerializeField, Outlet]
        VoidEvent _offEvent = new VoidEvent();

        #endregion

        #region Private members

        bool _input1, _input2;

        void InvokeEvent()
        {
            bool flag = _input1;

            switch (_modulationType)
            {
                case ModulationType.Or:
                    flag = _input1 || _input2;
                    break;
                case ModulationType.And:
                    flag = _input1 && _input2;
                    break;
                case ModulationType.Xor:
                    flag = _input1 ^ _input2;
                    break;
            }

            if (flag)
                _onEvent.Invoke();
            else
                _offEvent.Invoke();
        }

        #endregion
    }
}
