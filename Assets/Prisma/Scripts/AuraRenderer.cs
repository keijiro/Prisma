using UnityEngine;

namespace Prisma
{
    public class AuraRenderer : MonoBehaviour
    {
        #region Exposed attributes

        [SerializeField] int _triangleCount = 100;

        public int triangleCount {
            get { return _triangleCount; }
            set { _triangleCount = value; }
        }

        [SerializeField] float _triangleExtent = 0.1f;

        public float triangleExtent {
            get { return _triangleExtent; }
            set { _triangleExtent = value; }
        }

        [SerializeField] float _shuffleSpeed = 4;

        public float shuffleSpeed {
            get { return _shuffleSpeed; }
            set { _shuffleSpeed = value; }
        }

        [SerializeField] float _noiseAmplitude = 1;

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        [SerializeField] float _noiseFrequency = 1;

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        [SerializeField] Vector3 _noiseMotion = Vector3.up;

        public Vector3 noiseMotion {
            get { return _noiseMotion; }
            set { _noiseMotion = value; }
        }

        [SerializeField] Material _material;

        public Material material {
            get { return _material; }
        }

        #endregion

        #region Hidden attributes

        [SerializeField, HideInInspector] ComputeShader _compute;

        #endregion

        #region Private fields

        Mesh _mesh;
        ComputeBuffer _drawArgsBuffer;
        ComputeBuffer _positionBuffer;
        ComputeBuffer _normalBuffer;
        MaterialPropertyBlock _props;
        Vector3 _noiseOffset;

        #endregion

        #region Compute configurations

        const int kThreadCount = 64;
        int ThreadGroupCount { get { return _triangleCount / kThreadCount; } }
        int TriangleCount { get { return kThreadCount * ThreadGroupCount; } }

        #endregion

        #region MonoBehaviour functions

        void OnValidate()
        {
            _triangleCount = Mathf.Max(kThreadCount, _triangleCount);
            _triangleExtent = Mathf.Max(0, _triangleExtent);
            _noiseFrequency = Mathf.Max(0, _noiseFrequency);
        }

        void Start()
        {
            // Mesh with single triangle.
            _mesh = new Mesh();
            _mesh.vertices = new Vector3 [3];
            _mesh.SetIndices(new [] {0, 1, 2}, MeshTopology.Triangles, 0);
            _mesh.UploadMeshData(true);

            // Allocate the indirect draw args buffer.
            _drawArgsBuffer = new ComputeBuffer(
                1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments
            );

            // This property block is used only for avoiding a bug (issue #913828)
            _props = new MaterialPropertyBlock();
            _props.SetFloat("_UniqueID", Random.value);

            // Clone the given material before using.
            _material = new Material(_material);
            _material.name += " (cloned)";
        }

        void OnDestroy()
        {
            Destroy(_mesh);
            _drawArgsBuffer.Release();
            if (_positionBuffer != null) _positionBuffer.Release();
            if (_normalBuffer != null) _normalBuffer.Release();
            Destroy(_material);
        }

        void Update()
        {
            // Allocate/Reallocate the compute buffers when it hasn't been
            // initialized or the triangle count was changed from the last frame.
            if (_positionBuffer == null || _positionBuffer.count != TriangleCount * 3)
            {
                if (_positionBuffer != null) _positionBuffer.Release();
                if (_normalBuffer != null) _normalBuffer.Release();

                _positionBuffer = new ComputeBuffer(TriangleCount * 3, 16);
                _normalBuffer = new ComputeBuffer(TriangleCount * 3, 16);

                _drawArgsBuffer.SetData(new uint[5] {3, (uint)TriangleCount, 0, 0, 0});
            }

            // Invoke the update compute kernel.
            var kernel = _compute.FindKernel("Update");

            _compute.SetFloat("Time", Time.time * _shuffleSpeed);
            _compute.SetFloat("Extent", _triangleExtent);
            _compute.SetFloat("NoiseAmplitude", _noiseAmplitude);
            _compute.SetFloat("NoiseFrequency", _noiseFrequency);
            _compute.SetVector("NoiseOffset", _noiseOffset);

            _compute.SetBuffer(kernel, "PositionBuffer", _positionBuffer);
            _compute.SetBuffer(kernel, "NormalBuffer", _normalBuffer);

            _compute.Dispatch(kernel, ThreadGroupCount, 1, 1);

            // Move the noise field.
            _noiseOffset += _noiseMotion * Time.deltaTime;
        }

        void LateUpdate()
        {
            // Draw the mesh with instancing.
            _material.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
            _material.SetMatrix("_WorldToLocal", transform.worldToLocalMatrix);

            _material.SetBuffer("_PositionBuffer", _positionBuffer);
            _material.SetBuffer("_NormalBuffer", _normalBuffer);

            Graphics.DrawMeshInstancedIndirect(
                _mesh, 0, _material,
                new Bounds(transform.position, transform.lossyScale * 5),
                _drawArgsBuffer, 0, _props
            );
        }

        #endregion
    }
}
