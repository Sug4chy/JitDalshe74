FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JitDalshe.Ui.Admin/JitDalshe.Ui.Admin.csproj", "JitDalshe.Ui.Admin/"]
RUN dotnet restore "JitDalshe.Ui.Admin/JitDalshe.Ui.Admin.csproj"
COPY . .
WORKDIR "/src/JitDalshe.Ui.Admin"
RUN dotnet build "./JitDalshe.Ui.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JitDalshe.Ui.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JitDalshe.Ui.Admin.dll"]
