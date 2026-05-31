FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JitDalshe.Api.VkCallback/JitDalshe.Api.VkCallback.csproj", "JitDalshe.Api.VkCallback/"]
COPY ["Application/JitDalshe.Application.VkCallback/JitDalshe.Application.VkCallback.csproj", "Application/JitDalshe.Application.VkCallback/"]
COPY ["Application/JitDalshe.Application/JitDalshe.Application.csproj", "Application/JitDalshe.Application/"]
COPY ["Domain/JitDalshe.Domain/JitDalshe.Domain.csproj", "Domain/JitDalshe.Domain/"]
COPY ["Infrastructure/JitDalshe.Infrastructure.Persistence/JitDalshe.Infrastructure.Persistence.csproj", "Infrastructure/JitDalshe.Infrastructure.Persistence/"]
COPY ["JitDalshe.Api/JitDalshe.Api.csproj", "JitDalshe.Api/"]
RUN dotnet restore "JitDalshe.Api.VkCallback/JitDalshe.Api.VkCallback.csproj"
COPY . .
WORKDIR "/src/JitDalshe.Api.VkCallback"
RUN dotnet build "./JitDalshe.Api.VkCallback.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JitDalshe.Api.VkCallback.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JitDalshe.Api.VkCallback.dll"]
