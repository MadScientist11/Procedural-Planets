using System;

namespace Planets
{
    [Serializable]
    public struct RigidNoiseSettings
    {
        public float Frequency;
        public float Persistence;
        public float Amplitude;
        public int Octaves;
        public int Seed;
        public float MinValue;
        public float WeightMultiplier;
    }
}