using UnityEngine;

public class PlanetFaceFactory
{
    private readonly PlanetFace _planetFacePrefab;

    public PlanetFaceFactory()
    {
        _planetFacePrefab = Resources.Load<PlanetFace>("PlanetFacePrefab");
    }

    public PlanetFace CreateFace(Transform parent)
    {
        return Object.Instantiate(_planetFacePrefab, parent);
    }
}