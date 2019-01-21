using UnityEngine;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators.Mono
{
    public abstract class TerrainCreatorMonoBehaviour : TerrainCreator
    {
        private ChunkObject[] _chunkObjects;

        // ReSharper disable once PublicConstructorInAbstractClass
        public TerrainCreatorMonoBehaviour(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        public sealed override void SetUp()
        {
        }

        public sealed override void Run()
        {
            _chunkObjects = new ChunkObject[TerrainSettings.ChunkCount];
            CreateChunks();
        }

        public sealed override void CleanUp()
        {
            if (_chunkObjects == null)
            {
                return;
            }

            foreach (var chunkObject in _chunkObjects)
            {
                Object.DestroyImmediate(chunkObject.gameObject);
            }

            _chunkObjects = null;
        }

        private void CreateChunks()
        {
            for (var i = 0; i < TerrainSettings.ChunkCount; i++)
            {
                MainThreadDispatch.Instance().Enqueue(obj => { CreateChunk((int) obj); }, i);
            }
        }

        private void CreateChunk(int index)
        {
            var chunkObject = Object.Instantiate(TerrainSettings.ChunkObject);
            var position = Common.SpiralChunkPosition(index);
            var meshData = new MeshData(
                TerrainSettings.Resolution,
                position,
                TerrainSettings.NoiseSettings
            );

            chunkObject.transform.position = new Vector3(position.x, 0f, position.y);
            chunkObject.MeshFilter.mesh = CreateMesh(meshData);
            _chunkObjects[index] = chunkObject;
        }

        private protected abstract Mesh CreateMesh(MeshData meshData);
    }
}