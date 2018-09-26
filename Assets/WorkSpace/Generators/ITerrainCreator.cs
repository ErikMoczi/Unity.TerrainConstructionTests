namespace WorkSpace.Generators
{
    public interface ITerrainCreator
    {
        void SetUp();
        void Run();
        void CleanUp();
    }
}