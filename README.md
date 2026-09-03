# Flowlyze.NET

A modern .NET SDK for the [Flowlyze](https://flowlyze.com/) API.

> Early development preview. The public API may change before the first stable release.

## Goals

- Target modern .NET (`net10.0`)
- Handle Flowlyze OAuth 2.0 client-credentials authentication automatically
- Cache and refresh access tokens
- Add the required `tenant_id` header automatically
- Provide strongly typed clients for Flowlyze API resources
- Keep a low-level HTTP escape hatch for newly released endpoints
- Support cancellation throughout the SDK

## Current status

The repository currently contains the SDK foundation. Resource-specific clients such as Flow, Batch, Logs and Platform will be added incrementally.

The Flowlyze API reference currently documents resources including Batch, BatchTask, DataSpace, Destination, EditableFlow, Flow, GlobalVariable, Logs, MessageQueue, Platform, Queue, Statistic, Tenant and others.

## Authentication

Flowlyze uses OAuth 2.0 with the `client_credentials` grant. Access tokens are obtained from Auth0 and API calls require both a Bearer token and a `tenant_id` header.

```csharp
using Flowlyze;
using Flowlyze.Authentication;

var authOptions = new FlowlyzeAuthenticationOptions
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Audience = "your-api-audience"
};

var clientOptions = new FlowlyzeClientOptions
{
    BaseAddress = new Uri("https://your-flowlyze-api-base-url/"),
    TenantId = "your-tenant-id"
};

var authHttpClient = new HttpClient();
var apiHttpClient = new HttpClient();

using var tokenProvider = new Auth0AccessTokenProvider(authHttpClient, authOptions);
var flowlyze = new FlowlyzeClient(apiHttpClient, tokenProvider, clientOptions);
```

Until strongly typed resource clients are added, the low-level client can send authenticated Flowlyze requests:

```csharp
using var request = new HttpRequestMessage(HttpMethod.Get, "api/example");
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

Additional Flowlyze resources will follow after the core contracts are validated against the official API documentation.

## Documentation

- [Flowlyze API reference](https://doc.flowlyze.com/docs/api/)

## Project status

Flowlyze.NET is an open-source .NET client project developed with permission to integrate with the Flowlyze public API. It is currently community-maintained unless stated otherwise by Flowlyze.
