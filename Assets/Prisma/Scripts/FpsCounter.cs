using UnityEngine;
using System.Collections;

namespace Prisma
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _displayText;

        float _timeFrom;
        int _count;

        IEnumerator Start()
        {
            Label = "Initializing";

            yield return new WaitForSeconds(2);

            while (true)
            {
                var timeFrom = Time.time;

                for (var i = 0; i < 30; i++)
                    yield return null;

                var fps = 30 / (Time.time - timeFrom);

                Label = fps.ToString("0.00 fps");
            }
        }

        string Label {
            set { _displayText.text = value; }
        }
    }
}
