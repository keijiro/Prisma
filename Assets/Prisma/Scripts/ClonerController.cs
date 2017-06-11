using UnityEngine;

namespace Prisma
{
    public class ClonerController : MonoBehaviour
    {
        [SerializeField] Cloner.ClonerRenderer[] _cloners;

        public float throttle {
            set { _throttle = value; }
        }

        float _throttle;

        float _templateScale;
        float _scaleByNoise;
        float _scaleByPulse;

        void Start()
        {
            _templateScale = _cloners[0].templateScale;
            _scaleByNoise = _cloners[0].scaleByNoise;
            _scaleByPulse = _cloners[0].scaleByPulse;
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
                    c.enabled = true;
                    c.templateScale = _templateScale * _throttle * 2;
                    c.scaleByNoise = _scaleByNoise * _throttle * 2;
                    c.scaleByPulse = _scaleByPulse * _throttle * 2;
                }
            }
        }
    }
}
