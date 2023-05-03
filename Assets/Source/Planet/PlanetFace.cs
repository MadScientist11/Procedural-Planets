using System.Linq;
using Planets;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlanetFace : MonoBehaviour
{
    public MeshFilter MeshFilter => _meshFilter ??= GetComponent<MeshFilter>();
    public MeshRenderer MeshRenderer => _meshRenderer ??= GetComponent<MeshRenderer>();

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private PlanetSettings _planetSettings;

    private static readonly int Min = Shader.PropertyToID("_Min");
    private static readonly int Max = Shader.PropertyToID("_Max");

    public void Construct(PlanetSettings planetSettings)
    {
        _planetSettings = planetSettings;
    }

    public void UpdatePlanetFace(Texture2D colorTexture)
    {
        MeshRenderer.sharedMaterial.SetTexture("_MainTex", colorTexture);
        UpdateHeightValues();
    }

    private void UpdateHeightValues()
    {
        float lowestVertex = float.MaxValue;
        float highestVertex = float.MinValue;
        float max = MeshFilter.sharedMesh.vertices.Max(Vector3.Magnitude);
        float min = MeshFilter.sharedMesh.vertices.Min(Vector3.Magnitude);
        highestVertex = Mathf.Max(highestVertex, max);
        lowestVertex = Mathf.Min(lowestVertex, min);
        Vector2 minMax = new Vector2(lowestVertex, highestVertex);
        MeshRenderer.sharedMaterial.SetFloat(Min, minMax.x);
        MeshRenderer.sharedMaterial.SetFloat(Max, minMax.y);
    }
}