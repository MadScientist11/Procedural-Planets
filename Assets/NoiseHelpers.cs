using Unity.Mathematics;
using Noise =  Unity.Mathematics.noise;

namespace Planets
{
    public class NoiseHelpers
    {
        private static float Noise3dFbm(float3 value, float frequency, float amplitude, float persistence, int octave, int seed)
        {
            float noise = 0.0f;

            for (int i = 0; i < octave; ++i)
            {
                float snoise = Noise.snoise(value * frequency + seed, out _);
                noise += (snoise + 1) * 0.5f * amplitude;
                amplitude *= persistence;
                frequency *= 2.0f;
            }

            return noise / octave;
        }
        
        public static float SampleSimpleNoise(float3 value, SimpleNoiseSettings settings)
        {
            float elevation = Noise3dFbm(value, settings.Frequency, settings.Amplitude,
                settings.Persistence, settings.Octaves, settings.Seed);

            elevation = math.max(elevation - settings.MinValue, 0);
            return elevation;
        }
        
        public static float SampleRigidNoise(float3 value, RigidNoiseSettings settings)
        {
            float elevation = Noise3dFbm(value, settings.Frequency, settings.Amplitude,
                settings.Persistence, settings.Octaves, settings.Seed);

            elevation = math.max(elevation - settings.MinValue, 0);
            return elevation;
        }
    }
}