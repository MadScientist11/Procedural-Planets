using Planets;
using UnityEngine;

public class PlanetGenerator
{
    private readonly Transform _meshParent;
    private readonly PlanetPartPrefab _planetPartPrefab;

    private PlanetPartPrefab[] _planetParts;

    public PlanetGenerator(Transform meshParent, PlanetPartPrefab planetPartPrefab)
    {
        _meshParent = meshParent;
        _planetPartPrefab = planetPartPrefab;
    }

    public void Initialize()
    {
        if (_planetParts == null || _planetParts.Length == 0)
            _planetParts = new PlanetPartPrefab[6];

        for (int i = 0; i < 6; i++)
        {
            if (_planetParts[i] == null)
            {
                PlanetPartPrefab face = Object.Instantiate(_planetPartPrefab, _meshParent);
                _planetParts[i] = face;
                _planetParts[i].MeshFilter.sharedMesh = new Mesh();
            }
        }
    }

    public void CreatePlanet(PlanetSettings settings)
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            CreateFace(_planetParts[i].MeshFilter.sharedMesh, directions[i], settings);
            _planetParts[i].MeshRenderer.sharedMaterial.color = settings.Color;
        }
    }

    private void CreateFace(Mesh mesh, Vector3 direction, PlanetSettings settings)
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<TerrainFaceJobs, SingleStream, TerrainFaceJobs.TerrainFaceData>.ScheduleParallel(
            mesh, meshData, settings.Resolution, new TerrainFaceJobs.TerrainFaceData(direction, settings.Radius, settings.Frequency, settings.Amplitude, settings.Persistence, settings.Seed, settings.Octaves), default
        ).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        mesh.RecalculateNormals();
    }
}