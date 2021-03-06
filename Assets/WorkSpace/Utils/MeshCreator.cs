﻿using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using WorkSpace.Settings;

namespace WorkSpace.Utils
{
    /*
     * GridData algorithm
     * 
        var x = i % (TerrainSettings.Resolution + 1);
        var z = i / (TerrainSettings.Resolution + 1);
        
        var point00 = new Vector2(-0.5f, -0.5f) + offset;
        var point10 = new Vector2(0.5f, -0.5f) + offset;
        var point01 = new Vector2(-0.5f, 0.5f) + offset;
        var point11 = new Vector2(0.5f, 0.5f) + offset;

        var point0 = Vector2.Lerp(point00, point01, z * stepSize);
        var point1 = Vector2.Lerp(point10, point11, z * stepSize);

        var point = Vector2.Lerp(point0, point1, x * stepSize);
        var noiseValue = Common.NoiseValue(point, TerrainSettings.NoiseSettings);
        
        vertices[i] = new Vector3(x * stepSize - 0.5f, noiseValue, z * stepSize - 0.5f);
        if (x < TerrainSettings.Resolution && z < TerrainSettings.Resolution)
        {
            var t = 6 * (i - z);
            triangles[t] = i;
            triangles[t + 1] = i + TerrainSettings.Resolution + 1;
            triangles[t + 2] = i + 1;
            triangles[t + 3] = i + 1;
            triangles[t + 4] = i + TerrainSettings.Resolution + 1;
            triangles[t + 5] = i + TerrainSettings.Resolution + 2;
        }
     */
    internal static class MeshCreator
    {
        public static void GridData(ref NativeArray<Vector3> vertices, ref NativeArray<int> triangles,
            MeshData meshData, int index)
        {
            var position = Position(index, meshData.Size);
            vertices[index] = Vertex(meshData.StepSize, position, meshData.Offset, meshData.NoiseSettings);
            Triangle(ref triangles, index, meshData.Size, position);
        }

        public static void GridData(Vector3[] vertices, int[] triangles, MeshData meshData, int index)
        {
            var position = Position(index, meshData.Size);
            vertices[index] = Vertex(meshData.StepSize, position, meshData.Offset, meshData.NoiseSettings);
            Triangle(triangles, index, meshData.Size, position);
        }

        private static int2 Position(int index, int size)
        {
            return math.int2(
                index % (size + 1),
                index / (size + 1)
            );
        }

        private static float3 Vertex(float stepSize, int2 position, Vector2 offset, NoiseSettings noiseSettings)
        {
            var point = Common.MiddlePosition(position.x, position.y, stepSize, offset);
            var noiseValue = Common.NoiseValue(point, noiseSettings);
            return math.float3(position.x * stepSize - 0.5f, noiseValue, position.y * stepSize - 0.5f);
        }

        private static void Triangle(int[] triangles, int index, int size, int2 position)
        {
            if (position.x < size && position.y < size)
            {
                var t = 6 * (index - position.y);
                triangles[t] = index;
                triangles[t + 1] = index + size + 1;
                triangles[t + 2] = index + 1;
                triangles[t + 3] = index + 1;
                triangles[t + 4] = index + size + 1;
                triangles[t + 5] = index + size + 2;
            }
        }

        private static void Triangle(ref NativeArray<int> triangles, int index, int size, int2 position)
        {
            if (position.x < size && position.y < size)
            {
                var t = 6 * (index - position.y);
                triangles[t] = index;
                triangles[t + 1] = index + size + 1;
                triangles[t + 2] = index + 1;
                triangles[t + 3] = index + 1;
                triangles[t + 4] = index + size + 1;
                triangles[t + 5] = index + size + 2;
            }
        }
    }
}