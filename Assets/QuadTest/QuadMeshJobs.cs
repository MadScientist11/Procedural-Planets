using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Planets;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;


public struct SingleStream : IMeshStreams
{
    [StructLayout(LayoutKind.Sequential)]
    struct Stream0
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }

    [NativeDisableContainerSafetyRestriction]
    private NativeArray<Stream0> _stream0;

    [NativeDisableContainerSafetyRestriction]
    private NativeArray<TriangleUInt16> _triangles;

    public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
    {
        var descriptor = new NativeArray<VertexAttributeDescriptor>(
            4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
        );
        descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
        descriptor[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension: 3
        );
        descriptor[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4
        );
        descriptor[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension: 2
        );
        meshData.SetVertexBufferParams(vertexCount, descriptor);
        descriptor.Dispose();

        meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
            {
                bounds = bounds,
                vertexCount = vertexCount
            },
            MeshUpdateFlags.DontRecalculateBounds |
            MeshUpdateFlags.DontValidateIndices);


        _stream0 = meshData.GetVertexData<Stream0>();
     
        _triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVertex(int index, Vertex vertex) => _stream0[index] = new Stream0
    {
        position = vertex.position,
        normal = vertex.normal,
        tangent = vertex.tangent,
        texCoord0 = vertex.texCoord0
    };

    public void SetTriangle(int index, int3 triangle) => _triangles[index] = triangle;
}

public interface IMeshStreams
{
    void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount);
    void SetVertex(int index, Vertex vertex);
    void SetTriangle(int index, int3 triangle);
}

public struct SquareGridData
{
    
}

public struct SquareGrid : IMeshGenerator<SquareGridData>
{
    

    public int VertexCount => 4 * Resolution * Resolution;

    public int IndexCount => 6 * Resolution * Resolution;

    public int JobLength => Resolution * Resolution;
    public int Resolution { get; set; }

    public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

    public void Setup(SquareGridData data)
    {
    }
    
    public void Execute<S>(int i, S streams) where S : struct, IMeshStreams
    {
        int vi = 4 * i, ti = 2 * i;

        int y = i / Resolution;
        int x = i - Resolution * (i / Resolution);
        
    

        var coordinates = new float4(x, x + 0.9f, y, y + 0.9f);
        var vertex = new Vertex();
        vertex.normal.z = -1f;
        vertex.tangent.xw = new float2(1f, -1f);
        vertex.position.xy = coordinates.xz;
        streams.SetVertex(vi, vertex);

        vertex.position.xy = coordinates.yz;
        vertex.texCoord0 = new float2(1f, 0f);
        streams.SetVertex(vi + 1, vertex);

        vertex.position.xy = coordinates.xw;
        vertex.texCoord0 = new float2(0f, 1f);
        streams.SetVertex(vi + 2, vertex);

        vertex.position.xy = coordinates.yw;
        vertex.texCoord0 = 1f;
        streams.SetVertex(vi + 3, vertex);

        streams.SetTriangle(ti + 0, vi + new int3(0, 2, 1));
        streams.SetTriangle(ti + 1, vi + new int3(1, 2, 3));
    }
}

public interface IMeshGenerator<T> where T : struct
{
    void Setup(T data);
    int VertexCount { get; }
    int IndexCount { get; }
    int JobLength { get; }
    Bounds Bounds { get; }
    int Resolution { get; set; }
    void Execute<S>(int z, S streams) where S : struct, IMeshStreams;
}

[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
public struct MeshJob<G, S, D> : IJobFor
    where G : struct, IMeshGenerator<D>
    where S : struct, IMeshStreams
    where D : struct
{
    private G _generator;
    [WriteOnly] private S _streams;

    public void Execute(int index) =>
        _generator.Execute(index, _streams);

    public static JobHandle ScheduleParallel(Mesh mesh,
        Mesh.MeshData meshData, int resolution, D data, JobHandle dependency
    ) 
    {
        MeshJob<G, S, D> job = new MeshJob<G, S, D>();
        job._generator.Setup(data);
        job._streams.Setup(
            meshData,
            mesh.bounds = job._generator.Bounds,
            job._generator.VertexCount,
            job._generator.IndexCount
        );
        return job.ScheduleParallel(job._generator.JobLength, 1, dependency);
    }
}

public interface IData
{
}

[StructLayout(LayoutKind.Sequential)]
public struct TriangleUInt16
{
    public ushort a, b, c;

    public static implicit operator TriangleUInt16(int3 t) => new TriangleUInt16
    {
        a = (ushort)t.x,
        b = (ushort)t.y,
        c = (ushort)t.z
    };
}

public class QuadMeshJobs : MonoBehaviour
{
    Mesh mesh;
    [SerializeField, Range(1, 10)] int resolution = 1;

    void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void OnValidate() => enabled = true;

    void Update()
    {
        GenerateMesh();
        enabled = false;
    }

    private void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        MeshJob<SquareGrid, SingleStream, SquareGridData>.ScheduleParallel(
            mesh, meshData, resolution,new SquareGridData(),default
        ).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
    }
}