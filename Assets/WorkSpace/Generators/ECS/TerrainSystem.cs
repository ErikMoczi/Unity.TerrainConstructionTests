using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using WorkSpace.Generators.ECS.Base;
using WorkSpace.Generators.Job;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators.ECS
{
    [UpdateBefore(typeof(RenderMeshSystemV2))]
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class TerrainSystem : BaseSystem
    {
        private RenderMesh _renderMesh;

#pragma warning disable 649
        private struct Data
        {
            public readonly int Length;
            [WriteOnly] public ComponentDataArray<Position> Position;
            public EntityArray EntityArray;
        }
#pragma warning restore 649

        [Inject] private Data _data;

        public TerrainSystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _renderMesh = InitRenderMesh(TerrainSettings.ChunkObject);
            var entities = new NativeArray<Entity>(TerrainSettings.ChunkCount, Allocator.Temp);
            var entityArchetype = EntityManager.CreateArchetype(ComponentType.Create<Position>());
            EntityManager.CreateEntity(entityArchetype, entities);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                CreateChunk(i);
            }
        }

        private void CreateChunk(int i)
        {
            var spiralChunkPosition = Common.SpiralChunkPosition(i);
            var meshData = new MeshData(
                TerrainSettings.Resolution,
                spiralChunkPosition,
                TerrainSettings.NoiseSettings
            );

            var position = _data.Position[i];
            position.Value = new float3(spiralChunkPosition.x, 0f, spiralChunkPosition.y);
            _data.Position[i] = position;

            PostUpdateCommands.AddSharedComponent(_data.EntityArray[i], new RenderMesh
            {
                mesh = CreateMesh(meshData),
                castShadows = _renderMesh.castShadows,
                material = _renderMesh.material,
                receiveShadows = _renderMesh.receiveShadows,
                subMesh = _renderMesh.subMesh
            });
        }

        private static Mesh CreateMesh(MeshData meshData)
        {
            var mesh = new Mesh();
            using (var meshCalculationGrid = new MeshCalculationGrid(meshData))
            {
                meshCalculationGrid.Schedule(64).Complete();
                mesh.vertices = meshCalculationGrid.Vertices.ToArray();
                mesh.triangles = meshCalculationGrid.Triangles.ToArray();
            }

            return mesh;
        }

        private static RenderMesh InitRenderMesh(ChunkObject chunkObject)
        {
            var meshRenderer = chunkObject.GetComponent<MeshRenderer>();
            return new RenderMesh
            {
                material = meshRenderer.sharedMaterial,
                castShadows = meshRenderer.shadowCastingMode,
                receiveShadows = meshRenderer.receiveShadows,
                subMesh = 0
            };
        }
    }
}