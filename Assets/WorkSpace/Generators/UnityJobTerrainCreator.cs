using UnityEngine;
using WorkSpace.Generators.Job;
using WorkSpace.Settings;
using WorkSpace.Utils;

namespace WorkSpace.Generators
{
    public class UnityJobTerrainCreator : TerrainCreator
    {
        public UnityJobTerrainCreator(ITerrainSettings terrainSettings) : base(terrainSettings)
        {
        }

        protected override Mesh CreateMesh(MeshData meshData)
        {
            var mesh = new Mesh();
            using (var meshCalculationGrid = new MeshCalculationGrid(ref meshData))
            {
                meshCalculationGrid.Schedule(64).Complete();
                mesh.vertices = meshCalculationGrid.Vertices.ToArray();
                mesh.triangles = meshCalculationGrid.Triangles.ToArray();
            }

            return mesh;
        }
    }
}