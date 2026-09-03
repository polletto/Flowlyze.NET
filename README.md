# Flowlyze.NET

A modern .NET SDK for the [Flowlyze](https://flowlyze.com/) API.

> Early development preview. The public API may change before the first stable release.

## Goals

- Target modern .NET (`net10.0`)
- Support both currently documented Flowlyze authentication modes while the definitive contract is being confirmed
- Provide strongly typed clients for Flowlyze API resources
- Keep a low-level HTTP escape hatch for newly released endpoints
- Support cancellation throughout the SDK

## Current status

The repository contains the SDK foundation and the first typed Flow operation: `GET /api/flows/:id`.

The Flowlyze API reference documents resources including Batch, BatchTask, DataSpace, Destination, EditableFlow, Flow, GlobalVariable, Logs, MessageQueue, Platform, Queue, Statistic, Tenant and others.

## Authentication

The current Flowlyze documentation shows two authentication schemes in different sections. Flowlyze.NET therefore keeps authentication pluggable and currently supports both API-key and OAuth 2.0 client-credentials authentication.

### API key

The endpoint reference for `Flow - GetById` documents `x-api-key`, with optional `x-tenant-id` for multi-tenant/admin scenarios.

```csharp
using Flowlyze;
using Flowlyze.Authentication;

var clientOptions = new FlowlyzeClientOptions
{
    BaseAddress = new Uri("https://your-flowlyze-api-base-url/")
};

var authentication = new ApiKeyAuthenticationProvider(
    new ApiKeyAuthenticationOptions
    {
        ApiKey = "your-api-key",
        TenantId = "your-tenant-id" // optional when the API key is already tenant-scoped
    });

var flowlyze = new FlowlyzeClient(
    new HttpClient(),
    clientOptions,
    authentication);
```

### OAuth 2.0 client credentials

The general API documentation describes OAuth 2.0 `client_credentials` authentication through Auth0, using a Bearer token and an optional `tenant_id` header.

```csharp
using Flowlyze;
using Flowlyze.Authentication;

var clientOptions = new FlowlyzeClientOptions
{
    BaseAddress = new Uri("https://your-flowlyze-api-base-url/")
};

var authentication = new OAuthAuthenticationProvider(
    new HttpClient(),
    new OAuthAuthenticationOptions
    {
        ClientId = "your-client-id",
        ClientSecret = "your-client-secret",
        Audience = "your-api-audience",
        TenantId = "your-tenant-id"
    });

var flowlyze = new FlowlyzeClient(
    new HttpClient(),
    clientOptions,
    authentication);
```

The OAuth provider caches access tokens and refreshes them shortly before expiration.

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
