using UnityEngine;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators.Mono
{
    public sealed class SequentialTerrainCreator : TerrainCreatorMonoBehaviour
    {
        public SequentialTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override Mesh CreateMesh(MeshData meshData)
        {
            var dataSize = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var vertices = new Vector3[dataSize];
            var triangles = new int[TerrainSettings.Resolution * TerrainSettings.Resolution * 6];

            for (var i = 0; i < dataSize; i++)
            {
                MeshCreator.GridData(ref vertices, ref triangles, meshData, i);
            }

            return new Mesh {vertices = vertices, triangles = triangles};
        }
    }
}