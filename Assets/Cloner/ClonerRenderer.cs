// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using UnityEngine;
using Klak.Chromatics;

namespace Cloner
{
    public sealed class ClonerRenderer : MonoBehaviour
    {
        #region Point source properties

        [SerializeField] PointCloud _pointSource;

        public PointCloud pointSource {
            get { return _pointSource; }
        }

        #endregion

        #region Template properties

        [SerializeField] Mesh _template;

        public Mesh template {
            get { return _template; }
        }

        [SerializeField] float _templateScale = 0.05f;

        public float templateScale {
            get { return _templateScale; }
            set { _templateScale = value; }
        }

        [SerializeField] float _scaleByNoise = 0.1f;

        public float scaleByNoise {
            get { return _scaleByNoise; }
            set { _scaleByNoise = value; }
        }

        [SerializeField] float _scaleByPulse = 0.1f;

        public float scaleByPulse {
            get { return _scaleByPulse; }
            set { _scaleByPulse = value; }
        }

        #endregion

        #region Noise field properties

        [SerializeField] float _noiseFrequency = 1;

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        [SerializeField] Vector3 _noiseMotion = Vector3.up * 0.25f;

        public Vector3 noiseMotion {
            get { return _noiseMotion; }
            set { _noiseMotion = value; }
        }

        [SerializeField, Range(0, 1)] float _normalModifier = 0.125f;

        public float normalModifier {
            get { return _normalModifier; }
            set { _normalModifier = value; }
        }

        #endregion

        #region Pulse noise properties

        [SerializeField, Range(0, 0.1f)] float _pulseProbability = 0;

        public float pulseProbability {
            get { return _pulseProbability; }
            set { _pulseProbability = value; }
        }

        [SerializeField] float _pulseFrequency = 2;

        public float pulseFrequency {
            get { return _pulseFrequency; }
            set { _pulseFrequency = value; }
        }

        #endregion

        #region Material properties

        [SerializeField] Material _material;

        public Material material {
            get { return _material; }
        }

        [SerializeField] CosineGradient _gradient;

        public CosineGradient gradient {
            get { return _gradient; }
            set { _gradient = value; }
        }

        #endregion

        #region Misc properties

        [SerializeField] Bounds _bounds =
            new Bounds(Vector3.zero, Vector3.one * 10);

        public Bounds bounds {
            get { return _bounds; }
            set { _bounds = value; }
        }

        [SerializeField] int _randomSeed;

        public int randomSeed {
            get { return _randomSeed; }
        }

        #endregion

        #region Hidden attributes

        [SerializeField, HideInInspector] ComputeShader _compute;

        #endregion

        #region Private fields

        ComputeBuffer _drawArgsBuffer;
        ComputeBuffer _positionBuffer;
        ComputeBuffer _normalBuffer;
        ComputeBuffer _tangentBuffer;
        ComputeBuffer _transformBuffer;
        bool _materialCloned;
        MaterialPropertyBlock _props;
        Vector3 _noiseOffset;
        float _pulseTimer;

        Bounds TransformedBounds {
            get {
                return new Bounds(
                    transform.TransformPoint(_bounds.center),
                    Vector3.Scale(transform.lossyScale, _bounds.size)
                );
            }
        }

        #endregion

        #region Compute configurations

        const int kThreadCount = 64;

        int ThreadGroupCount {
            get { return Mathf.Max(1, _pointSource.pointCount / kThreadCount); }
        }

        int InstanceCount {
            get { return ThreadGroupCount * kThreadCount; }
        }

        #endregion

        #region MonoBehaviour functions

        void OnValidate()
        {
            _noiseFrequency = Mathf.Max(0, _noiseFrequency);
            _pulseFrequency = Mathf.Max(0, _pulseFrequency);
            _bounds.size = Vector3.Max(Vector3.zero, _bounds.size);
        }

        void Start()
        {
            // Initialize the indirect draw args buffer.
            _drawArgsBuffer = new ComputeBuffer(
                1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments
            );

            _drawArgsBuffer.SetData(new uint[5] {
                _template.GetIndexCount(0), (uint)InstanceCount, 0, 0, 0
            });

            // Allocate compute buffers.
            _positionBuffer = _pointSource.CreatePositionBuffer();
            _normalBuffer = _pointSource.CreateNormalBuffer();
            _tangentBuffer = _pointSource.CreateTangentBuffer();
            _transformBuffer = new ComputeBuffer(InstanceCount * 3, 4 * 4);

            // This property block is used only for avoiding an instancing bug.
            _props = new MaterialPropertyBlock();
            _props.SetFloat("_UniqueID", Random.value);

            // Initial noise offset/pulse timer = random seed
            _noiseOffset = Vector3.one * _randomSeed;
            _pulseTimer = _pulseFrequency * _randomSeed;

            // Clone the given material before using.
            _material = new Material(_material);
            _material.name += " (cloned)";
            _materialCloned = true;
        }

        void OnDestroy()
        {
            if (_drawArgsBuffer != null) _drawArgsBuffer.Release();
            if (_positionBuffer != null) _positionBuffer.Release();
            if (_normalBuffer != null) _normalBuffer.Release();
            if (_tangentBuffer != null) _tangentBuffer.Release();
            if (_transformBuffer != null) _transformBuffer.Release();
            if (_materialCloned) Destroy(_material);
        }

        void Update()
        {
            // Invoke the update compute kernel.
            var kernel = _compute.FindKernel("ClonerUpdate");

            _compute.SetInt("InstanceCount", InstanceCount);
            _compute.SetFloat("RcpInstanceCount", 1.0f / InstanceCount);

            _compute.SetFloat("BaseScale", _templateScale);
            _compute.SetFloat("ScaleNoise", _scaleByNoise);
            _compute.SetFloat("ScalePulse", _scaleByPulse);

            _compute.SetFloat("NoiseFrequency", _noiseFrequency);
            _compute.SetVector("NoiseOffset", _noiseOffset);
            _compute.SetFloat("NormalModifier", _normalModifier);

            _compute.SetFloat("PulseProbability", _pulseProbability);
            _compute.SetFloat("PulseTime", _pulseTimer);

            _compute.SetBuffer(kernel, "PositionBuffer", _positionBuffer);
            _compute.SetBuffer(kernel, "NormalBuffer", _normalBuffer);
            _compute.SetBuffer(kernel, "TangentBuffer", _tangentBuffer);
            _compute.SetBuffer(kernel, "TransformBuffer", _transformBuffer);

            _compute.Dispatch(kernel, ThreadGroupCount, 1, 1);

            // Draw the template mesh with instancing.
            _material.SetVector("_GradientA", _gradient.coeffsA);
            _material.SetVector("_GradientB", _gradient.coeffsB);
            _material.SetVector("_GradientC", _gradient.coeffsC2);
            _material.SetVector("_GradientD", _gradient.coeffsD2);

            _material.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
            _material.SetMatrix("_WorldToLocal", transform.worldToLocalMatrix);

            _material.SetBuffer("_TransformBuffer", _transformBuffer);
            _material.SetInt("_InstanceCount", InstanceCount);

            Graphics.DrawMeshInstancedIndirect(
                _template, 0, _material, TransformedBounds,
                _drawArgsBuffer, 0, _props
            );

            // Update the internal state.
            _noiseOffset += _noiseMotion * Time.deltaTime;
            _pulseTimer += _pulseFrequency * Time.deltaTime;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }

        #endregion
    }
}
