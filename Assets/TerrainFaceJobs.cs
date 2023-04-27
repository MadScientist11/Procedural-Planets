using Unity.Mathematics;
using UnityEngine;

namespace Planets
{
    public struct TerrainFaceJobs : IMeshGenerator<TerrainFaceJobs.TerrainFaceData>
    {
        public struct TerrainFaceData : IData
        {
            public float3 LocalUp { get; }
            public int Resolution { get; }
            public float Radius { get; }
            public int Seed { get; set; }
            public int Octave { get; set; }
            public float Persistence { get; set; }
            public float Amplitude { get; set; }
            public float Frequency { get; set; }
            public float BaseAmplitude { get; set; }
            public float MinValue { get; set; }

            public TerrainFaceData(Vector3 localUp, PlanetSettings planetSettings)
            {
                LocalUp = localUp;
                Radius = planetSettings.Radius;
                Seed = planetSettings.Seed;
                Octave = planetSettings.Octaves;
                Persistence = planetSettings.Persistence;
                Amplitude = planetSettings.Amplitude;
                Frequency = planetSettings.Frequency;
                BaseAmplitude = planetSettings.Amplitude;
                MinValue = planetSettings.MinValue;
                Resolution = planetSettings.Resolution;
            }
        }

        private float3 AxisA => Data.LocalUp.yzx;
        private float3 AxisB => Vector3.Cross(Data.LocalUp, AxisA);

        public TerrainFaceData Data { get; private set; }


        public void Setup(TerrainFaceData data)
        {
            Data = data;
            Resolution = data.Resolution;
        }

        public int VertexCount => Resolution * Resolution;

        public int IndexCount => (Resolution - 1) * (Resolution - 1) * 6;
        public int JobLength => 1;
        public Bounds Bounds => new(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));
        public int Resolution { get; set; }

        public void Execute<S>(int index, S streams) where S : struct, IMeshStreams
        {
            int triIndex = 0;
            for (int y = 0; y < Resolution; y++)
            {
                for (int x = 0; x < Resolution; x++)
                {
                    int i = x + y * Resolution;
                    Vector2 percent = new Vector2(x, y) / (Resolution - 1);
                    Vector3 pointOnUnitCube =
                        Data.LocalUp + (percent.x - .5f) * 2 * AxisA + (percent.y - .5f) * 2 * AxisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    var vertex = new Vertex();
                    vertex.position = SamplePlanetPoint(pointOnUnitSphere, Data.Radius, Data.MinValue);
                    vertex.normal = Vector3.back;
                    streams.SetVertex(i, vertex);


                    if (LastRow(x).Not() && LastColumn(y).Not())
                    {
                        streams.SetTriangle(triIndex, new int3(i, i + Resolution + 1, i + Resolution));

                        streams.SetTriangle(triIndex + 1, new int3(i, i + 1, i + Resolution + 1));

                        triIndex += 2;
                    }
                }
            }
        }

        private float Noise3D(float3 point, float frequency, float amplitude, float persistence, int octave, int seed)
        {
            float noise = 0.0f;


            for (int i = 0; i < octave; ++i)
            {
                float snoise = Unity.Mathematics.noise.snoise(point * frequency + seed, out float3 grad);
                noise += (snoise + 1) * 0.5f * amplitude;
                amplitude *= persistence;
                frequency *= 2.0f;
            }

            return noise / octave;
        }

        private Vector3 SamplePlanetPoint(Vector3 pointOnUnitSphere, float radius, float minValue)
        {
            float elevation = Noise3D(pointOnUnitSphere, Data.Frequency, Data.BaseAmplitude, Data.Persistence,
                Data.Octave,
                Data.Seed) * Data.Amplitude;

            elevation = Mathf.Max(0, elevation - minValue);
            return pointOnUnitSphere * radius * (1 + elevation);
        }


        bool LastRow(int x)
        {
            return x == Resolution - 1;
        }

        bool LastColumn(int y)
        {
            return y == Resolution - 1;
        }
    }
}