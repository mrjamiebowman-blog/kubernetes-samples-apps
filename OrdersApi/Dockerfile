# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# aspire
COPY ["aspire/MrJB.Kubernetes.ServiceDefaults/MrJB.Kubernetes.ServiceDefaults.csproj", "aspire/MrJB.Kubernetes.ServiceDefaults/"]
# app
COPY ["OrdersApi/MrJB.Kubernetes.Orders.Api/MrJB.Kubernetes.Orders.Api.csproj", "OrdersApi/MrJB.Kubernetes.Orders.Api/"]
RUN dotnet restore "./OrdersApi/MrJB.Kubernetes.Orders.Api/MrJB.Kubernetes.Orders.Api.csproj"

COPY . .

WORKDIR "/src/OrdersApi/MrJB.Kubernetes.Orders.Api"

RUN dotnet build "./MrJB.Kubernetes.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MrJB.Kubernetes.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MrJB.Kubernetes.Orders.Api.dll"]