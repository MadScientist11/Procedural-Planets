using UnityEngine;

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