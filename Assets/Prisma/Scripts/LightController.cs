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
        [SerializeField] float _noiseFrequency = 30;
        [SerializeField, Range(1, 10)] int _noiseOctave = 3;
        [SerializeField] float _boostAmplitude = 10;
        [SerializeField] float _flashFrequency = 1;
        [SerializeField] ParticleSystem _linkedParticleSystem;

        #endregion

        #region Public interface

        public float intensity {
            get { return _intensity; }
            set { _intensity = value; }
        }

        public float saturation {
            get { return _saturation; }
            set { _saturation = value; }
        }

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        public int noiseOctave {
            get { return _noiseOctave; }
            set { _noiseOctave = value; }
        }

        public float boostAmplitude {
            get { return _boostAmplitude; }
            set { _boostAmplitude = value; }
        }

        public float flashFrequency {
            get { return _flashFrequency; }
            set { _flashFrequency = value; }
        }

        public void KickBoost()
        {
            _boost = -1;
        }

        public void ChangeColor(Color color)
        {
            _light.color = color;

            if (_linkedParticleSystem != null)
            {
                var main = _linkedParticleSystem.main;
                main.startColor = color;
            }
        }

        public void RandomizeColor()
        {
            ChangeColor(Color.HSVToRGB(Random.value, _saturation, 1));
            KickBoost();
        }

        public void ResetColor()
        {
            ChangeColor(Color.white);
            KickBoost();
        }

        #endregion

        #region Private members

        Light _light;

        float _originalIntensity;
        float _originalEmissionRate;

        float _intensity = 1;

        NoiseGenerator _noise;

        // Light boost animation: This variable can be negative that means
        // single-frame blackout before boosting.
        float _boost;

        #endregion

        #region MonoBehaviour Functions

        void Start()
        {
            _light = GetComponent<Light>();

            _originalIntensity = _light.intensity;

            _noise = new NoiseGenerator() {
                Frequency = _noiseFrequency,
                FractalLevel = _noiseOctave
            };

            if (_linkedParticleSystem != null)
            {
                var em = _linkedParticleSystem.emission;
                _originalEmissionRate = em.rateOverTime.constant;
            }
        }

        void Update()
        {
            _noise.FractalLevel = _noiseOctave;
            _noise.Frequency = _noiseFrequency;
            _noise.Step();
            if (_boost >= 0) _boost = ETween.Step(_boost, 0, 28);
        }

        void LateUpdate()
        {
            float amp = 0;

            if (_boost >= 0)
            {
                // Base + noise
                amp = 1 + _noise.Value(0) * _noiseAmplitude;

                // Flash and boost
                var flash = Random.value < _flashFrequency * Time.deltaTime;
                amp += Mathf.Max(_boost, flash ? 1 : 0) * _boostAmplitude;
            }
            else
            {
                _boost = 1;
            }

            // Increase intensity if it's not white.
            if (_light.color != Color.white) amp *= 1.3f;

            _light.intensity = _originalIntensity * _intensity * amp;

            if (_linkedParticleSystem != null)
            {
                var em = _linkedParticleSystem.emission;
                em.rateOverTime = _originalEmissionRate * _intensity;
            }
        }

        #endregion
    }
}
