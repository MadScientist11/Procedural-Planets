using Planets;
using UnityEngine;


public class PlanetGenerator
{
    private readonly Transform _meshParent;
    private readonly PlanetFaceFactory _planetFaceFactory;

    private PlanetFace[] _planetFaces;

    public PlanetGenerator(Transform meshParent, PlanetFaceFactory planetFaceFactory)
    {
        _meshParent = meshParent;
        _planetFaceFactory = planetFaceFactory;
    }

    public void Initialize()
    {
        if (_planetFaces == null || _planetFaces.Length == 0)
            _planetFaces = new PlanetFace[6];
        
        for (int i = 0; i < 6; i++)
        {
            if (_planetFaces[i] == null)
            {
                PlanetFace face = _planetFaceFactory.CreateFace(_meshParent);
                _planetFaces[i] = face;
                _planetFaces[i].MeshFilter.sharedMesh = new Mesh();
            }
        }
    }

    public void CreatePlanet(PlanetSettings settings)
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };


        for (int i = 0; i < 6; i++)
        {
            CreateFace(_planetFaces[i].MeshFilter.sharedMesh, directions[i], settings);
            _planetFaces[i].MeshRenderer.sharedMaterial.color = settings.Color;
        }
    }

    private void CreateFace(Mesh mesh, Vector3 direction, PlanetSettings settings)
    {
        PlanetSettingsDTO planetSettingsDto = settings.ToJobDTO();
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<TerrainFaceJobs, SingleStream, TerrainFaceJobs.TerrainFaceData>.ScheduleParallel(
            mesh, meshData,new TerrainFaceJobs.TerrainFaceData(direction, planetSettingsDto), default
        ).Complete();
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        mesh.RecalculateNormals();
    }
}