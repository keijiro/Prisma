using UnityEngine;

namespace Prisma
{
    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class BlitScreen : MonoBehaviour
    {
        #region Exposed attributes

        [SerializeField] RenderTexture _sourceTexture;
        [SerializeField] Vector2 _scale = Vector2.one;
        [SerializeField] Vector2 _offset = Vector2.zero;
        [SerializeField, ColorUsage(false)] Color _tintColor = Color.white;
        [SerializeField, Range(0, 8)] float _ditherAmount = 0;

        #endregion

        #region Hidden attributes

        [SerializeField, HideInInspector] Shader _shader;
        [SerializeField, HideInInspector] Texture2D _noiseTexture0;
        [SerializeField, HideInInspector] Texture2D _noiseTexture1;
        [SerializeField, HideInInspector] Texture2D _noiseTexture2;
        [SerializeField, HideInInspector] Texture2D _noiseTexture3;
        [SerializeField, HideInInspector] Texture2D _noiseTexture4;
        [SerializeField, HideInInspector] Texture2D _noiseTexture5;
        [SerializeField, HideInInspector] Texture2D _noiseTexture6;
        [SerializeField, HideInInspector] Texture2D _noiseTexture7;

        #endregion

        #region Private members

        Material _material;
        Texture2D[] _noiseTextures;

        void SetUpNoiseTextures()
        {
            _noiseTextures = new [] {
                _noiseTexture0, _noiseTexture1, _noiseTexture2, _noiseTexture3,
                _noiseTexture4, _noiseTexture5, _noiseTexture6, _noiseTexture7
            };
        }

        #endregion

        #region MonoBehaviour Functions

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
            if (_sourceTexture == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            if (_noiseTextures == null) SetUpNoiseTextures();

            var aspect = (float)_sourceTexture.width / _sourceTexture.height;
            var scale = new Vector2(_scale.x, _scale.y * aspect * 9 / 16);
            var offset = _offset - new Vector2(0, (scale.y - 1) / 2);
            var noiseIndex = Random.Range(0, _noiseTextures.Length);

            _material.color = _tintColor;
            _material.SetVector("_Scale", scale);
            _material.SetVector("_Offset", offset);
            _material.SetTexture("_NoiseTex", _noiseTextures[noiseIndex]);
            _material.SetFloat("_DitherAmount", _ditherAmount);

            var pass = _ditherAmount > 0 ? 1 : 0;
            Graphics.Blit(_sourceTexture, destination, _material, pass);
        }

        #endregion
    }
}
