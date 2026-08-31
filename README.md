# .NET 8 Enterprise CRUD API

A practical ASP.NET Core 8 Web API using SQL Server + EF Core, Repository Pattern, Service Layer, async/await, DI lifetimes, custom middleware, custom action filter and built-in API rate limiting.

## Architecture
Controller -> Service -> Repository -> EF Core DbContext -> SQL Server

## Features
- Real SQL Server database communication
- EF Core migrations
- CRUD Product API
- Repository Pattern
- Service layer
- All DB calls async with CancellationToken
- Transient / Scoped / Singleton lifetime demo
- Custom exception handling middleware
- Custom request timing middleware
- Custom `IAsyncActionFilter`
- .NET 8 fixed-window rate limiting
- Swagger

## Setup
1. Install .NET 8 SDK and SQL Server/LocalDB.
2. Update `appsettings.json` connection string if needed.
3. Run:
```bash
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```
4. Open Swagger at the URL shown by the application.

## Endpoints
- GET `/api/products`
- GET `/api/products/{id}`
- POST `/api/products`
- PUT `/api/products/{id}`
- DELETE `/api/products/{id}`
- GET `/api/diagnostics/lifetimes`

## Example POST
```json
{"name":"Laptop","price":75000,"stock":10}
```

## Rate limiting
The Products controller uses the `api` fixed-window policy. Default: 10 requests per 60 seconds. Change `RateLimit` in `appsettings.json`.

## Middleware
`ExceptionHandlingMiddleware` catches validation and unhandled exceptions and returns consistent JSON. `RequestTimingMiddleware` logs request duration.

## Filter
`RequestLoggingFilter` demonstrates an async action filter running before and after controller actions.

## Service lifetimes
`/api/diagnostics/lifetimes` returns GUIDs for two injected instances of each lifetime. Transient normally differs, scoped matches within a request, and singleton remains the same across requests.

## Interview flow
Request -> Rate Limiter -> Middleware -> MVC Filter -> Controller -> Service -> Repository -> EF Core -> SQL Server -> back through the layers.



