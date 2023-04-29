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
        _planetSettings.OnSettingsUpdated += UpdateHeightValues;
    }

    private void OnDestroy() =>
        _planetSettings.OnSettingsUpdated -= UpdateHeightValues;

    private void UpdateHeightValues(PlanetSettings settings)
    {
        Vector2 heightMapMinMax = settings.MinMaxHeight.CalculateHeightMapRange(settings.Radius);
        MeshRenderer.sharedMaterial.SetFloat(Min, heightMapMinMax.x);
        MeshRenderer.sharedMaterial.SetFloat(Max, heightMapMinMax.y);
    }
}