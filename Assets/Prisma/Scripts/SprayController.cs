using UnityEngine;

namespace Prisma
{
    class SprayController : MonoBehaviour
    {
        [SerializeField] Transform _target;

        Kvant.SprayMV _spray;

        void Start()
        {
            _spray = GetComponent<Kvant.SprayMV>();
        }

        void Update()
        {
            _spray.emitterCenter = _target.position;
        }
    }
}
