using Unity.Entities;
using Unity.Mathematics;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MultipleJobs
{
    internal struct DataComponent : IBufferElementData
    {
        // ReSharper disable once NotAccessedField.Global
        public float3 Value;
    }
}