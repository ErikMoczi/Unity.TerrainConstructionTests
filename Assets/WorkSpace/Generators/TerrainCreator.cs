using WorkSpace.Settings;

namespace WorkSpace.Generators
{
    public abstract class TerrainCreator : ITerrainCreator
    {
        private readonly ITerrainSettings _terrainSettings;
        protected ITerrainSettings TerrainSettings => _terrainSettings;

        public TerrainCreator(ITerrainSettings terrainSettings)
        {
            _terrainSettings = terrainSettings;
        }

        public abstract void SetUp();
        public abstract void Run();
        public abstract void CleanUp();
    }
}