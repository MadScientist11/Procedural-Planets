using System;

namespace Planets
{
    [Serializable]
    public struct NoiseSettings
    {
        public PlanetNoiseType NoiseType;
        public SimpleNoiseSettings SimpleNoiseSettings;
        public RigidNoiseSettings RigidNoiseSettings;
    }
}