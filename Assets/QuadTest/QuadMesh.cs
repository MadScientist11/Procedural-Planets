using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class QuadMesh : MonoBehaviour
{
    [SerializeField] private int variable;

    private void Start()
    {
        Mesh mesh = new Mesh()
        {
            name = "Quad"
        };

        Vector3[] verticies =
        {
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
        };

        int[] triangles = {
            2, 1, 0, 
            3, 1, 2
        };

        Vector3[] normals = new[]
        {
            Vector3.back,Vector3.back,Vector3.back,Vector3.back
        };

        Vector2[] uvs = new[]
        {   
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };
        mesh.SetVertices(verticies);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
    }
}