using System;

namespace TerrainPerformance
{
    [Serializable]
    internal class TestRunnerResult
    {
        public _Base BaseSetUp;
        public _Terrain Terrain;
        public _Results TestResults;

        [Serializable]
        public class _Base
        {
            public string TestName;
            public int TotalRuns;
        }

        [Serializable]
        public class _Terrain
        {
            public int Resolution;
            public int ChunkCount;
            public float Frequency;
            public int Octaves;
            public float Lacunarity;
            public float Persistence;
        }

        [Serializable]
        public class _Results
        {
            public double First;
            public double Min;
            public double Max;
            public double Median;
            public double Average;
            public double StandardDeviation;
        }
    }
}