using System.Collections.Generic;
using Unity.Collections;

namespace Planets
{
    public static class Extensions
    {
        public static bool Not(this bool value)
        {
            return !value;
        }
        
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) 
            where TValue : new()
        {
            if (!dict.TryGetValue(key, out TValue val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }
        
        public static PlanetSettingsDTO ToJobDTO(this PlanetSettings planetSettings)
        {
            return new PlanetSettingsDTO
            {
                Resolution = planetSettings.Resolution,
                Radius = planetSettings.Radius,
                Color = planetSettings.Color,
                NoiseLayers = new NativeArray<NoiseLayer>(planetSettings.NoiseLayers, Allocator.TempJob),
            };
        }   
    }
}