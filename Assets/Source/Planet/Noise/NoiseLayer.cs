using System;
using UnityEngine.Serialization;

namespace Planets
{
    [Serializable]
    public struct NoiseLayer
    {
        public bool Enabled;
        [FormerlySerializedAs("UseAsLayerMask")] public bool UseFirstLayerAsMask;
        public NoiseSettings Settings;
    }
}