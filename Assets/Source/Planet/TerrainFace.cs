using Unity.Collections;
using UnityEngine;

namespace Planets
{


    public class TerrainFace
    {
        private readonly Mesh _mesh;
        private readonly int _resolution;
        private readonly Vector3 _localUp;

        private readonly Vector3 _axisA;
        private readonly Vector3 _axisB;

        public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
        {
            _mesh = mesh;
            _resolution = resolution;
            _localUp = localUp;

            _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            _axisB = Vector3.Cross(localUp, _axisA);
        }


        public void ConstructMesh()
        {
            Vector3[] verticies = new Vector3[_resolution * _resolution];
            int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];

            int triIndex = 0;
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    int i = x + y * _resolution;
                    Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                    Vector3 pointOnUnitCube =
                        _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    verticies[i] = pointOnUnitSphere;
                    if (LastRow(x).Not() && LastColumn(y).Not())
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + _resolution + 1;
                        triangles[triIndex + 2] = i + _resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + _resolution + 1;
                        triIndex += 6;
                    }
                }
            }

            _mesh.Clear();
            _mesh.vertices = verticies;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

        bool LastRow(int x)
        {
            return x == _resolution - 1;
        }

        bool LastColumn(int y)
        {
            return y == _resolution - 1;
        }
    }
}