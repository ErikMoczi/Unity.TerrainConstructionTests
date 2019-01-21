using System.Threading.Tasks;
using UnityEngine;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators.Mono
{
    public sealed class ParallelTerrainCreator : TerrainCreatorMonoBehaviour
    {
        public ParallelTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        private protected override Mesh CreateMesh(MeshData meshData)
        {
            var dataSize = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var vertices = new Vector3[dataSize];
            var triangles = new int[TerrainSettings.Resolution * TerrainSettings.Resolution * 6];

            Parallel.For(0, dataSize, i => { MeshCreator.GridData(vertices, triangles, meshData, i); });

            return new Mesh {vertices = vertices, triangles = triangles};
        }
    }
}