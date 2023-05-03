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
        public Gradient ColorGraident;
        public bool AutoUpdate;

        public NoiseLayer[] NoiseLayers;


        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
            RaiseChangedEvent();
        }

        public void RaiseChangedEvent()
        {
            if(AutoUpdate.Not()) 
                return;
            
            OnSettingsUpdated?.Invoke(this);
        }
    }
}