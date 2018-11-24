using Unity.Entities;
using Unity.Mathematics;

namespace WorkSpace.Generators.ECS.DataStructure.TestCase.MassivePoints
{
    internal struct DataComponent : IComponentData
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