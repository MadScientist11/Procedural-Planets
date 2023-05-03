using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

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
        Mesh.MeshData meshData, D data, JobHandle dependency
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