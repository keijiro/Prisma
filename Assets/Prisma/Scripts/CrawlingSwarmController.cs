using UnityEngine;

namespace Prisma
{
    public class CrawlingSwarmController : MonoBehaviour
    {
        [SerializeField] Swarm.CrawlingSwarm[] _swarms;

        public float throttle {
            set { _throttle = value; }
        }

        float _throttle;

        void OnDisable()
        {
            foreach (var s in _swarms)
                if (s != null) s.enabled = false;
        }

        void LateUpdate()
        {
            if (_throttle < 0.03f)
            {
                foreach (var s in _swarms) s.enabled = false;
            }
            else
            {
                foreach (var s in _swarms)
                {
                    s.enabled = true;
                    s.trim = _throttle;
                }
            }
        }
    }
}
