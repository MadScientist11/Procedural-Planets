using Unity.Mathematics;

namespace Planets
{
    public interface INoiseFilter
    {
        float Sample(float3 value);
    }
}