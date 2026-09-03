# Flowlyze.NET

A modern .NET SDK for the [Flowlyze](https://flowlyze.com/) API.

> Early development preview. The public API may change before the first stable release.

## Goals

- Target modern .NET (`net10.0`)
- Handle Flowlyze API-key authentication automatically
- Add the optional `x-tenant-id` header for multi-tenant/admin scenarios
- Provide strongly typed clients for Flowlyze API resources
- Keep a low-level HTTP escape hatch for newly released endpoints
- Support cancellation throughout the SDK

## Current status

The repository contains the SDK foundation and the first typed Flow operation: `GET /api/flows/:id`.

The Flowlyze API reference documents resources including Batch, BatchTask, DataSpace, Destination, EditableFlow, Flow, GlobalVariable, Logs, MessageQueue, Platform, Queue, Statistic, Tenant and others.

## Authentication

Flowlyze API endpoints use an API key sent through the `x-api-key` header.

When the authentication context is multi-tenant (for example an admin API key), the target tenant can be specified through the optional `x-tenant-id` header.

```csharp
using Flowlyze;

var options = new FlowlyzeClientOptions
{
    BaseAddress = new Uri("https://your-flowlyze-api-base-url/"),
    ApiKey = "your-api-key",
    TenantId = "your-tenant-id" // optional when the API key is already tenant-scoped
};

var httpClient = new HttpClient();
var flowlyze = new FlowlyzeClient(httpClient, options);
```

## Flow - GetById

```csharp
var flow = await flowlyze.Flows.GetByIdAsync(
    "flow-id",
    cancellationToken);
```

The documented response currently exposes `payload` and `metadata` objects. These are represented as `JsonElement?` until their complete schemas are mapped into strongly typed .NET models.

## Low-level requests

The low-level client remains available for endpoints that are not yet covered by a typed resource client:

```csharp
using var request = new HttpRequestMessage(HttpMethod.Get, "/api/example");
using var response = await flowlyze.SendAsync(request, cancellationToken: cancellationToken);

response.EnsureSuccessStatusCode();
```

## Roadmap

The initial API surface is planned around the most useful operational resources:

1. Flow
2. Batch / BatchTask
3. Logs
4. Platform
5. Destination

Additional Flowlyze resources will follow after the contracts are validated against the official API documentation.

## Documentation

- [Flowlyze API reference](https://doc.flowlyze.com/docs/api/)
- [Flow - GetById](https://doc.flowlyze.com/docs/api/flow/flow-get-by-id)

## Project status

Flowlyze.NET is an open-source .NET client project developed with permission to integrate with the Flowlyze public API. It is currently community-maintained unless stated otherwise by Flowlyze.
