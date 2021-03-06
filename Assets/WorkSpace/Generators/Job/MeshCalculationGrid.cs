using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using WorkSpace.Utils;

namespace WorkSpace.Generators.Job
{
    [BurstCompile]
    internal struct MeshCalculationGrid : IJobParallelForGrid, IDisposable
    {
        [WriteOnly] private NativeArray<Vector3> _vertices;

        [NativeDisableParallelForRestriction, WriteOnly]
        private NativeArray<int> _triangles;

        public NativeArray<Vector3> Vertices => _vertices;
        public NativeArray<int> Triangles => _triangles;

        public int GridSize { get; }

        [ReadOnly] private readonly MeshData _meshData;

        public MeshCalculationGrid(MeshData meshData) : this()
        {
            _meshData = meshData;
            GridSize = (meshData.Size + 1) * (meshData.Size + 1);
            _vertices = new NativeArray<Vector3>(GridSize, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
            _triangles = new NativeArray<int>(meshData.Size * meshData.Size * 6, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
        }

        public void Execute(int i)
        {
            MeshCreator.GridData(ref _vertices, ref _triangles, _meshData, i);
        }

        public void Dispose()
        {
            _vertices.Dispose();
            _triangles.Dispose();
        }
    }
}