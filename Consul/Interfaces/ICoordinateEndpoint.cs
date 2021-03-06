﻿using System.Threading;
using System.Threading.Tasks;

namespace NConsul.Interfaces
{
    public interface ICoordinateEndpoint
    {
        Task<QueryResult<CoordinateDatacenterMap[]>> Datacenters(CancellationToken ct = default(CancellationToken));
        Task<QueryResult<CoordinateEntry[]>> Nodes(CancellationToken ct = default(CancellationToken));
        Task<QueryResult<CoordinateEntry[]>> Nodes(QueryOptions q, CancellationToken ct = default(CancellationToken));
    }
}