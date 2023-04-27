using UnityEngine;

namespace Planets
{
    public class Planet : MonoBehaviour
    {
        public PlanetSettings PlanetSettings;
        public PlanetPartPrefab PlanetPartPrefab;

        private PlanetGenerator _planetGenerator;

        private void OnEnable()
        {
            _planetGenerator = new PlanetGenerator(transform, PlanetPartPrefab);
            _planetGenerator.Initialize();
        
            PlanetSettings.OnSettingsUpdated += RecreatePlanet;
        }

        private void OnDisable() => 
            PlanetSettings.OnSettingsUpdated -= RecreatePlanet;

        private void RecreatePlanet(PlanetSettings settings)
        {
            _planetGenerator.CreatePlanet(settings);
        }

    }
}