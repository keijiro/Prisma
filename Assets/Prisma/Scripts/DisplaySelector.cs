using UnityEngine;

namespace Prisma
{
    public class DisplaySelector : MonoBehaviour
    {
        [SerializeField] Camera[] _cameras;

        void Start()
        {
            if (Application.isEditor)
            {
                _cameras[0].targetDisplay = 0;
                _cameras[1].targetDisplay = 1;
                _cameras[2].targetDisplay = 2;
            }
            else
            {
                #if PRISMA_SINGLE

                // Single mode
                _cameras[1].enabled = false;
                _cameras[2].enabled = false;

                #elif PRISMA_DUAL

                // Dual mode
                _cameras[0].targetDisplay = 0; // Monitor -> Primary
                _cameras[1].enabled = false;   // Front (off)
                _cameras[2].targetDisplay = 1; // Back -> Secondary
                TryActivateDisplay(0);
                TryActivateDisplay(1);

                #else

                // Triple mode
                _cameras[0].targetDisplay = 0; // Monitor -> Primary
                _cameras[1].targetDisplay = 1; // Front -> Secondary
                _cameras[2].targetDisplay = 2; // Back -> 3rd display
                TryActivateDisplay(0);
                TryActivateDisplay(1);
                TryActivateDisplay(2);

                #endif
            }
        }

        void TryActivateDisplay(int index)
        {
            if (index < Display.displays.Length)
                Display.displays[index].Activate();
        }
    }
}
