using Unity.Collections;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring.Systems;
using WorkSpace.Settings;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.SingleLongArray
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreateSingleLongArraySystem : CreateSystem
    {
        public CreateSingleLongArraySystem(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        public static NativeArray<float3> Data;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var size = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1) * TerrainSettings.ChunkCount;
            Data = new NativeArray<float3>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();
            Data.Dispose();
        }
    }
}