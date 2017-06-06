// Cloner - An example of use of procedural instancing.
// https://github.com/keijiro/Cloner

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Cloner
{
    //
    // Serializable point cloud object
    //
    // This is useful to reduce memory allocation during run time.
    // (Reading vertex data from a mesh doesn't only allocates GC memory but
    // also requires "Read/Write Enabled" option in the mesh importer that
    // spends heap memory.
    //
    public sealed class PointCloud : ScriptableObject
    {
        #region Public properties and methods

        /// Number of points.
        public int pointCount {
            get { return _positionData.Length; }
        }

        /// Bounding box of the point cloud.
        public Bounds bounds {
            get { return _bounds; }
        }

        /// Create a compute buffer for the position data.
        /// The returned buffer must be released by the caller.
        public ComputeBuffer CreatePositionBuffer()
        {
            var buffer = new ComputeBuffer(pointCount, sizeof(float) * 4);
            buffer.SetData(_positionData);
            return buffer;
        }

        /// Create a compute buffer for the normal data.
        /// The returned buffer must be released by the caller.
        public ComputeBuffer CreateNormalBuffer()
        {
            var buffer = new ComputeBuffer(pointCount, sizeof(float) * 4);
            buffer.SetData(_normalData);
            return buffer;
        }

        /// Create a compute buffer for the tangent data.
        /// The returned buffer must be released by the caller.
        public ComputeBuffer CreateTangentBuffer()
        {
            var buffer = new ComputeBuffer(pointCount, sizeof(float) * 4);
            buffer.SetData(_tangentData);
            return buffer;
        }

        #endregion

        #region Serialized data fields

        [SerializeField] Bounds _bounds;
        [SerializeField] Vector4[] _positionData;
        [SerializeField] Vector4[] _normalData;
        [SerializeField] Vector4[] _tangentData;

        #endregion

        #region Editor functions

        #if UNITY_EDITOR

        public void InitWithMesh(Mesh mesh)
        {
            _bounds = mesh.bounds;

            // Input points
            var inVertices = mesh.vertices;
            var inNormals = mesh.normals;
            var inTangents = mesh.tangents;

            // Enumerate unique points.
            var outVertices = new List<Vector3>();
            var outIndices = new List<int>();

            for (var i = 0; i < inVertices.Length; i++)
            {
                if (!outVertices.Any(_ => _ == inVertices[i]))
                {
                    outVertices.Add(inVertices[i]);
                    outIndices.Add(i);
                }
            }

            // Convert the data into Vector4 arrays.
            _positionData = outIndices.Select(i => (Vector4)inVertices[i]).ToArray();
            _normalData   = outIndices.Select(i => (Vector4)inNormals [i]).ToArray();
            _tangentData  = outIndices.Select(i =>          inTangents[i]).ToArray();
        }

        #endif

        #endregion
    }
}
