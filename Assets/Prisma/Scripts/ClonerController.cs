using UnityEngine;
using Klak.Math;

namespace Prisma
{
    public class ClonerController : MonoBehaviour
    {
        [SerializeField] Cloner.ClonerRenderer[] _cloners;

        public float throttle {
            get { return _throttle; }
            set { _throttle = value; }
        }

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        float _throttle;
        float _noiseAmplitude;

        float _templateScale;
        float _scaleByNoise;
        float _scaleByPulse;

        NoiseGenerator _noise = new NoiseGenerator(18) { FractalLevel = 8 };

        void Start()
        {
            _templateScale = _cloners[0].templateScale;
            _scaleByNoise = _cloners[0].scaleByNoise;
            _scaleByPulse = _cloners[0].scaleByPulse;
        }

        void OnDisable()
        {
            foreach (var c in _cloners) c.enabled = false;
        }

        void Update()
        {
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
                    var noise = _noise.Value(0) * _noiseAmplitude * 2;
                    var amp = Mathf.Max(_throttle + noise) * 2;
                    c.enabled = true;
                    c.templateScale = _templateScale * amp;
                    c.scaleByNoise = _scaleByNoise * amp;
                    c.scaleByPulse = _scaleByPulse * amp;
                }
            }
        }
    }
}
