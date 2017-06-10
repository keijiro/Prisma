using UnityEngine;
using Klak.Chromatics;
using Klak.Math;

namespace Prisma
{
    public class LightController : MonoBehaviour
    {
        #region Exposed attributes

        [SerializeField, Range(0, 1)] float _saturation = 0.5f;
        [SerializeField, Range(0, 2)] float _noiseAmplitude = 1;
        [SerializeField] float _flashFrequency = 1;
        [SerializeField] float _flashStrength = 10;

        #endregion

        #region Public interface

        public float saturation {
            get { return _saturation; }
            set { _saturation = value; }
        }

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        public float flashFrequency {
            get { return _flashFrequency; }
            set { _flashFrequency = value; }
        }

        public float flashStrength {
            get { return _flashStrength; }
            set { _flashStrength = value; }
        }

        public void ChangeColor()
        {
            _light.color = Color.HSVToRGB(Random.value, _saturation, 1);
            _light.color *= 1 + _saturation / 2;
            _boost = -1;
        }

        public void ResetColor()
        {
            _light.color = Color.white;
            _boost = -1;
        }

        #endregion

        #region Private members

        Light _light;
        float _originalIntensity;

        NoiseGenerator _noise = new NoiseGenerator(30) { FractalLevel = 8 };
        float _boost;

        #endregion

        #region MonoBehaviour Functions

        void Start()
        {
            _light = GetComponent<Light>();
            _originalIntensity = _light.intensity;
        }

        void Update()
        {
            // Update the internal state.
            _boost = _boost < 0 ? 1 : ETween.Step(_boost, 0, 28);
            _noise.Step();
        }

        void LateUpdate()
        {
            if (_boost < 0)
            {
                // Insert a black frame before flash.
                _light.intensity = 0;
            }
            else
            {
                // Base + noise
                var amp = 1 + _noise.Value(0) * _noiseAmplitude;

                // Flash and boost
                var flash = Random.value < _flashFrequency * Time.deltaTime;
                amp += Mathf.Max(_boost, flash ? 1 : 0) * _flashStrength;

                // Update the light intensity.
                _light.intensity = _originalIntensity * amp;
            }
        }

        #endregion
    }
}
