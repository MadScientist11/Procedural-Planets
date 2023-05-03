using System;

namespace Planets
{
    [Serializable]
    public struct SimpleNoiseSettings
    {
        public float Frequency;
        public float Persistence;
        public float Amplitude;
        public int Octaves;
        public int Seed;
        public float MinValue;
    }
}