using UnityEngine;

namespace Planets
{
    public class Planet : MonoBehaviour
    {
        [SerializeField] private PlanetSettings _planetSettings;
        private PlanetGenerator _planetGenerator;

        private void OnEnable()
        {
            _planetGenerator = new PlanetGenerator(transform, new PlanetFaceFactory());
            _planetGenerator.Initialize();
            
            _planetSettings.OnSettingsUpdated += RecreatePlanet;
        }

        private void OnDisable() =>
            _planetSettings.OnSettingsUpdated -= RecreatePlanet;

        private void RecreatePlanet(PlanetSettings settings) => 
            _planetGenerator.CreatePlanet(settings);
    }
}