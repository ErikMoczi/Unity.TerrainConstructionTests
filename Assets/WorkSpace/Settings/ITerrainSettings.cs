namespace WorkSpace.Settings
{
    public interface ITerrainSettings
    {
        int Resolution { get; }
        int ChunkCount { get; }
        ChunkObject ChunkObject { get; }
        NoiseSettings NoiseSettings { get; }
    }
}