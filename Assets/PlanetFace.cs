using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlanetFace : MonoBehaviour
{
    public MeshFilter MeshFilter => GetComponent<MeshFilter>();
    public MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();

    private void Awake()
    {
        MeshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
    }
}
