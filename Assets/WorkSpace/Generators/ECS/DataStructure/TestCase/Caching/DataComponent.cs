using Unity.Entities;
using Unity.Mathematics;
using WorkSpace.Generators.ECS.DataStructure.Measuring;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.Caching
{
    [InternalBufferCapacity(SetUpData.ArraySizeCached)]
    internal struct DataComponent : IBufferElementData
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public float3 Value;

        public static implicit operator float3(DataComponent dataComponent)
        {
            return dataComponent.Value;
        }

        public static implicit operator DataComponent(float3 value)
        {
            return new DataComponent {Value = value};
        }
    }
}