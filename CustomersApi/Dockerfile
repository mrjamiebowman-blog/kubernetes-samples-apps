# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# aspire
COPY ["aspire/MrJB.Kubernetes.ServiceDefaults/MrJB.Kubernetes.ServiceDefaults.csproj", "aspire/MrJB.Kubernetes.ServiceDefaults/"]
# app
COPY ["CustomersApi/MrJB.Kubernetes.Customers.Api.csproj", "CustomersApi/"]

RUN dotnet restore "./CustomersApi/MrJB.Kubernetes.Customers.Api.csproj"

COPY . .

RUN dotnet build "./CustomersApi/MrJB.Kubernetes.Customers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CustomersApi/MrJB.Kubernetes.Customers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MrJB.Kubernetes.Customers.Api.dll"]