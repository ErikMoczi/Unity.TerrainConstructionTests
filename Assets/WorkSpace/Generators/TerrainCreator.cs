using WorkSpace.Settings;

namespace WorkSpace.Generators
{
    public abstract class TerrainCreator : ITerrainCreator
    {
        protected ITerrainSettings TerrainSettings { get; }

        // ReSharper disable once PublicConstructorInAbstractClass
        public TerrainCreator(ITerrainSettings terrainSettings)
        {
            TerrainSettings = terrainSettings;
        }

        public abstract void SetUp();
        public abstract void Run();
        public abstract void CleanUp();
    }
}