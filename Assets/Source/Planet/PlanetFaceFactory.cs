using Planets;
using UnityEngine;

public class PlanetFaceFactory
{
    private readonly PlanetFace _planetFacePrefab;

    public PlanetFaceFactory()
    {
        _planetFacePrefab = Resources.Load<PlanetFace>("PlanetFacePrefab");
    }

    public PlanetFace CreateFace(Transform parent, PlanetSettings settings)
    {
        PlanetFace planetFace = Object.Instantiate(_planetFacePrefab, parent);
        Shader planetShader = Shader.Find("Shader Graphs/Planet");
        planetFace.MeshRenderer.sharedMaterial = new Material(planetShader);
        planetFace.Construct(settings);
        return planetFace;
    }
}