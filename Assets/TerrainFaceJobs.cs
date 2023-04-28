using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Planets
{
    public struct TerrainFaceJobs : IMeshGenerator<TerrainFaceJobs.TerrainFaceData>
    {
        public struct TerrainFaceData : IData
        {
            public PlanetSettingsDTO PlanetSettings { get; }
            public float3 LocalUp { get; }
            public int Resolution { get; }
            public float Radius { get; }


            public TerrainFaceData(Vector3 localUp, PlanetSettingsDTO planetSettings)
            {
                PlanetSettings = planetSettings;
                LocalUp = localUp;
                Radius = planetSettings.Radius;
                Resolution = planetSettings.Resolution;
            }
        }

        private float3 AxisA => Data.LocalUp.yzx;
        private float3 AxisB => Vector3.Cross(Data.LocalUp, AxisA);

        public TerrainFaceJobs.TerrainFaceData Data { get; private set; }


        public void Setup(TerrainFaceJobs.TerrainFaceData data)
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
                    float3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    Vertex vertex = new Vertex();
                    vertex.position = SamplePlanetPoint(pointOnUnitSphere, Data.PlanetSettings);
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

        private float3 SamplePlanetPoint(float3 point, PlanetSettingsDTO planetSettings)
        {
            float elevation = 0;
            float firstLayerMask = 0;

            if (planetSettings.NoiseLayers.Length > 0)
            {
                firstLayerMask = CalculateNoiseValue(point, planetSettings.NoiseLayers[0]);
                if (planetSettings.NoiseLayers[0].Enabled)
                {
                    elevation = firstLayerMask;
                }
            }


            for (var i = 1; i < planetSettings.NoiseLayers.Length; i++)
            {
                NoiseLayer noiseLayer = planetSettings.NoiseLayers[i];

                if (planetSettings.NoiseLayers[i].Enabled)
                {
                    elevation += CalculateNoiseValue(point, noiseLayer);
                }
            }

            return point * planetSettings.Radius * (1 + elevation);
        }

        private float CalculateNoiseValue(float3 point, NoiseLayer noiseLayer)
        {
            return noiseLayer.Settings.NoiseType switch
            {
                PlanetNoiseType.Simple =>
                    NoiseHelpers.SampleSimpleNoise(point, noiseLayer.Settings.SimpleNoiseSettings),
                PlanetNoiseType.Rigid => 
                    NoiseHelpers.SampleRigidNoise(point, noiseLayer.Settings.RigidNoiseSettings),
                _ => throw new ArgumentOutOfRangeException()
            };
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