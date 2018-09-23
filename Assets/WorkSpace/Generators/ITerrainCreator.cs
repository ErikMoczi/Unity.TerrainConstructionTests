namespace WorkSpace.Generators
{
    public interface ITerrainCreator
    {
        void SetUp();
        void CleanUp();
        void Run();
    }
}