using Unity.Collections;
using UnityEngine;

namespace Planets
{
    public struct PlanetSettingsDto
    {
        public int Resolution;
        public float Radius;
        public Color Color;
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<NoiseLayer> NoiseLayers;
    }
}