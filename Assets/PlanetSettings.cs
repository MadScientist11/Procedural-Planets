using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Planets
{
    [CreateAssetMenu(fileName = "PlanetSettings", menuName = "Planets/PlanetSettings")]
    public class PlanetSettings : ScriptableObject
    {
        public event Action<PlanetSettings> OnSettingsUpdated;

        public int Resolution = 10;
        public float Radius = 1;
        public Color Color;

        public NoiseLayer[] NoiseLayers;


        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
            OnSettingsUpdated?.Invoke(this);
        }

        public void RaiseChangedEvent()
        {
            OnSettingsUpdated?.Invoke(this);
        }
    }

    public enum PlanetNoiseType
    {
        Simple = 0,
        Rigid = 1,
    }

    [Serializable]
    public struct NoiseLayer
    {
        public bool Enabled;
        [FormerlySerializedAs("UseAsLayerMask")] public bool UseFirstLayerAsMask;
        public NoiseSettings Settings;
    }

    [Serializable]
    public struct NoiseSettings
    {
        public PlanetNoiseType NoiseType;
        public SimpleNoiseSettings SimpleNoiseSettings;
        public RigidNoiseSettings RigidNoiseSettings;
    }

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

    public struct PlanetSettingsDTO
    {
        public int Resolution;
        public float Radius;
        public Color Color;
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<NoiseLayer> NoiseLayers;
    }
}