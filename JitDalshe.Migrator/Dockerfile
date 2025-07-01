FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JitDalshe.Migrator/JitDalshe.Migrator.csproj", "JitDalshe.Migrator/"]
COPY ["Infrastructure/JitDalshe.Infrastructure.Persistence/JitDalshe.Infrastructure.Persistence.csproj", "Infrastructure/JitDalshe.Infrastructure.Persistence/"]
COPY ["Application/JitDalshe.Application/JitDalshe.Application.csproj", "Application/JitDalshe.Application/"]
COPY ["Domain/JitDalshe.Domain/JitDalshe.Domain.csproj", "Domain/JitDalshe.Domain/"]
RUN dotnet restore "JitDalshe.Migrator/JitDalshe.Migrator.csproj"
COPY . .
WORKDIR "/src/JitDalshe.Migrator"
RUN dotnet build "./JitDalshe.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JitDalshe.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JitDalshe.Migrator.dll"]
