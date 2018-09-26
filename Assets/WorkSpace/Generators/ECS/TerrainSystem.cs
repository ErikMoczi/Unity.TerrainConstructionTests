using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using WorkSpace.Generators.Job;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators.ECS
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(MeshInstanceRendererSystem))]
    public sealed class TerrainSystem : ComponentSystem
    {
        private ITerrainSettings _terrainSettings;
        private MeshInstanceRenderer _meshInstanceRenderer;

        private struct Data
        {
            public readonly int Length;
            [WriteOnly] public ComponentDataArray<Position> Position;
            public EntityArray EntityArray;
        }

        [Inject] private Data _data;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _terrainSettings = ResourcesData.LoadTerrainSettings();
            _meshInstanceRenderer = InitMeshInstanceRenderer(_terrainSettings.ChunkObject);

            var entity = EntityManager.CreateEntity(ComponentType.Create<Position>());
            var entities = new NativeArray<Entity>(_terrainSettings.ChunkCount, Allocator.Temp);
            EntityManager.Instantiate(entity, entities);
            EntityManager.DestroyEntity(entity);
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
            var position = Common.SpiralChunkPosition(i);
            var meshData = new MeshData(
                _terrainSettings.Resolution,
                position,
                _terrainSettings.NoiseSettings
            );

            var item = _data.Position[i];
            item.Value = new float3(position.x, 0f, position.y);
            _data.Position[i] = item;

            PostUpdateCommands.AddSharedComponent(_data.EntityArray[i], new MeshInstanceRenderer
            {
                mesh = CreateMesh(meshData),
                castShadows = _meshInstanceRenderer.castShadows,
                material = _meshInstanceRenderer.material,
                receiveShadows = _meshInstanceRenderer.receiveShadows,
                subMesh = _meshInstanceRenderer.subMesh
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

        private static MeshInstanceRenderer InitMeshInstanceRenderer(ChunkObject chunkObject)
        {
            var meshRenderer = chunkObject.GetComponent<MeshRenderer>();
            return new MeshInstanceRenderer
            {
                material = meshRenderer.sharedMaterial,
                castShadows = meshRenderer.shadowCastingMode,
                receiveShadows = meshRenderer.receiveShadows,
                subMesh = 0
            };
        }
    }
}