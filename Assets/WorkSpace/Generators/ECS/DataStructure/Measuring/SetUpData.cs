namespace WorkSpace.Generators.ECS.DataStructure.Measuring
{
    public static class SetUpData
    {
        public const int Resolution = 255;
        public const int ChunkCount = 50;
        public const int ArraySizeCached = 128;
        public const int TotalPoints = (Resolution + 1) * (Resolution + 1);
        public const int BatchCountMultipleJobs = 64;
        public const int BatchCountLongArray = 64;
    }
}