using System.Threading.Tasks;
using UnityEngine;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators
{
    public class ParallelTerrainCreator : TerrainCreator
    {
        public ParallelTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override Mesh CreateMesh(MeshData meshData)
        {
            var dataSize = (TerrainSettings.Resolution + 1) * (TerrainSettings.Resolution + 1);
            var vertices = new Vector3[dataSize];
            var triangles = new int[TerrainSettings.Resolution * TerrainSettings.Resolution * 6];

            Parallel.For(0, dataSize, i => { MeshCreator.GridData(ref vertices, ref triangles, ref meshData, i); });

            return new Mesh {vertices = vertices, triangles = triangles};
        }
    }
}