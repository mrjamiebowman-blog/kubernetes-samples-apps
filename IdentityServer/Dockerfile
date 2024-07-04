# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000 

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityServer/MrJB.Kubernetes.IdentityServer.csproj", "IdentityServer/"]
COPY ["aspire/MrJB.Kubernetes.ServiceDefaults/MrJB.Kubernetes.ServiceDefaults.csproj", "aspire/MrJB.Kubernetes.ServiceDefaults/"]
RUN dotnet restore "./IdentityServer/MrJB.Kubernetes.IdentityServer.csproj"
COPY . .
WORKDIR "/src/IdentityServer"
RUN dotnet build "./MrJB.Kubernetes.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MrJB.Kubernetes.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MrJB.Kubernetes.IdentityServer.dll"]