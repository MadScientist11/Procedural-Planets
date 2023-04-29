using System;
using UnityEngine;

namespace Planets
{
    public class Planet : MonoBehaviour
    {
        public int Value;
        [Foldout("Value", typeof(PlanetSettings))]
        [SerializeField] private PlanetSettings _planetSettings;
        private PlanetGenerator _planetGenerator;

        private void OnEnable()
        {
            _planetGenerator = new PlanetGenerator(transform, new PlanetFaceFactory());
            _planetGenerator.Initialize(_planetSettings);

            _planetSettings.OnSettingsUpdated += RecreatePlanet;
        }

        private void OnDisable() =>
            _planetSettings.OnSettingsUpdated -= RecreatePlanet;

        private void RecreatePlanet(PlanetSettings settings) =>
            _planetGenerator.CreatePlanetMesh();
    }

    [Serializable]
    public struct PlanetMinMax
    {
        [SerializeField] private Vector2 _minMaxRange;
        
        public Vector2 CalculateHeightMapRange(float radius)
        {
            return _minMaxRange + radius * Vector2.one;
        }
    }
}