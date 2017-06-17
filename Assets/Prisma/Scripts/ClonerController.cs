using UnityEngine;
using Klak.Math;

namespace Prisma
{
    public class ClonerController : MonoBehaviour
    {
        [SerializeField] Cloner.ClonerRenderer[] _cloners;
        [SerializeField] float _noiseFrequency = 18;
        [SerializeField, Range(1, 10)] int _noiseOctave = 3;

        public float throttle {
            get { return _throttle; }
            set { _throttle = value; }
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

        float _throttle;
        float _noiseAmplitude;

        float _templateScale;
        float _scaleByNoise;
        float _scaleByPulse;

        NoiseGenerator _noise;

        void Start()
        {
            _templateScale = _cloners[0].templateScale;
            _scaleByNoise = _cloners[0].scaleByNoise;
            _scaleByPulse = _cloners[0].scaleByPulse;

            _noise = new NoiseGenerator() {
                Frequency = _noiseFrequency,
                FractalLevel = _noiseOctave
            };
        }

        void OnDisable()
        {
            foreach (var c in _cloners)
                if (c != null) c.enabled = false;
        }

        void Update()
        {
            _noise.FractalLevel = _noiseOctave;
            _noise.Frequency = _noiseFrequency;
            _noise.Step();
        }

        void LateUpdate()
        {
            if (_throttle < 0.01f)
            {
                foreach (var c in _cloners) c.enabled = false;
            }
            else
            {
                foreach (var c in _cloners)
                {
                    var noise = _noise.Value(0) * _noiseAmplitude * 1.5f;
                    var amp = Mathf.Max(0, _throttle * (1 + noise)) * 2;
                    c.enabled = true;
                    c.templateScale = _templateScale * amp;
                    c.scaleByNoise = _scaleByNoise * amp;
                    c.scaleByPulse = _scaleByPulse * amp;
                }
            }
        }
    }
}
