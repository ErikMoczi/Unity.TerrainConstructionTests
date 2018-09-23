using UnityEngine;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators
{
    public abstract class TerrainCreator : ITerrainCreator
    {
        private readonly ITerrainSettings _terrainSettings;
        private ChunkObject[] _chunkObjects;

        protected ITerrainSettings TerrainSettings => _terrainSettings;
        protected ChunkObject[] ChunkObjects => _chunkObjects;

        public TerrainCreator(ITerrainSettings terrainSettings)
        {
            _terrainSettings = terrainSettings;
        }

        public virtual void SetUp()
        {
        }

        public virtual void CleanUp()
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

        public void Run()
        {
            _chunkObjects = new ChunkObject[_terrainSettings.ChunkCount];
            CreateChunks();
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
            ChunkObjects[index] = chunkObject;
        }

        protected abstract Mesh CreateMesh(MeshData meshData);
    }
}