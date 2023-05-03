using System.Linq;
using Planets;
using UnityEngine;

public class PlanetGenerator
{
    private readonly Transform _meshParent;
    private readonly PlanetFaceFactory _planetFaceFactory;

    private PlanetFace[] _planetFaces;
    private PlanetSettings _settings;

    public PlanetGenerator(Transform meshParent, PlanetFaceFactory planetFaceFactory)
    {
        _meshParent = meshParent;
        _planetFaceFactory = planetFaceFactory;
    }

    public void Initialize(PlanetSettings settings)
    {
        _settings = settings;
        if (_planetFaces == null || _planetFaces.Length == 0)
            _planetFaces = new PlanetFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (_planetFaces[i] == null)
            {
                PlanetFace face = _planetFaceFactory.CreateFace(_meshParent, _settings);
                _planetFaces[i] = face;
                _planetFaces[i].MeshFilter.sharedMesh = new Mesh();
            }
        }
    }

    public void CreatePlanet()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        Texture2D colorTexture = CreateColorTexture();
        for (int i = 0; i < 6; i++)
        {
            CreateFaceMesh(_planetFaces[i].MeshFilter.sharedMesh, directions[i], _settings);
            _planetFaces[i].UpdatePlanetFace(colorTexture);
        }
    }

    private void CreateFaceMesh(Mesh mesh, Vector3 direction, PlanetSettings settings)
    {
        PlanetSettingsDto planetSettingsDto = settings.ToJobDTO();
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<TerrainFaceJobs, SingleStream, TerrainFaceJobs.TerrainFaceData>.ScheduleParallel(
            mesh, meshData, new TerrainFaceJobs.TerrainFaceData(direction, planetSettingsDto), default
        ).Complete();
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        mesh.RecalculateNormals();
    }

    private const int TextureResolution = 50;

    private Texture2D CreateColorTexture()
    {
        Color[] colours = new Color[TextureResolution];
        Texture2D colorTexture = new Texture2D(TextureResolution, 1);
        for (var i = 0; i < TextureResolution; i++)
        {
            colours[i] = _settings.ColorGraident.Evaluate(i / (TextureResolution - 1f));
        }

        colorTexture.SetPixels(colours);
        colorTexture.Apply();
        return colorTexture;
    }
}