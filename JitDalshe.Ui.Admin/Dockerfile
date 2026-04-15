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
RUN dotnet publish "./JitDalshe.Ui.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY JitDalshe.Ui.Admin/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]