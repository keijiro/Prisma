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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Klak.Wiring;

namespace Klak.UI
{
    [AddComponentMenu("UI/Klak/Knob")]
    public class Knob : Selectable, IDragHandler, IInitializePotentialDragHandler
    {
        #region Editable properties

        [SerializeField] float _minValue = 0;

        public float minValue
        {
            get { return _minValue; }
            set { _minValue = value; Set(_value); }
        }

        [SerializeField] float _maxValue = 1;

        public float maxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; Set(_value); }
        }

        [SerializeField] float _value;

        public float value
        {
            get { return _value; }
            set { Set(value); }
        }

        public float normalizedValue
        {
            get
            {
                if (Mathf.Approximately(minValue, maxValue)) return 0;
                return Mathf.InverseLerp(minValue, maxValue, value);
            }
            set
            {
                this.value = Mathf.Lerp(minValue, maxValue, value);
            }
        }

        [SerializeField] Graphic _graphic;

        public Graphic graphic
        {
            get { return _graphic; }
            set { _graphic = value; UpdateVisuals(); }
        }

        [SerializeField] UIKnobInput _target;

        public UIKnobInput target
        {
            get { return _target; }
            set { _target = value; }
        }

        #endregion

        #region Private methods

        DrivenRectTransformTracker _tracker;
        Configuration _config; 
        Vector2 _dragPoint;
        float _dragOffset;

        protected Knob() {}

        void Set(float input, bool sendCallback = true)
        {
            var newValue = Mathf.Clamp(input, minValue, maxValue);
            if (_value == newValue) return;

            _value = newValue;
            UpdateVisuals();

            if (sendCallback && _target != null)
                _target.ChangeValue(newValue);
        }

        bool MayDrag(PointerEventData eventData)
        {
            return IsActive() && IsInteractable() &&
                eventData.button == PointerEventData.InputButton.Left;
        }

        void UpdateVisuals()
        {
            if (_graphic == null) return;

            _tracker.Clear();
            _tracker.Add(this, _graphic.rectTransform, DrivenTransformProperties.Rotation);

            var angle = 179.9f - normalizedValue * 359.9f;
            _graphic.rectTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            var rectTransform = GetComponent<RectTransform>();

            Vector2 input;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle
                (rectTransform, eventData.position, cam, out input)) return;

            var delta = Vector2.Dot(input - _dragPoint, Vector2.one);
            delta *= _config.knobSensitivity * 0.005f;

            normalizedValue = _dragOffset + delta;
        }

        #endregion

        #region Selectable functions

        protected override void OnEnable()
        {
            base.OnEnable();

            _config = Configuration.Search(gameObject);

            Set(_value, false);
            UpdateVisuals();
        }

        protected override void OnDisable()
        {
            _tracker.Clear();
            base.OnDisable();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (IsActive())
            {
                Set(_value, false);
                UpdateVisuals();
            }
        }
#endif

        protected override void Start()
        {
            if (_target != null) _target.ChangeValue(_value);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!MayDrag(eventData)) return;

            base.OnPointerDown(eventData);

            _dragPoint = Vector2.zero;
            _dragOffset = normalizedValue;

            var rectTransform = GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (rectTransform, eventData.position, eventData.pressEventCamera, out _dragPoint);
        }

        #endregion

        #region IInitializePotentialDragHandler implementation

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        #endregion

        #region IDragHandler implementation

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData)) return;
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        #endregion
    }
}
