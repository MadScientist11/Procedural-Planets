using System;
using UnityEditor;
using UnityEngine;

namespace Planets
{
    public class PlanetOld : MonoBehaviour
    {
        [SerializeField] private PlanetSettings _planetSettings;

        [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;
        private TerrainFace[] _terrainFaces;


        private void OnValidate()
        {
            if (_planetSettings != null)
                GeneratePlanet(_planetSettings);
        }

        private void GeneratePlanet(PlanetSettings planetSettings)
        {
            Initialize(planetSettings);
            GenerateMesh();
        }

        private void Initialize(PlanetSettings planetSettings)
        {
            if (_meshFilters == null || _meshFilters.Length == 0)
                _meshFilters = new MeshFilter[6];

            _terrainFaces = new TerrainFace[6];

            Vector3[] directions =
                { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }

                _terrainFaces[i] =
                    new TerrainFace(_meshFilters[i].sharedMesh, planetSettings.Resolution, directions[i]);
            }
        }


        private void GenerateMesh()
        {
            foreach (TerrainFace face in _terrainFaces)
            {
                face.ConstructMesh();
            }
        }
    }
}