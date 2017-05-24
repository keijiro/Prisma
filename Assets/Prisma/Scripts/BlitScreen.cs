using UnityEngine;

namespace Prisma
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class BlitScreen : MonoBehaviour
    {
        [SerializeField] RenderTexture _source;
        [SerializeField] Color _tint = Color.white;
        [SerializeField] Vector2 _scale = Vector2.one;
        [SerializeField] Vector2 _offset = Vector2.zero;
        [SerializeField] bool _test = false;

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        void OnDestroy()
        {
            if (_material != null)
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_source == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            var aspect = (float)_source.width / _source.height;
            var scale = new Vector2(_scale.x, _scale.y * aspect * 9 / 16);
            var offset = _offset - new Vector2(0, (scale.y - 1) / 2);

            _material.color = _tint;
            _material.SetVector("_Scale", scale);
            _material.SetVector("_Offset", offset);

            var pass = _test ? 1 : 0;
            Graphics.Blit(_source, destination, _material, pass);
        }
    }
}
