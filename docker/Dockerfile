FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Documents.sln ./
COPY src/Documents.Api/Documents.Api.csproj src/Documents.Api/
COPY src/Documents.Application/Documents.Application.csproj src/Documents.Application/
COPY src/Documents.Core/Documents.Core.csproj src/Documents.Core/
COPY src/Documents.Infrastructure/Documents.Infrastructure.csproj src/Documents.Infrastructure/
COPY src/Documents.Tests/Documents.Tests.csproj src/Documents.Tests/

RUN dotnet restore Documents.sln

COPY . .

RUN dotnet publish src/Documents.Api/Documents.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ARG API_PORT=8080
ENV API_PORT=${API_PORT}

EXPOSE ${API_PORT}

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${API_PORT}
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "Documents.Api.dll"]