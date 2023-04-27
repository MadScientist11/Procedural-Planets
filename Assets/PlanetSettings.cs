using System;
using UnityEditor;
using UnityEngine;
namespace Planets
{
    [CreateAssetMenu(fileName = "PlanetSettings", menuName = "Planets/PlanetSettings")]
    public class PlanetSettings : ScriptableObject
    {
        public event Action<PlanetSettings> OnSettingsUpdated;

        public int Resolution = 10;
        public float Radius = 1;
        public Color Color;
        
        [Foldout("Noise Settings")]
        public float Frequency = 1;
        [Foldout("Noise Settings")]
        public float Persistence = 0.25f;
        [Foldout("Noise Settings")]
        public float Amplitude = 1;
        [Foldout("Noise Settings")]
        public int Octaves = 6;
        [Foldout("Noise Settings")]
        public int Seed = 1;
        
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

    public class FoldoutAttribute : Attribute
    {
        private readonly string _name;

        public string Name => _name;

        public FoldoutAttribute(string name)
        {
            _name = name;
        }
    }
}